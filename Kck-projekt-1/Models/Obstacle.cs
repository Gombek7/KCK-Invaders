﻿using Kck_projekt_1.Utils;
using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class Obstacle : GameObject
    {
        public Obstacle(Vector2Int coords) : base(coords, 4)
        {
            Hitbox = new Hitbox() { UpperLeftCorner = new Vector2Int(0, 0), RightDownCorner = new Vector2Int(1, 0) };
        }

        public override void NextFrame()
        {
        }

        public override void Hit(int damage = 1)
        {
            base.Hit(damage);
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