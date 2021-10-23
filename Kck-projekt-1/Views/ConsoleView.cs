using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Kck_projekt_1.ViewModels;
using Kck_projekt_1.Models;
using System.Reflection;
using Kck_projekt_1.Utils;

namespace Kck_projekt_1.Views
{
    static class ConsoleViewConfig
    {
        public static int Fps { get; } = 60;
    }
    class ConsoleView
    {
        private ViewModel viewModel;
        public ConsoleView()
        {

        }
        public int Start()
        {
            viewModel = new ViewModel();
            viewModel.PropertyChanged += RefreshData;
            // Todo: Usunąć bezpośrednią subskrypcje eventu

            string key = "none";
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.LeftArrow:
                            viewModel.MoveLeftCommand.Execute(null);
                            break;
                        case ConsoleKey.RightArrow:
                            viewModel.MoveRightCommand.Execute(null);
                            break;
                        case ConsoleKey.Spacebar:
                            viewModel.ShootCommand.Execute(null);
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
                viewModel.NextFrameCommand.Execute(null);
                Console.WriteLine();
            }
            return 0;
        }

        private void RefreshData(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
                
            

            switch (e.PropertyName)
            {
                case nameof(ViewModel.PlayerInfo):
                    Vector2Int pos = viewModel.PlayerInfo.Position;
                    Console.Clear();
                    Console.SetCursorPosition(pos.x, pos.y);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(pos);
                    break;
                case nameof(ViewModel.Score):
                    //Console.WriteLine($"Nowe punkty: {viewModel.Score}");
                    break;
                default:
                    break;
            }      
        }

    }
}
