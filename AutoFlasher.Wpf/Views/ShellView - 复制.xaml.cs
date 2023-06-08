using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoFlasher.Wpf.Views
{
    /// <summary>
    /// ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellViewBak : Window
    {
        public ShellViewBak()
        {
            InitializeComponent();
            SizeChanged += (object sender, SizeChangedEventArgs e) =>
            {
                var h = e.NewSize.Height;
                FlashRecordList.Height = h - 225;
            };
        }

        private void minimizeWindowBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void maxWindowBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {

            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        }

        private void closeWindowBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }
    }
}
