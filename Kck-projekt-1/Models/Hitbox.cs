using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class Hitbox
    {
        public Vector2Int UpperLeftCorner { get; set; } = new Vector2Int(-1, -1);
        public Vector2Int RightDownCorner { get; set; } = new Vector2Int(1, 1);
        public bool IsHit(Vector2Int point, Vector2Int hitboxPosition)
        {
            return UpperLeftCorner.x + hitboxPosition.x <= point.x && point.x <= RightDownCorner.x + hitboxPosition.x
                && UpperLeftCorner.y + hitboxPosition.y <= point.y && point.y <= RightDownCorner.y + hitboxPosition.y;
        }
    }
}
