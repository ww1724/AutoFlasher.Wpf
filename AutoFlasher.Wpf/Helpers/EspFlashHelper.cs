using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFlasher.Wpf.Helpers
{
    public class EspFlashHelper
    {
        public static string[] BaudList { get; set; } = new string[] { "9600", "19200", "51200", "115200", "230400", "460800", "57600", "921600", "1152000" };
        
        public static string[] ChipTypeList { get; set; } = { "ESP32", "ESP32C3", "ESP8266", "ESP8285", "ESPD2WD", "ESP32S2", "ESP32S3", "ESP32C2", "ESP32C6" };
        public static string[] CrystalFrequencyList { get; set; } = { "40M" };
        public static string[] SpiSpeedList { get; set; } = { "40MHz", "26.7MHz", "20MHz", "80MHz" };
        public static string[] SpiModeList { get; set; } = { "QIO", "QOUT", "DIO", "DOUT", "FASTRD" };
        public static string[] FlashSizeList { get; set; } = { "8MB", "16MB", "32MB", "64MB", "128MB" };

        public static string DefaultBaud = "460800";
        public static string DefaultChipType = "ESP32";
        public static string DefaultCrystalFrequency = "40M";
        public static string DefaultSpiSpeed = "40MHz";
        public static string DefaultSpiMode = "DIO";
        public static string DefaultFlashSize = "16MB";

        public static bool DefaultIsShowFirewareFullName = true;
        public static bool DefaultIsAutoParseFirewareOffest = true;
        public static string DefaultFirewareParseOffsetString = "0x[0-9a-fA-F]+";

        public static string DefaultStartSignal = "M10";
        public static string DefaultStopSignal = "M11";
        public static string DefaultFinishSignal = "M20";
        public static string DefaultFlashStateSignalStartIndex = "M30";

    }
}
