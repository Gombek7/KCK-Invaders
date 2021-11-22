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
        private int lifes = 0;
        public int Lifes {
            get => lifes;
            set
            {
                if (lifes != value)
                {
                    lifes = value;
                    OnPropertyChange(nameof(Lifes));
                }
            }
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
        private Obstacle[] obstacles;

        public ICommand MoveRightCommand { get; }
        public ICommand MoveLeftCommand { get; }
        public ICommand ShootCommand { get; }
        public ICommand NextFrameCommand { get; }
        public ICommand ManualRefreshDataCommand { get; }

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
            ManualRefreshDataCommand = new RelayCommand(ManualRefreshData);

            //game init
            GameObjectInfos = new ObservableCollection<GameObjectInfo>();
            player = new Player(new Vector2Int(GameConfig.Width / 2, 30), 3);
            Lifes = 3;
            enemies = new Enemy[15];
            for (int i = 0; i < 5; i++)
            {
                enemies[i] = new EnemyTierIII(new Vector2Int(2 + i * 7, 2));
                enemies[i].Projectile.AddTarget(player);
                player.Projectile.AddTarget(enemies[i]);
                player.Projectile.AddTarget(enemies[i].Projectile);
            }
            for (int i = 0; i < 5; i++)
            {
                enemies[i + 5] = new EnemyTierII(new Vector2Int(2 + i * 7, 7));
                enemies[i + 5].Projectile.AddTarget(player);
                player.Projectile.AddTarget(enemies[i + 5]);
                player.Projectile.AddTarget(enemies[i + 5].Projectile);
            }
            for (int i = 0; i < 5; i++)
            {
                enemies[i + 10] = new EnemyTierI(new Vector2Int(2 + i * 7, 10));
                enemies[i + 10].Projectile.AddTarget(player);
                player.Projectile.AddTarget(enemies[i + 10]);
                player.Projectile.AddTarget(enemies[i + 10].Projectile);
            }

            obstacles = new Obstacle[5];
            for (int i = 0; i < 5; i++)
            {
                obstacles[i] = new Obstacle(new Vector2Int(2 + i * 7, 25));
                foreach (Enemy enemy in enemies)
                    enemy.Projectile.AddTarget(obstacles[i]);
                player.Projectile.AddTarget(obstacles[i]);
            }
        }

        void ManualRefreshData()
        {
            player.UpdateInfo();
            foreach (Enemy enemy in enemies)
                enemy.UpdateInfo();
            foreach (Obstacle obstacle in obstacles)
                obstacle.UpdateInfo();
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
