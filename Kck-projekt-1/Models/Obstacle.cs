using Kck_projekt_1.Utils;
using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class Obstacle : GameObject
    {
        public Obstacle(Vector2Int coords, int HP = 4) : base(coords, HP)
        {
            Hitbox = new Hitbox() { UpperLeftCorner = new Vector2Int(-1, 0), RightDownCorner = new Vector2Int(0, 0) };
        }
        public override void Reincarnate(Vector2Int coords, int Health = 4)
        {
            base.Reincarnate(coords, 4);
        }
        public override void NextFrame()
        {
        }

        public override void Hit(int damage = 1)
        {
            base.Hit(damage);
            Skin++;
            UpdateInfo();
        }

        public override void UpdateInfo()
        {
            GameObjectInfo info = new GameObjectInfo(this) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.Obstacle };
            if (Id >= ViewModel.Instance.GameObjectInfos.Count)
                ViewModel.Instance.GameObjectInfos.Add(info);
            else
                ViewModel.Instance.GameObjectInfos[Id] = info;
        }
    }
}
