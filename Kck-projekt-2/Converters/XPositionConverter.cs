using Kck_projekt_1.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Kck_projekt_2.Converters
{
    class XPositionConverter : IValueConverter
    {
        private static readonly int multipler = 10;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Vector2Int)value).x * multipler;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Vector2Int((int)value / multipler, 0);
        }
    }
}
