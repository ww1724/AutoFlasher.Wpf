using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace AutoFlasher.Wpf.Converters
{
    public class FlashRecordState2ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value.ToString();
            Color color;
            switch (state)
            {
                case "Idle": color = (Color)ColorConverter.ConvertFromString("#ff666666"); break;
                case "Flashing": color = (Color)ColorConverter.ConvertFromString("#ffFCC055"); break;
                case "Erasing": color = (Color)ColorConverter.ConvertFromString("#ffFCC055"); break;
                case "Vertify": color = (Color)ColorConverter.ConvertFromString("#ffFCC055"); break;
                case "Success": color = (Color)ColorConverter.ConvertFromString("#FF00DD00"); break;
                case "Erase_Success": color = (Color)ColorConverter.ConvertFromString("#FF00DD00"); break;
                case "Error": color = (Color)ColorConverter.ConvertFromString("#ffEB3D4F"); break;
                case "Stop": color = (Color)ColorConverter.ConvertFromString("#ff33619A"); break;
                
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
