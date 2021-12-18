using Kck_projekt_1.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
namespace Kck_projekt_2.Converters
{
    class ObjectTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GameObjectInfo gameObjectInfo = (GameObjectInfo)value;
            if (gameObjectInfo == null)
                return "";

            switch (gameObjectInfo.GameObjectType)
            {
                case GameObjectInfo.GameObjectTypeEnum.Player:
                    return "Player";
                case GameObjectInfo.GameObjectTypeEnum.EnemyTierI:
                    return "EnemyTierI";
                case GameObjectInfo.GameObjectTypeEnum.EnemyTierII:
                    return "EnemyTierII";
                case GameObjectInfo.GameObjectTypeEnum.EnemyTierIII:
                    return "EnemyTierIII";
                case GameObjectInfo.GameObjectTypeEnum.EnemyTierIV:
                    return "EnemyTierIV";
                case GameObjectInfo.GameObjectTypeEnum.Obstacle:
                    return "Obstacle";
                case GameObjectInfo.GameObjectTypeEnum.PlayerProjectile:
                    return "PlayerProjectile";
                case GameObjectInfo.GameObjectTypeEnum.EnemyProjectile:
                    return "EnemyProjectile";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //return new Vector2Int((int)value / multipler, 0);
        }
    }
}
