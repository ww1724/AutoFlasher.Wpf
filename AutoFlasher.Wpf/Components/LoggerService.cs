using AutoFlasher.Wpf.Components;
using AutoFlasher.Wpf.Interfaces;
using Serilog;
using System;

namespace AutoFlasher.Wpf.Components
{
    public class LoggerService
        : ILoggerService
    {

        private static Serilog.Core.Logger _logger;

        private static LoggerService _instance;

        public static LoggerService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoggerService();
                }
                return _instance;
            }
        }

        private void InitializeLogger()
        {
            if (_logger == null)
            {
                var logFilePath = Environments.LogFilePath;
                _logger = new LoggerConfiguration()
                    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
                    .CreateLogger();
            }
        }

        public void Error(string message, Exception exception)
        {
            InitializeLogger();
            _logger?.Error(message, exception);
        }

        public void Error(string message)
        {
            InitializeLogger();
            _logger?.Error(message);
        }

        public void Info(string message)
        {
            InitializeLogger();
            _logger?.Information(message);
        }

        public void Message(string message)
        {
            InitializeLogger();
            _logger?.Debug(message);
        }

        public void Warning(string message)
        {
            InitializeLogger();
            _logger?.Warning(message);
        }


    }
}
