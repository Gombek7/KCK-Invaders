using Kck_projekt_1.Utils;
using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class PlayerProjectile : Projectile
    {
        public PlayerProjectile(Vector2Int coords) : base(coords)
        {
            direction = new Vector2Int(0, -1);
            moveFrameDelay = 0;
            skinsCount = 3;
            Hitbox = new Hitbox() {UpperLeftCorner = new Vector2Int(0,-1), RightDownCorner = new Vector2Int(0, 1) };
        }

        public override void UpdateInfo()
        {
            GameObjectInfo info = new GameObjectInfo(this) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.PlayerProjectile };
            if (Id >= ViewModel.Instance.GameObjectInfos.Count)
                ViewModel.Instance.GameObjectInfos.Add(info);
            else
                ViewModel.Instance.GameObjectInfos[Id] = info;
        }
    }
}
