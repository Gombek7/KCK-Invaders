using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    static class GameConfig
    {
        public static int Fps { get; } = 30;
        //default 100
        public static int Width { get; } = 100;
        //default 40
        public static int Height { get; } = 36;
        public static bool EasyMode { get; set; } = false;
    }
}
