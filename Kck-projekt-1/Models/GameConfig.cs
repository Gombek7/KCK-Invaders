using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    static class GameConfig
    {
        public static int Fps { get; } = 60;
        public static int Width { get; } = 100;
        public static int Height { get; } = 40;
        public static bool EasyMode { get; set; } = false;
    }
}
