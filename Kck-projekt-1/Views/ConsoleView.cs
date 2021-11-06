﻿using System;
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
        //data from viewModel
        //GameObjectInfo ;

        //art
        string playerArt;
        public ConsoleView()
        {
            //Load art
            playerArt = ConsoleUtils.LoadArt(@"C:\Programy\Projekty Visual Studio\Kck-projekty-1-2\Kck-projekt-1\Art\player.txt");

            //Draw Basic UI
            //ConsoleUtils.DrawBorder(-2, -2, 2, 2);
            ConsoleUtils.DrawBorder(-1, -1, GameConfig.Width, GameConfig.Height,ConsoleColor.Black, ConsoleColor.Cyan);
            //ConsoleUtils.Fill(' ',0, 0, GameConfig.Width-1, GameConfig.Height-1);
            Console.SetCursorPosition(0, 0);
            ConsoleUtils.PrintBigNumber(1, 5);

        }
        public int Start()
        {
            viewModel = new ViewModel();
            viewModel.PropertyChanged += RefreshData;
            viewModel.GameObjectInfos.CollectionChanged += ObjectsChanged;

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
            }
            return 0;
        }

        private void ObjectsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach(GameObjectInfo objectInfo in e.NewItems)
                    {
                        DrawObject(objectInfo);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (GameObjectInfo objectInfo in e.OldItems)
                    {
                        ClearObject(objectInfo);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    foreach (GameObjectInfo objectInfo in e.OldItems)
                    {
                        ClearObject(objectInfo);
                    }
                    foreach (GameObjectInfo objectInfo in e.NewItems)
                    {
                        DrawObject(objectInfo);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        private void RefreshData(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.PlayerInfo):
                    //updatePlayer();
                    break;
                case nameof(ViewModel.Score):
                    //Console.WriteLine($"Nowe punkty: {viewModel.Score}");
                    break;
                default:
                    break;
            }      
        }

        private void updatePlayer(GameObjectInfo oldPlayerInfo, GameObjectInfo newPlayerInfo)
        {
            Vector2Int UL, RD;
            if (oldPlayerInfo != null)
            {
                //clear last position
                UL = oldPlayerInfo.Position + oldPlayerInfo.Hitbox.UpperLeftCorner;
                UL.CropToGameBorders();
                RD = oldPlayerInfo.Position + oldPlayerInfo.Hitbox.RightDownCorner;
                RD.CropToGameBorders();
                ConsoleUtils.Fill(' ', UL.x, UL.y, RD.x, RD.y);
            }
            //playerInfo = viewModel.PlayerInfo;

            //draw player
            UL = newPlayerInfo.Position + newPlayerInfo.Hitbox.UpperLeftCorner;
            UL.CropToGameBorders();
            //RD = playerInfo.Position + playerInfo.Hitbox.RightDownCorner;
            //RD.CropToGameBorders();
            //ConsoleUtils.Fill('P', UL.x, UL.y, RD.x, RD.y);
            Console.SetCursorPosition(UL.x,UL.y);
            Console.Write(playerArt);
        }

        private void DrawObject(GameObjectInfo objectInfo)
        {
            Vector2Int UL = objectInfo.Position + objectInfo.Hitbox.UpperLeftCorner;
            //UL.CropToGameBorders();
            Vector2Int RD = objectInfo.Position + objectInfo.Hitbox.RightDownCorner;
            //RD.CropToGameBorders();
            ConsoleUtils.DrawBorder(UL.x, UL.y, RD.x, RD.y, ConsoleColor.Black, ConsoleColor.Green);

        }
        private void ClearObject(GameObjectInfo objectInfo)
        {
            Vector2Int UL = objectInfo.Position + objectInfo.Hitbox.UpperLeftCorner;
            //UL.CropToGameBorders();
            Vector2Int RD = objectInfo.Position + objectInfo.Hitbox.RightDownCorner;
            //RD.CropToGameBorders();
            ConsoleUtils.Fill(' ', UL.x, UL.y, RD.x, RD.y);
        }

    }
}