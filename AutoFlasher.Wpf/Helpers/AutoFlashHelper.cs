using System.Collections.Generic;
using System.IO.Ports;

namespace AutoFlasher.Wpf.Helpers
{
    public enum AutoFlashState
    {
        Idle,
        WaitingStartSignal,
        WaitingFlashFinished,
    }

    public class AutoFlashHelper
    {
        public static Dictionary<AutoFlashState, string> stateWithString = new Dictionary<AutoFlashState, string>
        {
            { AutoFlashState.Idle, "空闲" },
            { AutoFlashState.WaitingStartSignal, "等待启动信号" },
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
