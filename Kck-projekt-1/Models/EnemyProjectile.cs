using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Kck_projekt_1.Utils;

namespace Kck_projekt_1.Models
{
    class EnemyProjectile : Projectile
    {
        public EnemyProjectile(Vector2Int coords) : base(coords)
        {
            direction = new Vector2Int(0, 1);
            moveFrameDelay = 1;
            Hitbox = new Hitbox() {UpperLeftCorner = new Vector2Int(0,-1), RightDownCorner = new Vector2Int(0, 0) };
        }

        public override void UpdateInfo()
        {
            GameObjectInfo info = new GameObjectInfo(this) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.EnemyProjectile };
            if (Id >= ViewModel.Instance.GameObjectInfos.Count)
                ViewModel.Instance.GameObjectInfos.Add(info);
            else
                ViewModel.Instance.GameObjectInfos[Id] = info;
        }
    }
}
