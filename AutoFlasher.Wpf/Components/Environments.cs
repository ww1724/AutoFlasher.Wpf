using AutoFlasher.Wpf.Interfaces;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFlasher.Wpf.Components
{
    public static class Environments
    {
        private static string AppName = "AutoFlasher";

        private static string ConfigPath = "data";

        private static string ConfigFileName = "config.json";

        private static string LogFolderName = "log";

        

        public static string AppPath
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
                if (!Directory.Exists(path)) 
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public static string ConfigFilePath
        {
            get
            {
                string path = Path.Combine(AppPath, ConfigPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string filePath = Path.Combine(path, ConfigFileName);

                return filePath;
            }
        }


        public static string LogFilePath
        {
            get
            {
                string path = Path.Combine(AppPath, LogFolderName);
                if (!Directory.Exists(path))
                   Directory.CreateDirectory(path);
                string filepath = Path.Combine(path, $"{DateTime.Today.ToShortDateString().Replace("/", "-")}.log");
                return filepath;
            }
        }

        public static string CombineBinFilePath
        {
            get
            {
                return Path.Combine(AppPath, "data", "combined 0x0.bin");
            }
        }

        public static string CombineBinPath
        {
            get
            {
                return Path.Combine(AppPath, "data");
            }
        }

        public static string EsptoolPath
        {
            get
            {
                string path = Path.Combine(AppPath, "bin", "esptool.exe");
                if (!File.Exists(path))
                    throw new ArgumentNullException("未找到Esptool，检查安装情况");
                return path;
            }
        }
    }
}
