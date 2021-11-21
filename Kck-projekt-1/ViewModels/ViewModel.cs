using Kck_projekt_1.Models;
using Kck_projekt_1.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Kck_projekt_1.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {
        private static ViewModel instance = null;
        public static ViewModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new ViewModel();
                return instance;
            }
        }
        private Random random = new Random();
        private int score = 0;
        public int Score
        {
            get => score;
            set
            {
                if (score != value)
                {
                    score = value;
                    OnPropertyChange(nameof(Score));
                }
            }
        }
        public int Lifes {
            get => player.CurrentHealth;
        }
        private GameObjectInfo playerInfo;
        public GameObjectInfo PlayerInfo
        {
            get => playerInfo;
            private set
            {
                if (playerInfo != value)
                {
                    playerInfo = value;
                    OnPropertyChange(nameof(PlayerInfo));
                }
            }
        }
        public ObservableCollection<GameObjectInfo> GameObjectInfos { get; private set; }

        private Player player;
        private Enemy[] enemies;

        public ICommand MoveRightCommand { get; }
        public ICommand MoveLeftCommand { get; }
        public ICommand ShootCommand { get; }
        public ICommand NextFrameCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private ViewModel()
        {
            //commands config
            MoveRightCommand = new RelayCommand(MoveRight);
            MoveLeftCommand = new RelayCommand(MoveLeft);
            ShootCommand = new RelayCommand(Shoot);
            NextFrameCommand = new RelayCommand(NextFrame);

            //game init
            GameObjectInfos = new ObservableCollection<GameObjectInfo>();
            player = new Player(new Vector2Int(GameConfig.Width/2, 20), 3);

            enemies = new Enemy[15];
            for (int i = 0; i < 5; i++)
            {
                enemies[i] = new EnemyTierI(new Vector2Int(2 + i * 5, 2));
                enemies[i].Projectile.AddTarget(player);
                player.Projectile.AddTarget(enemies[i]);
            }
            for (int i = 0; i < 5; i++)
            {
                enemies[i + 5] = new EnemyTierI(new Vector2Int(2 + i * 5, 5));
                enemies[i + 5].Projectile.AddTarget(player);
                player.Projectile.AddTarget(enemies[i + 5]);
            }
            for (int i = 0; i < 5; i++)
            {
                enemies[i + 10] = new EnemyTierI(new Vector2Int(2 + i * 5, 8));
                enemies[i + 10].Projectile.AddTarget(player);
                player.Projectile.AddTarget(enemies[i + 10]);
            }
        }

        void MoveRight()
        {
            player.MovingRight = true;
        }
        void MoveLeft()
        {
            player.MovingLeft = true;
        }
        void Shoot()
        {
            player.Shoot();        
        }
        void NextFrame()
        {
            player.NextFrame();
            foreach (Enemy enemy in enemies)
                enemy.CheckBorderCollision();
            foreach(Enemy enemy in enemies)
                enemy.NextFrame();
            Enemy.borderCollision = false;
        }
    }
}
