using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class WeakEnemy : Enemy
    {
        public WeakEnemy(Vector2Int coords, int Health = 1) : base(coords, Health)
        {

        }
    }
}
