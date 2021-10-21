using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Kck_projekty_1_2.ViewModels;
namespace Kck_projekty_1_2.Views
{
    static class ConsoleViewConfig
    {
        public static int Fps { get; } = 60;
    }
    class ConsoleView
    {
        private ConsoleViewModel consoleViewModel;
        public ConsoleView()
        {

        }
        public int Start()
        {
            consoleViewModel = new ConsoleViewModel();
            // Todo: Usunąć bezpośrednią subskrypcje eventu
            consoleViewModel.game.PlayerMoved += (int x, int y) =>
            {
                Console.Clear();
                Console.SetCursorPosition(x, y);
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.Black;
            };

            string key = "none";
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.LeftArrow:
                            consoleViewModel.MoveLeftCommand.Execute(null);
                            break;
                        case ConsoleKey.RightArrow:
                            consoleViewModel.MoveRightCommand.Execute(null);
                            break;
                        case ConsoleKey.Spacebar:
                            consoleViewModel.ShootCommand.Execute(null);
                            break;
                        case ConsoleKey.Escape:
                            return 0;
                            break;
                        default:
                            break;
                    }
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);
                }
                Thread.Sleep(1000 / ConsoleViewConfig.Fps);
            }
            return 0;
        }
    }
}
