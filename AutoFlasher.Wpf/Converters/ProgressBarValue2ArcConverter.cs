using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace AutoFlasher.Wpf.Converters
{
    public class Style1ArcConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double PIch2 = 2 * Math.PI;

            double centerX = (double)values[0] / 2;
            double centerY = (double)values[1] / 2;
            double angle = (double)values[2] / 100 * PIch2;

            double radius = 40;

            var 起始弧度的X偏移 = Math.Sin(0);
            var 起始弧度的Y偏移 = Math.Cos(0);

            var 终止弧度的X偏移 = Math.Sin(angle);
            var 终止弧度的Y偏移 = Math.Cos(angle);

            Point 起点 = new Point((int)(centerX + radius * 起始弧度的X偏移), (int)(centerY - radius * 起始弧度的Y偏移));
            Point 终点 = new Point((int)(centerX + radius * 终止弧度的X偏移), (int)(centerY - radius * 终止弧度的Y偏移));

            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            string data;

            if (angle == 0)
            {
                data = string.Empty;
            }
            else if (angle > 0 && angle < Math.PI)
            {
                data = $"M{起点.X},{起点.Y} A{radius},{radius},0,0,1 {终点.X},{终点.Y}";
            }
            else if (angle >= Math.PI && angle < 2 * Math.PI)
            {
                data = $"M{起点.X},{起点.Y} A{radius},{radius},0,1,1 {终点.X},{终点.Y}";
            }
            else
            {
                data = $"M{起点.X},{起点.Y} A{radius},{radius},0,1,1 {终点.X - 0.001},{终点.Y} Z";
            }

            return (Geometry)converter.ConvertFrom(data);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
