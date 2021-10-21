using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekty_1_2.Models
{
    class Player : GameObject
    {
        public Player(Coords coords, int Health = 1) : base(coords, Health)
        {
        }
    }
}
