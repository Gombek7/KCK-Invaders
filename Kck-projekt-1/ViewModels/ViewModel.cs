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

        public bool GameOver
        {
            get => Lifes <= 0;
        }
        public bool GameWon
        {
            get; private set;
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
        private List<Enemy> enemies;
        private List<Obstacle> obstacles;

        public ICommand MoveRightCommand { get; }
        public ICommand MoveLeftCommand { get; }
        public ICommand ShootCommand { get; }
        public ICommand NextFrameCommand { get; }
        public ICommand ManualRefreshDataCommand { get; }
        public ICommand RestartCommand { get;  }
        public ICommand NextRoundCommand { get;  }

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
            RestartCommand = new RelayCommand(Restart);
            NextRoundCommand = new RelayCommand(NextRound);

            //game init
            GameObjectInfos = new ObservableCollection<GameObjectInfo>();
            player = new Player(new Vector2Int(GameConfig.Width / 2, GameConfig.Height - 2), 3);
            Lifes = 3;
            GameWon = false;

            int enemiesLineCount = (GameConfig.Width - 10) / 7;
            enemies = new List<Enemy>();
            for (int i = 0; i < enemiesLineCount; i++)
            {
                Enemy newEnemyIII = new EnemyTierIII(new Vector2Int(2 + i * 7, 2));
                newEnemyIII.Projectile.AddTarget(player);
                player.Projectile.AddTarget(newEnemyIII);
                player.Projectile.AddTarget(newEnemyIII.Projectile);
                enemies.Add(newEnemyIII);
            }
            for (int i = 0; i < enemiesLineCount; i++)
            {
                Enemy newEnemyII = new EnemyTierII(new Vector2Int(3 + i * 7, 6));
                newEnemyII.Projectile.AddTarget(player);
                player.Projectile.AddTarget(newEnemyII);
                player.Projectile.AddTarget(newEnemyII.Projectile);
                enemies.Add(newEnemyII);
                newEnemyII = new EnemyTierII(new Vector2Int(3 + i * 7, 10));
                newEnemyII.Projectile.AddTarget(player);
                player.Projectile.AddTarget(newEnemyII);
                player.Projectile.AddTarget(newEnemyII.Projectile);
                enemies.Add(newEnemyII);
            }
            for (int i = 0; i < enemiesLineCount; i++)
            {
                Enemy newEnemyI = new EnemyTierI(new Vector2Int(2 + i * 7, 14));
                newEnemyI.Projectile.AddTarget(player);
                player.Projectile.AddTarget(newEnemyI);
                player.Projectile.AddTarget(newEnemyI.Projectile);
                enemies.Add(newEnemyI);
                newEnemyI = new EnemyTierI(new Vector2Int(2 + i * 7, 18));
                newEnemyI.Projectile.AddTarget(player);
                player.Projectile.AddTarget(newEnemyI);
                player.Projectile.AddTarget(newEnemyI.Projectile);
                enemies.Add(newEnemyI);
            }

            obstacles = new List<Obstacle>();
            for (int i=5; i<=(GameConfig.Width-15); i+=17 )
                initBaricade(i, GameConfig.Height - 10);
        }


        void initBaricade(int x, int y)
        {
            for (int i = 3; i <= 7; i += 2)
                initObstacle(x + i, y);
            for (int i = 1; i <= 9; i += 2)
                initObstacle(x + i, y+1);
            for (int i = 1; i <= 3; i += 2)
            {
                initObstacle(x + i, y + 2);
                initObstacle(x + i, y + 3);
            }
            for (int i = 7; i <= 9; i += 2)
            {
                initObstacle(x + i, y + 2);
                initObstacle(x + i, y + 3);
            }
        }
        void initObstacle(int x, int y)
        {
            Obstacle newObstacle = new Obstacle(new Vector2Int(x, y));
            foreach (Enemy enemy in enemies)
                enemy.Projectile.AddTarget(newObstacle);
            player.Projectile.AddTarget(newObstacle);
            obstacles.Add(newObstacle);
        }
        void ManualRefreshData()
        {
            player.UpdateInfo();
            player.Projectile.UpdateInfo();
            foreach (Enemy enemy in enemies)
            {
                enemy.UpdateInfo();
                enemy.Projectile.UpdateInfo();
            }
                
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
            if (GameOver || GameWon)
                return;
            player.NextFrame();
            foreach (Enemy enemy in enemies)
                enemy.CheckBorderCollision();
            bool allEnemiesDestroyed = true;
            foreach(Enemy enemy in enemies)
            {
                enemy.NextFrame();
                if (enemy.Position.y + enemy.Hitbox.RightDownCorner.y> (GameConfig.Height - 10))
                {
                    player.Hit(player.CurrentHealth);
                    break;
                }
                if (!enemy.IsDestroyed)
                    allEnemiesDestroyed = false;
            }
            Enemy.borderCollision = false;

            if (allEnemiesDestroyed)
                GameWon = true;
        }
        private void ResetEnemies()
        {
            int enemiesLineCount = (GameConfig.Width - 10) / 7;
            int index = 0;
            for (int i = 0; i < enemiesLineCount; i++)
            {
                enemies[index++].Reincarnate(new Vector2Int(2 + i * 7, 2));
            }
            for (int i = 0; i < enemiesLineCount; i++)
            {
                enemies[index++].Reincarnate(new Vector2Int(3 + i * 7, 6));
                enemies[index++].Reincarnate(new Vector2Int(3 + i * 7, 10));
            }
            for (int i = 0; i < enemiesLineCount; i++)
            {
                enemies[index++].Reincarnate(new Vector2Int(2 + i * 7, 14));
                enemies[index++].Reincarnate(new Vector2Int(2 + i * 7, 18));
            }
        }
        private void ResetObstacles()
        {
            foreach (Obstacle obstacle in obstacles)
                obstacle.Reincarnate(obstacle.Position);
        }
        private void Restart()
        {
            player.Reincarnate(new Vector2Int(GameConfig.Width / 2, GameConfig.Height - 2), 3);
            Lifes = 3;
            Score = 0;
            GameWon = false;

            ResetEnemies();
            ResetObstacles();
        }
        private void NextRound()
        {
            Lifes += 3;
            player.Reincarnate(new Vector2Int(GameConfig.Width / 2, GameConfig.Height - 2), Lifes);
            GameWon = false;

            ResetEnemies();
            ResetObstacles();
        }
    }
}
