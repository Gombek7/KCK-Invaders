using Kck_projekt_1.Utils;
using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class EnemyTierI : Enemy
    {
        public EnemyTierI(Vector2Int coords, int Health = 1) : base(coords, Health)
        {
            Hitbox = new Hitbox()
            {
                UpperLeftCorner = new Vector2Int(-1, -1),
                RightDownCorner = new Vector2Int(2, 1)
            };
        }

        public override void UpdateInfo()
        {
            GameObjectInfo info = new GameObjectInfo(this) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.EnemyTierI };
            if (Id >= ViewModel.Instance.GameObjectInfos.Count)
                ViewModel.Instance.GameObjectInfos.Add(info);
            else
                ViewModel.Instance.GameObjectInfos[Id] = info;
        }
    }
}
