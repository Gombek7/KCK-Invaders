using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Kck_projekt_1.ViewModels;
using Kck_projekt_1.Models;
using Kck_projekt_1.Utils;
using System.Media;
using System.Runtime.InteropServices;

namespace Kck_projekt_1.Views
{

    class ConsoleView
    {
        //enable virtual terminal processing https://www.jerriepelser.com/blog/using-ansi-color-codes-in-net-console-apps/
        
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        

        private ViewModel viewModel;
        //data from viewModel
        //GameObjectInfo ;

        //art
        Art playerArt;
        Art enemyTierIArt;
        Art enemyTierIIArt;
        Art enemyTierIIIArt;
        Art obstacleArt;
        Art playerProjectileArt;
        Art enemyProjectileArt;
        Art explosionArt;
        Art titleArt;
        Art gameoverArt;
        Art youwonArt;

        EffectPlayer effectPlayer;
        //sounds
        SoundPlayer explosion;
        SoundPlayer fastinvader1;
        SoundPlayer fastinvader2;
        SoundPlayer fastinvader3;
        SoundPlayer fastinvader4;
        SoundPlayer invaderkilled;
        SoundPlayer shoot;
        SoundPlayer spaceinvaders1;
        SoundPlayer ufo_highpitch;
        SoundPlayer ufo_lowpitch;
       

        public ConsoleView()
        {
            viewModel = ViewModel.Instance;
            viewModel.PropertyChanged += RefreshData;
            viewModel.GameObjectInfos.CollectionChanged += ObjectsChanged;

            //Load art
            playerArt = new Art(@"Art\player.txt")
            {
                Color = ConsoleColor.Green
            };
            enemyTierIArt = new Art(@"Art\enemyTierI.txt")
            {
                Color = ConsoleColor.DarkCyan,
                NextFrameDelay = 1
            };
            enemyTierIIArt = new Art(@"Art\enemyTierII.txt")
            {
                Color = ConsoleColor.DarkGreen,
                NextFrameDelay = 1
            };
            enemyTierIIIArt = new Art(@"Art\enemyTierIII.txt")
            {
                Color = ConsoleColor.White,
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
            gameoverArt = new Art(@"Art\gameover.txt")
            {
                Color = ConsoleColor.DarkRed
            };
            youwonArt = new Art(@"Art\youwon.txt")
            {
                Color = ConsoleColor.Green
            };

            effectPlayer = new EffectPlayer();

            explosion = new SoundPlayer(@"Sounds\explosion.wav");
            explosion.Load();
            fastinvader1 = new SoundPlayer(@"Sounds\fastinvader1.wav");
            fastinvader1.Load();
            fastinvader2 = new SoundPlayer(@"Sounds\fastinvader2.wav");
            fastinvader2.Load();
            fastinvader3 = new SoundPlayer(@"Sounds\fastinvader3.wav");
            fastinvader3.Load();
            fastinvader4 = new SoundPlayer(@"Sounds\fastinvader4.wav");
            fastinvader4.Load();
            invaderkilled = new SoundPlayer(@"Sounds\invaderkilled.wav");
            invaderkilled.Load();
            shoot = new SoundPlayer(@"Sounds\shoot.wav");
            shoot.Load();
            spaceinvaders1 = new SoundPlayer(@"Sounds\spaceinvaders1.wav");
            spaceinvaders1.Load();
            ufo_highpitch = new SoundPlayer(@"Sounds\ufo_highpitch.wav");
            ufo_highpitch.Load();
            ufo_lowpitch = new SoundPlayer(@"Sounds\ufo_lowpitch.wav");
            ufo_lowpitch.Load();

            //Configure Console
            Console.Title = "KCK Invaders by Jarosław Dakowicz";
            Console.SetWindowSize(GameConfig.Width + 20, GameConfig.Height + 2);
            Console.CursorVisible = false;
            
            
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                return;
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                Console.ReadKey();
                return;
            }
        }
        public int Start()
        {
            WelcomeScreen();
            while (true)
            {
                if (GameScreen()) //round won
                {
                    if (!YouWonScreen())
                        return 0;
                    viewModel.NextRoundCommand.Execute(null);
                }
                else //round lost or ended by user
                {
                    if (!GameOverScreen())
                        return 0;
                    viewModel.RestartCommand.Execute(null);
                }
            }
            
        }
        private void WelcomeScreen()
        {
            spaceinvaders1.Play();
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
        // returns true if round finished, else false
        private bool GameScreen()
        {
            spaceinvaders1.Stop();
            //Configure Console
            Console.SetWindowSize(GameConfig.Width + 20, GameConfig.Height + 4);
            Console.CursorVisible = false;


            //Draw Basic UI
            ConsoleUtils.DrawBorder(-1, -1, GameConfig.Width, GameConfig.Height, ConsoleColor.Black, ConsoleColor.Cyan);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(GameConfig.Width + 6, 0);
            Console.Write("SCORE");
            Console.SetCursorPosition(GameConfig.Width + 1, 1);
            ConsoleUtils.PrintBigNumber(viewModel.Score, 5);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(GameConfig.Width + 6, 4);
            Console.Write("LIFES");
            Console.SetCursorPosition(GameConfig.Width + 1, 5);
            ConsoleUtils.PrintBigNumber(viewModel.Lifes, 5);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(GameConfig.Width + 3, 8);
            Console.Write("HIGH SCORE");
            Console.SetCursorPosition(GameConfig.Width + 1, 9);
            ConsoleUtils.PrintBigNumber(viewModel.HighScore, 5);

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.SetCursorPosition(1, GameConfig.Height + 2);
            Console.Write("\u001b[47m<-\u001b[40m MOVE LEFT    \u001b[47m->\u001b[40m MOVE RIGHT    \u001b[47mSPACE\u001b[40m SHOOT    \u001b[47mR\u001b[40m RESTART    \u001b[47mESC\u001b[40m EXIT");

            Console.ForegroundColor = ConsoleColor.White;



            viewModel.ManualRefreshDataCommand.Execute(null);
            //Game Loop
            while (!viewModel.GameWon)
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
                            shoot.Play();
                            viewModel.ShootCommand.Execute(null);
                            break;
                        case ConsoleKey.R:
                            viewModel.RestartCommand.Execute(null);
                            Thread.Sleep(1000);
                            break;
                        case ConsoleKey.Escape:
                            ConsoleUtils.Fill(' ', 0, 0, GameConfig.Width + 20, GameConfig.Height + 4);
                            effectPlayer.Clear();
                            return false;
                            break;
                        default:
                            break;
                    }
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);
                }
                Thread.Sleep(1000 / GameConfig.Fps);
                viewModel.NextFrameCommand.Execute(null);
                effectPlayer.UpdateEffects();
                if(viewModel.GameOver)
                {
                    while(effectPlayer.Count > 0)
                    {
                        Thread.Sleep(1000 / GameConfig.Fps);
                        effectPlayer.UpdateEffects();
                    }
                    Thread.Sleep(1000);
                    ConsoleUtils.Fill(' ', 0, 0, GameConfig.Width + 20, GameConfig.Height + 4);
                    return false;
                }

            }
            while (effectPlayer.Count > 0)
            {
                Thread.Sleep(1000 / GameConfig.Fps);
                effectPlayer.UpdateEffects();
            }
            Thread.Sleep(1000);
            ConsoleUtils.Fill(' ', 0, 0, GameConfig.Width + 20, GameConfig.Height + 4);
            return true;
        }
        // true if continue
        private bool GameOverScreen()
        {
            spaceinvaders1.Play();
            Console.SetWindowSize(75, 20);
            gameoverArt.Draw(5, 1, 0);

            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleUtils.Fill('=', 0, 5, 75, 5);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(29, 8);
            Console.Write("YOUR FINAL SCORE:");
            Console.SetCursorPosition(30, 9);
            ConsoleUtils.PrintBigNumber(viewModel.Score, 5);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(32, 12);
            Console.Write("HIGH SCORE:");
            Console.SetCursorPosition(30, 13);
            ConsoleUtils.PrintBigNumber(viewModel.HighScore, 5);

            Console.SetCursorPosition(2, 19);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("PRESS SPACE TO RESTART!");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(50, 19);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("PRESS ESCAPE TO EXIT :(");
            Console.ForegroundColor = ConsoleColor.White;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Spacebar:
                            ConsoleUtils.Fill(' ', 0, 0, 75, 20);
                            return true;
                            break;
                        case ConsoleKey.Escape:
                            ConsoleUtils.Fill(' ', 0, 0, 75, 20);
                            return false;
                            break;
                        default:
                            break;
                    }
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);
                }
            }
        }
        private bool YouWonScreen()
        {
            spaceinvaders1.Play();
            Console.SetWindowSize(75, 20);
            youwonArt.Draw(12, 1, 0);

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleUtils.Fill('=', 0, 5, 75, 5);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(28, 8);
            Console.Write("YOUR CURRENT SCORE:");
            Console.SetCursorPosition(30, 9);
            ConsoleUtils.PrintBigNumber(viewModel.Score, 5);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(32, 12);
            Console.Write("HIGH SCORE:");
            Console.SetCursorPosition(30, 13);
            ConsoleUtils.PrintBigNumber(viewModel.HighScore, 5);

            Console.SetCursorPosition(2, 19);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("PRESS SPACE TO CONTINUE!");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(50, 19);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("PRESS ESCAPE TO EXIT :(");
            Console.ForegroundColor = ConsoleColor.White;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Spacebar:
                            ConsoleUtils.Fill(' ', 0, 0, 75, 20);
                            return true;
                            break;
                        case ConsoleKey.Escape:
                            ConsoleUtils.Fill(' ', 0, 0, 75, 20);
                            return false;
                            break;
                        default:
                            break;
                    }
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);
                }
            }
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
                            explosion.Play();
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
                    invaderkilled.Play();
                    ConsoleUtils.Fill(' ', GameConfig.Width + 1, 5, GameConfig.Width + 16, 7);
                    Console.SetCursorPosition(GameConfig.Width + 1, 5);
                    Console.ForegroundColor = ConsoleColor.Red;
                    ConsoleUtils.PrintBigNumber(viewModel.Lifes, 5);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case nameof(ViewModel.HighScore):
                    ConsoleUtils.Fill(' ', GameConfig.Width + 1, 9, GameConfig.Width + 16, 11);
                    Console.SetCursorPosition(GameConfig.Width + 1, 9);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    ConsoleUtils.PrintBigNumber(viewModel.HighScore, 5);
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