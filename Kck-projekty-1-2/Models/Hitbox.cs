using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekty_1_2.Models
{
    class Hitbox
    {
        public Coords UpperLeftCorner { get; set; }
        public Coords RightDownCorner { get; set; }
        public bool IsHit(Coords point)
        {
            return UpperLeftCorner.x <= point.x && point.x <= RightDownCorner.x 
                && UpperLeftCorner.y <= point.y && point.y <= RightDownCorner.y;
        }
    }
}
