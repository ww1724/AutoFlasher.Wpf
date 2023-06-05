using System.Collections.Generic;
using System.IO.Ports;

namespace AutoFlasher.Wpf.Helpers
{
    public enum AutoFlashState
    {
        Idle,
        WaitingStartSignal,
        ReadyToFlash,
        Flashing,
        Finished,
        Stop
    }

    public enum PortType
    {
        PLC, Left, Right
    }

    public class AutoFlashHelper
    {
        public static Dictionary<AutoFlashState, string> stateWithString = new Dictionary<AutoFlashState, string>
        {
            { AutoFlashState.Idle, "空闲" },
            { AutoFlashState.WaitingStartSignal, "等待启动信号" },
        };

        public static Dictionary<string, int> RecordTypes = new()
        {
            { "PLC", 0 } , { "左烧录", 1}, { "右烧录", 2 }
        };

        public AutoFlashHelper()
        {

        }

        public void setPlcComPort(string port)
        {
            SerialPort serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            //modbusMaster = ModbusSerialMaster
        }

        
    }


}
