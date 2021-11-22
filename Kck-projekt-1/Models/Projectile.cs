using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    abstract class Projectile : GameObject
    {
        protected Vector2Int direction;
        protected int moveFrameDelay = 2;
        protected int currentMoveFrameDelay = 2;
        protected int skinsCount = 1;
        public Projectile(Vector2Int coords) : base(coords, 1)
        {
            targets = new List<GameObject>();
            CurrentHealth = 0;
        }
        public override void NextFrame()
        {
            if (IsDestroyed)
                return;
            if (--currentMoveFrameDelay < 0)
            {
                currentMoveFrameDelay = moveFrameDelay;
                Vector2Int newPosition = Position + direction;
                if ((newPosition + Hitbox.UpperLeftCorner).IsCorrectCoords() && (newPosition + Hitbox.RightDownCorner).IsCorrectCoords())
                {
                    MoveTo(newPosition);
                    Skin = (Skin + 1) % skinsCount;
                    UpdateInfo();
                    foreach (GameObject target in targets)
                        if (!target.IsDestroyed && Collides(target))
                        {
                            Hit(1);
                            target.Hit(1);
                            return;
                        }
                }
                else
                    Hit(1);
            }
        }

        private List<GameObject> targets;

        public void AddTarget(GameObject gameObject)
        {
            targets.Add(gameObject);
        }

        public void RemoveTarget(GameObject gameObject)
        {
            targets.Remove(gameObject);
        }
    }
}
