using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2Int(Vector2Int vector2Int)
        {
            this.x = vector2Int.x;
            this.y = vector2Int.y;
        }
        public static Vector2Int operator +(Vector2Int a) => a;
        public static Vector2Int operator -(Vector2Int a) => new Vector2Int(-a.x, -a.y);

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
            => new Vector2Int(a.x + b.x, a.y + b.y);

        public static Vector2Int operator +(Vector2Int a, int b)
             => new Vector2Int(a.x + b, a.y + b);

        public static Vector2Int operator -(Vector2Int a, int b)
             => new Vector2Int(a.x - b, a.y - b);

        public static Vector2Int operator *(Vector2Int a, int b)
             => new Vector2Int(a.x * b, a.y * b);
        public static Vector2Int operator *(int b, Vector2Int a)
             => new Vector2Int(a.x * b, a.y * b);

        public static Vector2Int operator /(Vector2Int a, int b)
             => new Vector2Int(a.x / b, a.y / b);

        public override bool Equals(object obj) => obj is Vector2Int other && this.Equals(other);

        public bool Equals(Vector2Int v)
            => x == v.x && y == v.y;

        public override int GetHashCode()
            => (x, y).GetHashCode();

        public static bool operator ==(Vector2Int l, Vector2Int r)
            => l.Equals(r);

        public static bool operator !=(Vector2Int l, Vector2Int r)
            => !(l == r);

        public bool IsCorrectCoords()
            => x >= 0 && x < GameConfig.Width && y >= 0 && y < GameConfig.Height;

        public override string ToString()
        {
            return $"({x},{y})";
        }
    }
}
