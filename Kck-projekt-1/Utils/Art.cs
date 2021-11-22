using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Utils
{
    class Art
    {
        private string[] frames;
        private int currentFrameId = 0;
        private int currentFrameDelay = 0;
        
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
        public int NextFrameDelay { get; set; } = 0;

        public Art(string path)
        {
            //content = new string[1];
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
                    output += lines[i++];
                    output += $"\u001b[u\u001b[{offset++}B";
                }
                loadedFrames.Add(output);
            }

            frames = loadedFrames.ToArray();
        }

        public void Draw(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Draw();
        }
        public void Draw()
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.Write(frames[currentFrameId]);
            Console.ForegroundColor = currentColor;

            if (currentFrameDelay++ == NextFrameDelay)
            {
                currentFrameDelay = 0;
                currentFrameId = (currentFrameId + 1) % frames.Length;
            }
        }
        public void DrawFrame(int f)
        {
            if (f < 0) f = 0;
            if (f >= frames.Length) f = frames.Length - 1;
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.Write(frames[f]);
            Console.ForegroundColor = currentColor;
        }
    }
}
