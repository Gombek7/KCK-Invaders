using Kck_projekt_1.Utils;
using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class Player : GameObject
    {
        private bool movingLeft = false;
        public bool MovingLeft
        {
            get => movingLeft;
            set
            {
                if(value == true)
                    movingRight = false;
                movingLeft = value;
            }
        }
        private bool movingRight = false;
        public bool MovingRight
        {
            get => movingRight;
            set
            {
                if (value == true)
                    movingLeft = false;
                movingRight = value;
            }
        }
        private PlayerProjectile projectile;
        public PlayerProjectile Projectile
        {
            get => projectile;
            private set
            {
                projectile = value;
            }
        }

        public Player(Vector2Int coords, int Health = 1) : base(coords, Health)
        {
            Hitbox = new Hitbox() { UpperLeftCorner = new Vector2Int(-1, -1), RightDownCorner = new Vector2Int(2, 1) };
            Projectile = new PlayerProjectile(Position);
        }

        public override void NextFrame()
        {
            if (movingLeft)
                MoveTo(Position + new Vector2Int(-1, 0));
            if (movingRight)
                MoveTo(Position + new Vector2Int(1, 0));
            if (movingLeft || movingRight)
                UpdateInfo();
            movingLeft = movingRight = false;

            Projectile.NextFrame();
        }
        public override void UpdateInfo()
        {
            GameObjectInfo info = new GameObjectInfo(this) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.Player };
            if (Id == ViewModel.Instance.GameObjectInfos.Count)
                ViewModel.Instance.GameObjectInfos.Add(info);
            else
                ViewModel.Instance.GameObjectInfos[Id] = info;
        }

        public void Shoot()
        {
            if (Projectile.IsDestroyed)
                Projectile.Reincarnate(Position);
        }
    }
}
