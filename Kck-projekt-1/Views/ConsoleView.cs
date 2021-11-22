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
        //data from viewModel
        //GameObjectInfo ;

        //art
        //TODO: numeracja klatek jest wspólna dla wszystkich. To błąd!
        Art playerArt;
        Art enemyTierIArt;
        Art enemyTierIIArt;
        Art enemyTierIIIArt;
        Art obstacleArt;
        Art playerProjectileArt;
        Art enemyProjectileArt;
        Art explosionArt;
        Art titleArt;
        EffectPlayer effectPlayer;
        public ConsoleView()
        {
            viewModel = ViewModel.Instance;
            viewModel.PropertyChanged += RefreshData;
            viewModel.GameObjectInfos.CollectionChanged += ObjectsChanged;

            //Load art
            playerArt = new Art(@"Art\player.txt");
            enemyTierIArt = new Art(@"Art\enemyTierI.txt")
            {
                Color = ConsoleColor.DarkGray,
                NextFrameDelay = 1
            };
            enemyTierIIArt = new Art(@"Art\enemyTierII.txt")
            {
                Color = ConsoleColor.DarkGreen,
                NextFrameDelay = 1
            };
            enemyTierIIIArt = new Art(@"Art\enemyTierIII.txt")
            {
                Color = ConsoleColor.Green,
                NextFrameDelay = 1
            };
            obstacleArt = new Art(@"Art\obstacle.txt")
            {
                Color = ConsoleColor.Green
            };
            playerProjectileArt = new Art(@"Art\playerProjectile.txt")
            {
                Color = ConsoleColor.Blue
            };
            enemyProjectileArt = new Art(@"Art\enemyProjectile.txt")
            {
                Color = ConsoleColor.Green
            };
            explosionArt = new Art(@"Art\explosion.txt")
            {
                Color = ConsoleColor.Yellow
            };
            titleArt = new Art(@"Art\title.txt")
            {
                Color = ConsoleColor.DarkGreen
            };

            effectPlayer = new EffectPlayer();
            //Configure Console
            Console.Title = "KCK Invaders by Jarosław Dakowicz";
            Console.SetWindowSize(GameConfig.Width + 20, GameConfig.Height + 2);
            Console.CursorVisible = false;

        }
        public int Start()
        {
            WelcomeScreen();
            GameScreen();
            ConsoleUtils.Fill(' ',0, 0, GameConfig.Width-1, GameConfig.Height-1);
            viewModel.ManualRefreshDataCommand.Execute(null);
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
                effectPlayer.UpdateEffects();
            }
            return 0;
        }
        private void WelcomeScreen()
        {
            Console.SetWindowSize(118, 35);
            titleArt.Draw(1, 1, 0);

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            ConsoleUtils.Fill('=',0,10,118,10);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            ConsoleUtils.DrawBorder(20, 12, 98, 33, ConsoleColor.DarkCyan, ConsoleColor.White);

            enemyTierIArt.Draw(50, 14);
            Console.SetCursorPosition(58, 14);
            Console.Write("===\u001b[1B\u001b[3D===");
            Console.SetCursorPosition(64, 14);
            ConsoleUtils.PrintBigNumber(5, 2);

            enemyTierIIArt.Draw(50, 18);
            Console.SetCursorPosition(58, 18);
            Console.Write("===\u001b[1B\u001b[3D===");
            Console.SetCursorPosition(64, 18);
            ConsoleUtils.PrintBigNumber(10, 2);

            enemyTierIIIArt.Draw(50, 22);
            Console.SetCursorPosition(58, 22);
            Console.Write("===\u001b[1B\u001b[3D===");
            Console.SetCursorPosition(64, 22);
            ConsoleUtils.PrintBigNumber(20, 2);

            Console.SetCursorPosition(48, 31);
            Console.Write("PRESS SPACE TO START!");
            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
            {

            }

            ConsoleUtils.Fill(' ', 0, 0, 118, 35);
        }
        private void GameScreen()
        {
            //Configure Console
            Console.SetWindowSize(GameConfig.Width + 20, GameConfig.Height + 2);
            Console.CursorVisible = false;


            //Draw Basic UI
            ConsoleUtils.DrawBorder(-1, -1, GameConfig.Width, GameConfig.Height, ConsoleColor.Black, ConsoleColor.Cyan);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(GameConfig.Width + 6, 0);
            Console.Write("SCORE");
            Console.SetCursorPosition(GameConfig.Width + 1, 1);
            ConsoleUtils.PrintBigNumber(0, 5);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(GameConfig.Width + 6, 4);
            Console.Write("LIFES");
            Console.SetCursorPosition(GameConfig.Width + 1, 5);
            ConsoleUtils.PrintBigNumber(viewModel.Lifes, 5);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(GameConfig.Width + 5, 9);
            Console.Write("RANKING");

            Console.ForegroundColor = ConsoleColor.White;


        }

        private void ObjectsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach(GameObjectInfo objectInfo in e.NewItems)
                        DrawObject(objectInfo);
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
                        if(!objectInfo.IsDestroyed)
                            ClearObject(objectInfo);
                    }
                    foreach (GameObjectInfo objectInfo in e.NewItems)
                    {
                        if (objectInfo.IsDestroyed && (
                                objectInfo.GameObjectType == GameObjectInfo.GameObjectTypeEnum.Player ||
                                objectInfo.GameObjectType == GameObjectInfo.GameObjectTypeEnum.EnemyTierI ||
                                objectInfo.GameObjectType == GameObjectInfo.GameObjectTypeEnum.EnemyTierII ||
                                objectInfo.GameObjectType == GameObjectInfo.GameObjectTypeEnum.EnemyTierIII ||
                                objectInfo.GameObjectType == GameObjectInfo.GameObjectTypeEnum.EnemyTierIV))
                        {
                            Vector2Int effectPosition = objectInfo.Position + objectInfo.Hitbox.UpperLeftCorner;
                            effectPlayer.PlayEffect(effectPosition.x, effectPosition.y, explosionArt);
                        }
                        else
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
                    ConsoleUtils.Fill(' ', GameConfig.Width + 1, 1, GameConfig.Width + 16, 3);
                    Console.SetCursorPosition(GameConfig.Width + 1, 1);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    ConsoleUtils.PrintBigNumber(viewModel.Score, 5);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case nameof(ViewModel.Lifes):
                    ConsoleUtils.Fill(' ', GameConfig.Width + 1, 5, GameConfig.Width + 16, 7);
                    Console.SetCursorPosition(GameConfig.Width + 1, 5);
                    Console.ForegroundColor = ConsoleColor.Red;
                    ConsoleUtils.PrintBigNumber(viewModel.Lifes, 5);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }      
        }

        private void DrawObject(GameObjectInfo objectInfo)
        {
            if (objectInfo.IsDestroyed)
            {
                return;
            }

            Vector2Int UL = objectInfo.Position + objectInfo.Hitbox.UpperLeftCorner;
            Console.SetCursorPosition(UL.x, UL.y);
            switch (objectInfo.GameObjectType)
            {
                case GameObjectInfo.GameObjectTypeEnum.Player:
                    playerArt.Draw(objectInfo.Skin);
                    break;
                case GameObjectInfo.GameObjectTypeEnum.EnemyTierI:
                    enemyTierIArt.Draw(objectInfo.Skin);
                    break;
                case GameObjectInfo.GameObjectTypeEnum.EnemyTierII:
                    enemyTierIIArt.Draw(objectInfo.Skin);
                    break;
                case GameObjectInfo.GameObjectTypeEnum.EnemyTierIII:
                    enemyTierIIIArt.Draw(objectInfo.Skin);
                    break;
                case GameObjectInfo.GameObjectTypeEnum.Obstacle:
                    //Vector2Int l = objectInfo.Position + objectInfo.Hitbox.UpperLeftCorner;
                    //Vector2Int r = objectInfo.Position + objectInfo.Hitbox.RightDownCorner;
                    //ConsoleUtils.DrawBorder(l.x, l.y, r.x, r.y, ConsoleColor.Black, ConsoleColor.Green);
                    obstacleArt.Draw(objectInfo.Skin);
                    break;
                case GameObjectInfo.GameObjectTypeEnum.PlayerProjectile:
                    playerProjectileArt.Draw(objectInfo.Skin);
                    break;
                case GameObjectInfo.GameObjectTypeEnum.EnemyProjectile:
                    enemyProjectileArt.Draw(objectInfo.Skin);
                    break;
            }

            //---------Draw Hitbox
            //Vector2Int UL = objectInfo.Position + objectInfo.Hitbox.UpperLeftCorner;
            ////UL.CropToGameBorders();
            //Vector2Int RD = objectInfo.Position + objectInfo.Hitbox.RightDownCorner;
            ////RD.CropToGameBorders();
            //ConsoleUtils.DrawBorder(UL.x, UL.y, RD.x, RD.y, ConsoleColor.Black, ConsoleColor.Green);

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