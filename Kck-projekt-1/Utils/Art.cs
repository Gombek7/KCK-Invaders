using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Utils
{
    class Art
    {
        private string[] skins;
        
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
        public int NextFrameDelay { get; set; } = 0;
        public int Skins { get => skins.Length; }
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;

        public Art(string path)
        {
            List<string> loadedFrames = new List<string>();
            
            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(path);
            }
            catch
            {
                lines = new string[1];
                lines[0] = "";
            }
            for (int i = 0; i < lines.Length; i++)
            {
                string output = "\u001b[s";
                int offset = 1;
                while (!String.IsNullOrEmpty(lines[i]))
                {
                    string line = lines[i++];
                    output += line;
                    output += $"\u001b[u\u001b[{offset++}B";
                    if (Width < line.Length) Width = line.Length;
                }

                if (Height < offset-1) Height = offset-1;
                loadedFrames.Add(output);
            }

            skins = loadedFrames.ToArray();
        }

        public void Draw(int x, int y, int skin = 0)
        {
            Console.SetCursorPosition(x, y);
            Draw(skin);
        }
        public void Draw(int skin = 0)
        {
            if (skin < 0) skin = 0;
            skin %= skins.Length;

            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.Write(skins[skin]);
            Console.ForegroundColor = currentColor;
        }
    }
}
