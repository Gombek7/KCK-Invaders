using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Utils
{
    static class ConsoleUtils
    {
        public static void DrawBorder(int x1, int y1, int x2, int y2, ConsoleColor background, ConsoleColor foreground)
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = background;
            ConsoleColor currentForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = foreground;
            DrawBorder(x1, y1, x2, y2);
            Console.BackgroundColor = currentBackgroundColor;
            Console.ForegroundColor = currentForegroundColor;
        }

        public static void DrawBorder(int x1, int y1, int x2, int y2)
        {
            if (/*x1 < 0 || x2 < 0 || y1 < 0 || y2 < 0 ||*/ x1 > x2 || y1 > y2)
                return;
            //draw up
            if (y1 >=0)
            {
                if(x1>=0)
                {
                    Console.SetCursorPosition(x1, y1);
                    Console.Write('╔');
                    for (int i = x2 - x1 - 2; i >= 0; i--)
                        Console.Write('═');
                }
                else
                {
                    Console.SetCursorPosition(0, y1);
                    for (int i = x2 - 1; i >= 0; i--)
                        Console.Write('═');
                }
                Console.Write('╗');
            }
            //draw bottom
            if (x1>=0)
            {
                Console.SetCursorPosition(x1, y2);
                Console.Write('╚');
                for (int i = x2 - x1 - 2; i >= 0; i--)
                    Console.Write('═');
            }
            else
            {
                Console.SetCursorPosition(0, y2);
                for (int i = x2 - 1; i >= 0; i--)
                    Console.Write('═');
            }
            Console.Write('╝');

            int siteHeight = y1 >= 0 ? y2 - y1 - 1 : y2;
            int startPosition = (y1 >= 0) ? y1 + 1 : 0;
            //draw left
            if (x1>=0)
            {
                for (int i = 0; i < siteHeight; i++)
                {
                    Console.SetCursorPosition(x1, startPosition+i);
                    Console.Write('║');
                }
            }
            //draw right
            Console.SetCursorPosition(x2, (y1 >= 0) ? y1 + 1 : 0);
            for (int i = 0; i < siteHeight; i++)
            {
                Console.SetCursorPosition(x2, startPosition + i);
                Console.Write('║');
            }
        }
        public static void ClearBorder(int x1, int y1, int x2, int y2)
        {
            if (x1 < 0 || x2 < 0 || y1 < 0 || y2 < 0 || x1 > x2 || y1 > y2)
                return;
            Console.SetCursorPosition(x1, y1);
            for (int i = x2 - x1; i >= 0; i--)
                Console.Write(' ');
            Console.SetCursorPosition(x1, y2);
            for (int i = x2 - x1; i >= 0; i--)
                Console.Write(' ');
            for (int r = y1 + 1; r < y2; r++)
            {
                Console.SetCursorPosition(x1, r);
                Console.Write(' ');
                Console.SetCursorPosition(x2, r);
                Console.Write(' ');
            }
        }
        public static void Fill(char ch, int x1, int y1, int x2, int y2)
        {
            if (x2 < 0 || y2 < 0 || x1 > x2 || y1 > y2)
                return;
            if (x1 < 0)
                x1 = 0;
            if (y1 < 0)
                y1 = 0;
            int width = x2 - x1 + 1;
            int height = y2 - y1 + 1;
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x1, y1+i);
                for (int j = 0; j < width; j++)
                    Console.Write(ch);
            }
        }

        public static string LoadArt(string path)
        {
            string output = "\u001b[s";
            int offset = 1;
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
            foreach(string line in lines)
            {
                output += line;
                output += $"\u001b[u\u001b[{offset++}B";
            }

            return output;
        }

        public static void PrintBigDigit(int d)
        {
            switch (d)
            {
                case 0:
                    Console.Write("\u001b[s┌─┐\u001b[u\u001b[1B│ │\u001b[u\u001b[2B└─┘");
                    break;
                case 1:
                    Console.Write("\u001b[s ┬\u001b[u\u001b[1B │\u001b[u\u001b[2B ┴");
                    break;
                case 2:
                    Console.Write("\u001b[s┌─┐\u001b[u\u001b[1B┌─┘\u001b[u\u001b[2B└─┘");
                    break;
                case 3:
                    Console.Write("\u001b[s┌─┐\u001b[u\u001b[1B ─┤\u001b[u\u001b[2B└─┘");
                    break;
                case 4:
                    Console.Write("\u001b[s┬ ┬\u001b[u\u001b[1B└─┤\u001b[u\u001b[2B  ┴");
                    break;
                case 5:
                    Console.Write("\u001b[s┌─┐\u001b[u\u001b[1B└─┐\u001b[u\u001b[2B└─┘");
                    break;
                case 6:
                    Console.Write("\u001b[s┌─┐\u001b[u\u001b[1B├─┐\u001b[u\u001b[2B└─┘");
                    break;
                case 7:
                    Console.Write("\u001b[s┌─┐\u001b[u\u001b[1B  ┼\u001b[u\u001b[2B  ┴");
                    break;
                case 8:
                    Console.Write("\u001b[s┌─┐\u001b[u\u001b[1B├─┤\u001b[u\u001b[2B└─┘");
                    break;
                case 9:
                    Console.Write("\u001b[s┌─┐\u001b[u\u001b[1B└─┤\u001b[u\u001b[2B└─┘");
                    break;
            }
        }
        public static void PrintBigNumber(int n, int digitsCount)
        {
            int[] digits = new int[digitsCount];
            for (int i = 0; i < digitsCount; i++)
            {
                digits[i] = n % 10;
                n /= 10;
            }
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            for (int i = 0; i < digitsCount; i++)
            {
                Console.SetCursorPosition(x + i * 3, y);
                PrintBigDigit(digits[digitsCount - i - 1]);
            }

        }
    }
}
