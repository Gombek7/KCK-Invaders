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
        protected int moveFrameDelay = 10;
        protected int currentMoveFrameDelay = 10;
        public static bool borderCollision = false;
        private float shotChance = 0.02f;
        protected int scoreValue = 5;
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
                    if (borderCollision)
                    {
                        MoveTo(Position + new Vector2Int(0, 1));
                        moveVector = -moveVector;
                    }
                    else
                    {
                        currentMoveFrameDelay += moveFrameDelay;
                        Vector2Int newPosition = Position + moveVector;
                        MoveTo(newPosition);
                        if ((float)random.NextDouble() <= ShotChance && Projectile.IsDestroyed)
                            Projectile.Reincarnate(Position);
                    }
                    Skin = (Skin + 1) % 2;
                    UpdateInfo();
                }
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
                ViewModel.Instance.Score += scoreValue;
        }
        public bool CheckBorderCollision()
        {
            Vector2Int newPosition = Position + moveVector;
            bool thisEnemyCollision = !(newPosition + Hitbox.UpperLeftCorner).IsCorrectCoords() || !(newPosition + Hitbox.RightDownCorner).IsCorrectCoords();
            if (thisEnemyCollision)
                borderCollision = true;
            return thisEnemyCollision;
        }
    }
}
