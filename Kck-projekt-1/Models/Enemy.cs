using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    abstract class Enemy : GameObject
    {
        protected Random random = new Random();
        protected Vector2Int moveVector = new Vector2Int(1,0);
        protected int moveFrameDelay = 5;
        protected int currentMoveFrameDelay = 5;
        private float shotChance = 0.05f;
        public float ShotChance
        {
            get => shotChance;
            private set
            {
                shotChance = value;
            }
        }
        private EnemyProjectile projectile;
        public EnemyProjectile Projectile
        {
            get => projectile;
            private set
            {
                projectile = value;
            }
        }
        protected Enemy(Vector2Int coords, int Health = 1) : base(coords, Health)
        {
            Projectile = new EnemyProjectile(Position);
        }
        public override void NextFrame()
        {
            if (!IsDestroyed)
            {
                if (--currentMoveFrameDelay < 0)
                {
                    currentMoveFrameDelay = moveFrameDelay;
                    Vector2Int newPosition = Position + moveVector;
                    if ((newPosition + Hitbox.UpperLeftCorner).IsCorrectCoords() && (newPosition + Hitbox.RightDownCorner).IsCorrectCoords())
                    {
                        MoveTo(newPosition);
                    }
                    else
                        moveVector = -moveVector;
                    UpdateInfo();
                }

                if ((float)random.NextDouble() <= ShotChance && Projectile.IsDestroyed)
                    Projectile.Reincarnate(Position);
            }
            Projectile.NextFrame();
        }
        public void Shoot()
        {
            if (Projectile.IsDestroyed)
                Projectile.Reincarnate(Position);
        }
        public override void Hit(int damage = 1)
        {
            base.Hit(damage);
            if (IsDestroyed)
                ViewModel.Instance.Score++;
        }
    }
}
