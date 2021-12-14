using Kck_projekt_1.Models;
using Kck_projekt_1.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Kck_projekt_2.Converters
{
    class YPositionConverter : IValueConverter
    {
        private static readonly int multipler = 10;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GameObjectInfo gameObjectInfo = (GameObjectInfo)value;
            return (gameObjectInfo.Position + gameObjectInfo.Hitbox.UpperLeftCorner).y * multipler;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //return new Vector2Int(0, (int)value)/ multipler;
        }
    }
}
