using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekty_1_2.Models
{
    struct Coords
    {
        public int x;
        public int y;

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Coords operator +(Coords a) => a;
        public static Coords operator -(Coords a) => new Coords(-a.x, -a.y);

        public static Coords operator +(Coords a, Coords b)
            => new Coords(a.x + b.x, a.y + b.y);
        public static Coords operator +(Coords a, int b)
             => new Coords(a.x + b, a.y + b);

        public static Coords operator -(Coords a, int b)
             => new Coords(a.x - b, a.y - b);


        public bool AreCorrect()
            => x > 0 && x < GameConfig.Width && y >= 0 && y < GameConfig.Height;
    }
}
