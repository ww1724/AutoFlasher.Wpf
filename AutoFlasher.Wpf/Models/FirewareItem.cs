using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFlasher.Wpf.Models
{
    public class FirewareItem : PropertyChangedBase
    {

        private bool enable;

        public bool Enable
        {
            get { return enable; }
            set { enable = value; NotifyOfPropertyChange(); }
        }

        public string FileName { get => fileName; set { fileName = value; NotifyOfPropertyChange(); } }

        private string firewareFile;

        public string FirewareFile
        {
            get { return firewareFile; }
            set { firewareFile = value; NotifyOfPropertyChange(); }
        }

        private string offset;
        private string fileName;

        public string Offset
        {
            get { return offset; }
            set { offset = value; NotifyOfPropertyChange(); }
        }


    }
}
