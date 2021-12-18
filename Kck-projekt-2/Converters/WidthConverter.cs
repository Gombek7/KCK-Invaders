using Kck_projekt_1.Models;
using Kck_projekt_1.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Kck_projekt_2.Converters
{
    class WidthConverter : IValueConverter
    {
        private static readonly int multipler = 10;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int width = (int)value;
            return width * multipler;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value / multipler;
            //return new Vector2Int((int)value / multipler, 0);
        }
    }
}
