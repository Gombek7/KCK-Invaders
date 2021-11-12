using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class Projectile : GameObject
    {
        protected Vector2Int direction;
        protected int moveFrameDelay = 2;
        protected int currentMoveFrameDelay = 2;
        public Projectile(Vector2Int coords) : base(coords, 1)
        {

        }
        public override void NextFrame()
        {
            base.NextFrame();
            if (--currentMoveFrameDelay < 0)
            {
                currentMoveFrameDelay = moveFrameDelay;
                Vector2Int newPosition = Position + direction;
                if ((newPosition + Hitbox.UpperLeftCorner).IsCorrectCoords() && (newPosition + Hitbox.RightDownCorner).IsCorrectCoords())
                {
                    MoveTo(newPosition);
                }
                else
                    Hit(1);
            }
        }
    }
}
