using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFlasher.Wpf.Interfaces
{
    public interface ILoggerService
    {
        void Message(string message);

        void Info(string message);

        void Warning(string message);

        void Error(string message);

        void Error(string message, Exception exception);
    }
}
