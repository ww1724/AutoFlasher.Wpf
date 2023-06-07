using AutoFlasher.Wpf.Components;
using AutoFlasher.Wpf.Helpers;
using AutoFlasher.Wpf.Interfaces;
using AutoFlasher.Wpf.Models;
using Caliburn.Micro;
using Microsoft.Win32;
using Newtonsoft.Json;
using NModbus;
using NModbus.IO;
using NModbus.Serial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AutoFlasher.Wpf.ViewModels
{

    public class ShellViewModel : Screen, IViewModel
    {
        private Timer Timer { get; set; }

        public dynamic ConfigObject { get; set; }

        private ILoggerService LoggerService
            => IoC.Get<ILoggerService>();

        IModbusMaster PlcMaster { get; set; }

        private static ModbusFactory ModbusFactory;

        public ShellViewModel()
        {
            ModbusFactory = new ModbusFactory();
            Records = new ObservableCollection<FlasherRecord>();
            FirewareList = new ObservableCollection<FirewareItem>();
            AvailablePorts = new ObservableCollection<string>();

            _ = ReloadConfig();
            Application.Current.Dispatcher.UnhandledException += (object sender, DispatcherUnhandledExceptionEventArgs e) =>
            {
                LoggerService.Error(e.Exception.ToString());
                MessageBox.Show("Application Error\r\n" + e.Exception.ToString());
            };
        }

        public SerialPort ModbusSerial { get; set; }

        #region Private
        private List<Task> FlashTasks = new List<Task>();
        private string chipType;
        private bool isShowFirewareFullName;
        private bool isAutoParseFirewareOffest;
        private string startSignal;
        private string finishSignal;
        private string stopSignal;
        private string flashStateSignalStartIndex;
        private string themeColor;
        private string firewareParseOffsetString;
        private bool isAutoFlashing;
        private ObservableCollection<FirewareItem> firewareList;
        private string spiMode;
        private string flashSize;
        private string spiSpeed;
        private string crystalFrequency;
        private ObservableCollection<FlasherRecord> records;
        private AutoFlashState autoState;
        private ObservableCollection<string> availablePorts;
        private string defaultBaud;
        private bool isAllAvailable = true;
        private Dictionary<string, Process> flashProcessTable = new();

        public Dictionary<string, Process> FlashProcessTable { get => flashProcessTable; set
            {
                flashProcessTable = value;
                if (flashProcessTable.Count == 0)
                    IsAllAvailable= true;
                else 
                    IsAllAvailable= false;
            }
        }
        #endregion

        // view models
        #region State
        public bool IsAllAvailable { get => isAllAvailable; set { isAllAvailable = value; NotifyOfPropertyChange(); } }

        FlasherRecord LeftRecord { get; set; }
        FlasherRecord RightRecord { get; set; }
        #endregion

        #region Config

        #region Flash Config

        public string ChipType { get => chipType; set { chipType = value; NotifyOfPropertyChange(); } }

        public bool IsShowFirewareFullName { get => isShowFirewareFullName; set { isShowFirewareFullName = value; NotifyOfPropertyChange(); } }

        public bool IsAutoParseFirewareOffest { get => isAutoParseFirewareOffest; set { isAutoParseFirewareOffest = value; NotifyOfPropertyChange(); } }

        public string FirewareParseOffsetString { get => firewareParseOffsetString; set { firewareParseOffsetString = value; NotifyOfPropertyChange(); } }

        public string SpiMode { get => spiMode; set { spiMode = value; NotifyOfPropertyChange(); } }

        public string FlashSize { get => flashSize; set { flashSize = value; NotifyOfPropertyChange(); } }

        public string SpiSpeed { get => spiSpeed; set { spiSpeed = value; NotifyOfPropertyChange(); } }

        public string CrystalFrequency { get => crystalFrequency; set { crystalFrequency = value; NotifyOfPropertyChange(); } }

        public string DefaultBaud { get => defaultBaud; set { defaultBaud = value; NotifyOfPropertyChange(); } }

        #endregion
        #region Auto Flash Config

        public string StartSignal { get => startSignal; set { startSignal = value; NotifyOfPropertyChange(); } }
        public string StopSignal { get => stopSignal; set { stopSignal = value; NotifyOfPropertyChange(); } }
        public string FinishSignal { get => finishSignal; set { finishSignal = value; NotifyOfPropertyChange(); } }
        public string FlashStateSignalStartIndex { get => flashStateSignalStartIndex; set { flashStateSignalStartIndex = value; NotifyOfPropertyChange(); } }

        #endregion
        #region AppConfig
        public string ThemeColor { get => themeColor; set { themeColor = value; NotifyOfPropertyChange(); } }
        #endregion
        #endregion

        #region Variable
        public bool IsAutoFlashing { get => isAutoFlashing; set { isAutoFlashing = value; NotifyOfPropertyChange(); } }

        public AutoFlashState AutoState { get => autoState; set { OnAutoStateChanged(value, autoState); autoState = value; NotifyOfPropertyChange(); ; } }

        private void OnAutoStateChanged(AutoFlashState newVal, AutoFlashState oldVal)
        {
            // 烧录完成收尾
            if (oldVal == AutoFlashState.Flashing && newVal == AutoFlashState.Finished)
            {
                autoState = AutoFlashState.WaitingStartSignal;
            } 
        }

        public ObservableCollection<FirewareItem> FirewareList { get => firewareList; set { firewareList = value; NotifyOfPropertyChange(); } }

        public ObservableCollection<FlasherRecord> Records { get => records; set { records = value; NotifyOfPropertyChange(); } }

        public ObservableCollection<string> AvailablePorts { get => availablePorts; set { availablePorts = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Actions
        #region 烧录
        public void StartAutoFlash()
        {
            IsAutoFlashing = true;
            var leftRecord = Records.Where(x => x.PortType == 1).FirstOrDefault();
            var rightRecord = Records.Where(x => x.PortType == 2).FirstOrDefault();
            if (leftRecord == null || rightRecord == null)
            {
                MessageBox.Show("串口未配置好");
                AutoState = AutoFlashState.Idle;
                IsAutoFlashing = false;
                return;
            }

            AutoState = AutoFlashState.WaitingStartSignal;

            AutoFlashThreadAction();
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;
            byte data = 0;
            int len = 0;
            int bufsize = serialPort.BytesToRead;
            string a = "";
            while (len < bufsize)
            {
                data = (byte)serialPort.ReadByte();
                len++;
                a += Convert.ToString(data, 16).ToUpper();
                a += " "; 
            }
            serialPort.DiscardInBuffer();
            byte commandType =  Convert.ToByte(a.Split(" ")[1], 16);
            //{
            //    //StartFlashToRecord(records.FirstOrDefault());
            //    autoState = AutoFlashState.Idle;
            //}

            if (a.Split(" ")[1] == "1")
            {
                // 当前为等待开始
                if (AutoState == AutoFlashState.WaitingStartSignal)
                {
                    if (a.Split(' ')[3] == "1")
                    {
                        AutoState = AutoFlashState.ReadyToFlash;
                    }
                }

            }
                
        }

        public void StopAutoFlash()
        {
            IsAutoFlashing = false;
            AutoState = AutoFlashState.Stop;
        }

        public void StartAllFlash()
        {
            if (firewareList.Count == 0)
            {
                MessageBox.Show("请先导入固件");
                return;
            }
            foreach (var record in Records)
            {
                StartFlashToRecord(record);
            }
        }

        public void StopAllFlash()
        {
            try
            {
                foreach (var item in FlashProcessTable.Keys)
                {
                    if (item == null)
                        continue;

                    FlasherRecord record = Records.FirstOrDefault(x => x.ComPortName == item);
                    StopOperationToRecord(record);
                }
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        public void EraseAllFlash()
        {
            foreach (var item in Records)
            {
                EraseFlashToRecord(item);
            }
        }

        public void StartFlashToRecord(FlasherRecord record)
        {
            if (firewareList.Count == 0)
            {
                MessageBox.Show("请先导入固件");
                if (IsAutoFlashing) IsAutoFlashing = false;
                return;
            }
            if (record.IsAvailable)
                StartFlashToPortFunc(record);
        }

        public void StopOperationToRecord(FlasherRecord record)
        {
            if (record.IsAvailable)
                return;
            StopOperationToRecordFunc(record);
        }

        public void EraseFlashToRecord(FlasherRecord record)
        {
            if (!record.IsAvailable == true)
                return;
            record.IsAvailable = false;
            EraseFlashToPortFunc(record);
        }

        public void ClearInfoToRecord(FlasherRecord record)
        {
            record.Logs = string.Empty;
            record.SuccessfulNumber = 0;
            record.TotalNumber = 0;
        }

        public async void ImportFirewareAction()
        {
            try
            {
                string formatter = FirewareParseOffsetString;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Bin files (*.bin)|*.bin|All files (*.*)|*.*";
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "选择固件";
                var dResult = openFileDialog.ShowDialog();
                if (dResult == true)
                {
                    IList<FirewareItem> items = new List<FirewareItem>();
                    foreach (var file in openFileDialog.FileNames)
                    {
                        Regex regex = new Regex(formatter);
                        Match match = regex.Match(file);
                        string offset = match.Success ? match.Value : "";
                        items.Add(new FirewareItem { Enable = true, FileName = file.Split("\\").Last(), FirewareFile = file, Offset = offset });
                    }
                    FirewareList = new ObservableCollection<FirewareItem>(items);
                }
                await RestoreConfig();
            }
            catch (Exception ex)
            {
                LoggerService.Error(ex.Message, ex);
            }
        }

        public void RemoveFireware(FirewareItem firewareItem)
        {
            firewareList.Remove(firewareItem);
        }

        public void StartCombineBinAction()
        {
            if (firewareList.Count == 0)
            {
                MessageBox.Show("请先导入固件");
                return;
            }
            //StartCombineBinFunc();
        }

        public void ScanComs()
        {
            string[] ports = SerialPort.GetPortNames();
            AvailablePorts = new ObservableCollection<string>(ports);
            List<FlasherRecord> flashToDelete = new List<FlasherRecord>();
            foreach (var record in Records)
            {
                if (!ports.ToList().Contains(record.ComPortName))
                    flashToDelete.Add(record);
            }

            foreach (var record in flashToDelete)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Records.Remove(record);
                });
            }

            foreach (var port in ports)
            {
                if (!Records.Any(x => x.ComPortName == port))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Records.Add(new FlasherRecord
                        {
                            ComPortName = port,
                            State = FlashRecordState.Idle,
                            BaudRate = defaultBaud,
                            Logs = "",
                            Progress = 0,
                        });
                    });
                }
            }
        }
        #endregion

        #region 配置
        public async Task ReloadConfig()
        {
            try
            {
                string json = "";
                await Task.Run(async () =>
                {
                    var path = Environments.ConfigFilePath;
                    if (!File.Exists(path))
                    {
                        using (FileStream file = File.Create(path))
                        {
                            file.Close();
                        };
                        await LoadDefaultConfig();
                        return;
                    }
                    json = await File.ReadAllTextAsync(Environments.ConfigFilePath);
                    if (json == "")
                    {
                        await LoadDefaultConfig();
                        return;
                    }
                    #region Configs
                    ConfigObject = JsonConvert.DeserializeObject(json);

                    ChipType = ConfigObject.ChipType;
                    IsShowFirewareFullName = ConfigObject.IsShowFirewareFullName;
                    IsAutoParseFirewareOffest = ConfigObject.IsAutoParseFirewareOffest;
                    FirewareParseOffsetString = ConfigObject.FirewareParseOffsetString;
                    SpiMode = ConfigObject.SpiMode;
                    SpiSpeed = ConfigObject.SpiSpeed;
                    FlashSize = ConfigObject.FlashSize;
                    CrystalFrequency = ConfigObject.CrystalFrequency;
                    DefaultBaud = ConfigObject.DefaultBaud;

                    StartSignal = ConfigObject.StartSignal;
                    StopSignal = ConfigObject.StopSignal;
                    FinishSignal = ConfigObject.FinishSignal;
                    FlashStateSignalStartIndex = ConfigObject.FlashStateSignalStartIndex;

                    Dictionary<string, string> tempList = ConfigObject.FirewareList.ToObject<Dictionary<string, string>>();
                    FirewareList = new ObservableCollection<FirewareItem>(tempList.Select((x) => new FirewareItem { Enable = true, FileName = x.Key.Split("\\").Last(), FirewareFile = x.Key, Offset = x.Value }));

                    ThemeColor = ConfigObject.ThemeColor;


                    #endregion
                });
            }
            catch (Exception e)
            {
                LoggerService.Error(e.Message, e);
            }
        }

        public async Task<object> RestoreConfig()
        {
            await Task.Run(() =>
            {
                try
                {
                    var tempFireList = firewareList.ToList().ToDictionary(x => x.FirewareFile, x => x.Offset);
                    dynamic config = new
                    {
                        ChipType,
                        IsShowFirewareFullName,
                        IsAutoParseFirewareOffest,
                        FirewareParseOffsetString,
                        SpiMode,
                        SpiSpeed,
                        FlashSize,
                        CrystalFrequency,
                        DefaultBaud,

                        StartSignal,
                        StopSignal,
                        FinishSignal,
                        FlashStateSignalStartIndex,

                        ThemeColor,

                        FirewareList = tempFireList
                    };
                    string json = JsonConvert.SerializeObject(config);

                    File.WriteAllText(Environments.ConfigFilePath, json);
                }
                catch (Exception e)
                {
                    LoggerService.Error(e.ToString(), e);
                }
            });

            return Task.FromResult(0);
        }

        public async Task LoadDefaultConfig()
        {
            try
            {
                var config = new
                {
                    ChipType = EspFlashHelper.DefaultChipType,
                    IsShowFirewareFullName = EspFlashHelper.DefaultIsShowFirewareFullName,
                    IsAutoParseFirewareOffest = EspFlashHelper.DefaultIsAutoParseFirewareOffest,
                    FirewareParseOffsetString = EspFlashHelper.DefaultFirewareParseOffsetString,
                    SpiMode = EspFlashHelper.DefaultSpiMode,
                    SpiSpeed = EspFlashHelper.DefaultSpiSpeed,
                    FlashSize = EspFlashHelper.DefaultFlashSize,
                    CrystalFrequency = EspFlashHelper.DefaultCrystalFrequency,
                    DefaultBaud = EspFlashHelper.DefaultBaud,

                    StartSignal = EspFlashHelper.DefaultStartSignal,
                    StopSignal = EspFlashHelper.DefaultStopSignal,
                    FinishSignal = EspFlashHelper.DefaultFinishSignal,
                    FlashStateSignalStartIndex = EspFlashHelper.DefaultFlashStateSignalStartIndex,

                    ThemeColor = "Blue",
                    FirewareList = new List<FirewareItem>()
                };

                await Task.Run(() =>
                {
                    string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                    File.WriteAllText(Environments.ConfigFilePath, json);
                });

                await ReloadConfig();
            }
            catch (Exception e)
            {
                LoggerService.Error(e.Message, e);
            }
        }

        public void ImportConfig()
        {

        }
        #endregion

        #region 其他
        public void ChangeThemeColor(string color)
        {
            var colors = new string[] { "Pink", "Red", "Blue", "Orange", "Green", "Purple", "Yellow" };
            if (colors.Contains(color))
            {
                var resources = Application.Current.Resources.MergedDictionaries;
                resources[0].Source = new System.Uri($"pack://application:,,,/Zoranof.UI.Wpf;component/Themes/{color}.xaml");
                ThemeColor = color;
            }
        }
        #endregion

        #endregion

        #region Functions

        public void StartFlashToPortFunc(FlasherRecord record)
        {
            record.StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000 * 1.0;
            List<string> commands = new List<string>
            {
                $"--chip {chipType}",
                $"--port {record.ComPortName}",
                $"--baud {record.BaudRate}",
                $"--before default_reset --after hard_reset",
                "write_flash",
                $"--flash_freq {CrystalFrequency.ToLower()}",
                $"--flash_mode {SpiMode.ToLower()}",
                $"--flash_size {FlashSize}",
            };
            commands.AddRange(FirewareList.Select(x => x.Enable ? $"{x.Offset} \"{x.FirewareFile}\"" : null));
            string command = string.Join(" ", commands);
            RunCommandAsync(command, "write", record);
        }

        public async void StopOperationToRecordFunc(FlasherRecord record)
        {
            if (!FlashProcessTable.ContainsKey(record.ComPortName))
                return;
            try
            {
                await Task.Run(() =>
                {
                    var taskKillProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "taskkill",
                            Arguments = $"/F /T /PID {FlashProcessTable[record.ComPortName].Id}",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };

                    taskKillProcess.Start();
                    taskKillProcess.WaitForExit();
                    record.IsAvailable = true;
                    record.Logs += "\r\n中断操作成功";
                    record.State = FlashRecordState.Stop;
                });
            }
            catch (Exception e)
            {
                record.Logs += "\r\n烧录中断失败";
                LoggerService.Error(e.Message, e);
                MessageBox.Show(e.Message);
            }
        }

        public void EraseFlashToPortFunc(FlasherRecord record)
        {
            List<string> commands = new List<string>
            {
                $"--port {record.ComPortName}",
                $"erase_flash"
            };
            var command = string.Join(" ", commands);
            RunCommandAsync(command, "erase", record);
        }

        public void StartCombineBinFunc()
        {
            List<string> parameters = new List<string>
            {
                $"--chip {chipType}",
                $"merge_bin",
            };
            parameters.AddRange(FirewareList.Select(x => $"{x.Offset} \"{x.FirewareFile}\""));
            parameters.Add($"--output {Environments.CombineBinFilePath}");
            string command = string.Join(" ", parameters);
            RunCommandAsync(command, "combine", records[0]);
            //File.WriteAllText(Path.Combine(Environments.AppPath, "a.txt"), command);
            //await Task.Run(() =>
            //{
            //    try
            //    {
            //        ProcessStartInfo startInfo = new ProcessStartInfo(Environments.EsptoolPath);
            //        startInfo.Arguments = command;
            //        startInfo.UseShellExecute = false;
            //        startInfo.RedirectStandardOutput = true;
            //        startInfo.RedirectStandardError = true;
            //        startInfo.CreateNoWindow = true;

            //        var combineProcess = new Process();
            //        combineProcess.EnableRaisingEvents = true;
            //        combineProcess.StartInfo = startInfo;

            //        combineProcess.OutputDataReceived += (s, e) =>
            //        {
            //            LoggerService.Info(e.Data);
            //            MessageBox.Show(e.Data);
            //        };

            //        combineProcess.ErrorDataReceived += (s, e) =>
            //        {
            //            LoggerService.Error(e.Data);
            //            MessageBox.Show(e.Data);
            //        };

            //        combineProcess.Exited += (s, e) =>
            //        {
            //            LoggerService.Info($"Combine Exited");
            //        };

            //        combineProcess.Start();
            //        //combineProcess.WaitForExit();

            //    }
            //    catch (Exception e)
            //    {
            //        LoggerService.Error(e.Message, e);
            //        MessageBox.Show(e.Message);
            //    }


            //});
        }

        public async void RunCommandAsync(string command, string type, FlasherRecord record)
        {

            record.Logs = string.Empty;
            record.Progress = 0;
            record.Logs += $">: {Environments.EsptoolPath} {command}\r\n";
            await Task.Run(() =>
            {
                try
                {
                    Timer timer = null;
                    string commandType = type.ToLower();
                    if (!record.IsAvailable && commandType == "write")
                        return;

                    if (commandType == "write")
                    {
                        record.State = FlashRecordState.Flashing;

                        timer = new Timer((object state) =>
                        {
                            record.SpendTime = Math.Round(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1.0 / 1000 - record.StartTime, 2);
                        }, null, 0, 100);
                    }

                    if (commandType == "erase")
                        record.State = FlashRecordState.Erasing;
                    record.IsAvailable = false;

                    ProcessStartInfo startInfo = new ProcessStartInfo(Environments.EsptoolPath);
                    startInfo.Arguments = command;
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.CreateNoWindow = true;

                    StringBuilder errorString = new StringBuilder();

                    Process process = new Process();
                    process.EnableRaisingEvents = true;
                    process.StartInfo = startInfo;

                    process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            ParseOutputString();
                            record.Logs = record.Logs + e.Data + "\r\n";
                            LoggerService.Info(e.Data);
                        }

                        void ParseOutputString()
                        {
                            Dictionary<string, System.Action<string[]>> patternActions = new()
                            {
                                { "Writing at.*?(\\d+) %",  new Action<string[]>( values => { record.Progress = Convert.ToInt32(values[1]); }) },
                                { "Hard resetting via RTS pin.*?", new Action<string[]>( 
                                    values => {
                                        if(commandType == "write") 
                                        { 
                                            record.State = FlashRecordState.Success; 
                                            if (isAutoFlashing)
                                            {
                                                AutoState = AutoFlashState.Finished;
                                            }
                                        } 
                                    }) },
                                { "Chip erase completed successfully in .*?s", new Action<string[]> ( values => record.State = FlashRecordState.Erase_Success) },
                                { "A fatal error occurred.*?", new Action<string[]> (values => record.State = FlashRecordState.Error) }
                            };

                            //Match match = null;
                            foreach (var item in patternActions)
                            {
                                Match match = Regex.Match(e.Data, item.Key);
                                if (match.Success)
                                {
                                    var matchValues = match.Groups.Cast<Group>().Select(x => x.Value).ToArray();
                                    item.Value.Invoke(matchValues);
                                }
                            }

                        }
                    });

                    process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
                    {
                        errorString.AppendLine(e.Data);
                    });

                    process.Exited += new EventHandler((sender, e) =>
                    {
                        FlashProcessTable.Remove(record.ComPortName);
                        if (timer != null)
                            timer.Dispose();

                        process.CancelOutputRead();
                        process.CancelErrorRead();

                        var errroMessage = errorString.ToString();
                        if (errroMessage != "\r\n" && errroMessage != "")
                        {
                            MessageBox.Show(errroMessage + "串口烧录异常");
                        }
                        switch (commandType)
                        {
                            case "write":
                                {
                                    if (record.State != FlashRecordState.Flashing)
                                    {
                                        record.TotalNumber += 1;
                                        timer.Dispose();
                                    }
                                    else
                                    {
                                        record.Progress = 0;
                                        record.StopTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000 * 1.0;
                                        
                                    }
                                    if (record.State == FlashRecordState.Success)
                                    {
                                        record.SuccessfulNumber += 1;
                                        record.AverageTime = 
                                            record.AverageTime == 0 ? record.SpendTime : Math.Round((record.SpendTime + record.AverageTime) / 2, 2);
                                    }
                                        
                                    break;
                                }
                            case "erase":
                                {

                                    break;
                                }
                            default: break;
                        }
                        record.IsAvailable = true;

                    });

                    if (FlashProcessTable.ContainsKey(record.ComPortName))
                        FlashProcessTable.Remove(record.ComPortName);
                    FlashProcessTable.Add(record.ComPortName, process);


                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    record.IsAvailable = true;
                    LoggerService.Error(ex.Message);
                    MessageBox.Show(ex.Message, "烧录错误,检查线缆连接", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly | MessageBoxOptions.ServiceNotification);

                }
            });
        }

        #endregion

        #region lifetime
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            ScanComs();

            ChangeThemeColor(ThemeColor);



            Timer = new Timer((object state) =>
            {
                ScanComs();
            }, null, 0, 200);
        }
        #endregion
   
    
        public async void AutoFlashThreadAction()
        {
            var plcPort = Records.Where(x => x.PortType == 0).FirstOrDefault();
            if (plcPort == null) {
                MessageBox.Show("请选择PLC所属COM");
                return;
            }
            try
            {
                PlcMaster = ModbusFactory.CreateRtuMaster(new SerialPort(
                    plcPort.ComPortName, 19200, Parity.Even, stopBits:StopBits.One,dataBits:8)) ;
            }
            catch(Exception ex)
            {
                MessageBox.Show("PLC连接失败, 请检查PLC通讯线" + ex.Message);
            }
            await Task.Run(() =>
            {
                AutoFlashThreadSequence();
            });
        }

        public void AutoFlashThreadSequence()
        {
            while(IsAutoFlashing)
            {
                WaitToFlash();
                ReadyToFlash();
                WaitFlashFinished();
                FinishFlash();
            }
        }

        public void WaitToFlash()
        {
            while (true && IsAutoFlashing)
            {
               bool toStart = PlcMaster.ReadCoils(1, Convert.ToUInt16(StartSignal), 1)[0];
               if (toStart) { 
                    AutoState = AutoFlashState.Flashing; 
                    break;
               }
            }
            //SerialPort serialPort = new();
            //serialPort.BaudRate = 19200;
            //try
            //{
            //    serialPort.PortName = records.Where(x => x.PortType == 0).First().ComPortName;
            //    if (serialPort.PortName == null)
            //        throw new Exception();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("串口未配置");
            //    AutoState = AutoFlashState.Idle;
            //    IsAutoFlashing = false;
            //    return;
            //}

            //serialPort.Parity = Parity.Even;
            //serialPort.StopBits = StopBits.One;
            //serialPort.DataBits = 8;
            //serialPort.Handshake = Handshake.None;

            //serialPort.DataReceived += SerialPort_DataReceived;
            //serialPort.ErrorReceived += SerialPort_ErrorReceived;

            //byte[] buffer = new byte[] { 0x01, 0x01, 0x50, 0x00, 0x00, 0x01, 0xEC, 0xCA };
            //serialPort.Open();
            //await Task.Run(() =>
            //{
            //    while (autoState == AutoFlashState.WaitingStartSignal)
            //    {
            //        serialPort.Write(buffer, 0, buffer.Length);
            //        Thread.Sleep(100);
            //    }
            //});
            //serialPort.Close();
            //if (autoState == AutoFlashState.ReadyToFlash)
            //    ReadyToFlash();
        }

        public void ReadyToFlash()
        {
            var leftRecord = Records.Where(x => x.PortType == 1).FirstOrDefault();
            var rightRecord = Records.Where(x => x.PortType == 2).FirstOrDefault();
            if (leftRecord == null || rightRecord == null)
            {
                MessageBox.Show("烧录串口未配置好");
                AutoState = AutoFlashState.Idle;
                IsAutoFlashing = false;
                return;
            }
            StartFlashToRecord(leftRecord);
            StartFlashToRecord(rightRecord);
        }

        public void WaitFlashFinished()
        {
            var leftRecord = Records.Where(x => x.PortType == 1).FirstOrDefault();
            var rightRecord = Records.Where(x => x.PortType == 2).FirstOrDefault();
            while (IsAutoFlashing)
            {
                if ((leftRecord.State == FlashRecordState.Success || leftRecord.State == FlashRecordState.Error ) &&
                    (rightRecord.State == FlashRecordState.Success || rightRecord.State == FlashRecordState.Error))
                {
                    break;
                }
            }

            bool[] result = { leftRecord.State == FlashRecordState.Success, rightRecord.State == FlashRecordState.Success };
            PlcMaster.WriteMultipleCoils(1, Convert.ToUInt16(FlashStateSignalStartIndex), result);
            Thread.Sleep(5);
            PlcMaster.WriteSingleCoil(1, Convert.ToUInt16(FinishSignal), true);
        }

        public void FinishFlash()
        {
            AutoState = AutoFlashState.WaitingStartSignal;
        }






    }
}
