using AutoFlasher.Wpf.Helpers;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFlasher.Wpf.Models
{
    public enum FlashRecordState
    {
        Idle = 0,
        Flashing = 1,
        Stop = 2,
        Vertify = 3,
        Success = 4,
        Error = 5,
        Erasing = 6,
        Erase_Success = 7,
    }



    public class FlasherRecord : PropertyChangedBase
    {
        public static Dictionary<string, string> StateWithString = new()
        {
            { "Idle", "等待" }, {"Flashing", "烧录中" }, { "Success", "烧录成功" }, { "Error", "错误" },
            { "Stop", "停止" }, {"Vertify", "校验中"}, {"Erasing", "擦除中"}, { "Erase_Success", "擦除成功"},
        };

        public FlasherRecord() { }

        private bool isAvailable = true;

        public bool IsAvailable { get => isAvailable; set { isAvailable = value; NotifyOfPropertyChange(); } }

        private FlashRecordState state;

        public FlashRecordState State
        {
            get { return state; }
            set { state = value; StateText = StateWithString[State.ToString()]; NotifyOfPropertyChange(); }
        }

        private string stateText;

        public string StateText
        {
            get { return stateText; }
            set { stateText = value; NotifyOfPropertyChange(); }
        }

        public int PortType { get => portType; set { portType = value; NotifyOfPropertyChange(); } }

        private string comPortName;

        public string ComPortName
        {
            get { return comPortName; }
            set { comPortName = value; NotifyOfPropertyChange(); }
        }

        private string baudRate;

        public string BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; NotifyOfPropertyChange(); }
        }

        private string logs;

        public string Logs
        {
            get { return logs; }
            set { logs = value; NotifyOfPropertyChange(); }
        }

        private double progress = 0;

        public double Progress
        {
            get { return progress; }
            set { progress = value; NotifyOfPropertyChange(); }
        }

        private int successfulNumber;

        public int SuccessfulNumber
        {
            get { return successfulNumber; }
            set
            {
                successfulNumber = value;
                if (totalNumber == 0)
                    SuccessPercent = 100;
                else
                    SuccessPercent = Math.Round((SuccessfulNumber * 1.0 / totalNumber) * 100, 2);
                NotifyOfPropertyChange();
            }
        }

        private int totalNumber;

        public int TotalNumber
        {
            get { return totalNumber; }
            set
            {
                totalNumber = value;
                if (totalNumber == 0)
                    SuccessPercent = 100;
                else
                    SuccessPercent = Math.Round((SuccessfulNumber * 1.0 / totalNumber) * 100, 2);
                NotifyOfPropertyChange();
            }
        }


        private double successPercent = 100;

        public double SuccessPercent
        {
            get { return successPercent; }
            set { successPercent = value; NotifyOfPropertyChange(); }
        }

        private double startTime;

        public double StartTime
        {
            get { return startTime; }
            set { startTime = value; NotifyOfPropertyChange(); }
        }

        private double spendTime;

        public double SpendTime
        {
            get { return spendTime; }
            set { spendTime = value; NotifyOfPropertyChange(); }
        }

        private double stopTime;

        public double StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }

        private double averageTime = 0;
        private int portType = -1;

        public double AverageTime
        {
            get { return averageTime; }
            set { averageTime = value; NotifyOfPropertyChange(); }
        }

    }
}
