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

        public Player(Vector2Int coords, int Health = 1) : base(coords, Health)
        {
            Hitbox = new Hitbox() { UpperLeftCorner = new Vector2Int(-1, -1), RightDownCorner = new Vector2Int(2, 1) };
        }

        public override void NextFrame()
        {
            base.NextFrame();
            if (movingLeft)
                MoveTo(Position + new Vector2Int(-1, 0));
            if (movingRight)
                MoveTo(Position + new Vector2Int(1, 0));
            movingLeft = movingRight = false;
        }
    }
}
