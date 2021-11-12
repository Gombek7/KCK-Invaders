using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    abstract class Enemy : GameObject
    {
        protected Vector2Int moveVector = new Vector2Int(1,0);
        protected int moveFrameDelay = 5;
        protected int currentMoveFrameDelay = 5;
        protected Enemy(Vector2Int coords, int Health = 1) : base(coords, Health)
        {
        }
        public override void NextFrame()
        {
            base.NextFrame();
            if(--currentMoveFrameDelay <= 0)
            {
                currentMoveFrameDelay = moveFrameDelay;
                Vector2Int newPosition = Position + moveVector;
                if ((newPosition + Hitbox.UpperLeftCorner).IsCorrectCoords() && (newPosition + Hitbox.RightDownCorner).IsCorrectCoords())
                {
                    MoveTo(newPosition);
                }
                else
                    moveVector = -moveVector;
            }
        }
    }
}
