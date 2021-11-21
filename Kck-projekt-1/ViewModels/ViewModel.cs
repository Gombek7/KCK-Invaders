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
        private WeakEnemy enemy;

        //private PlayerProjectile playerProjectile;
        private EnemyProjectile enemyProjectile;

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
        public ViewModel()
        {
            //commands config
            MoveRightCommand = new RelayCommand(MoveRight);
            MoveLeftCommand = new RelayCommand(MoveLeft);
            ShootCommand = new RelayCommand(Shoot);
            NextFrameCommand = new RelayCommand(NextFrame);

            //game init
            GameObjectInfos = new ObservableCollection<GameObjectInfo>();
            player = new Player(new Vector2Int(GameConfig.Width/2, 20), 3);
            enemy = new WeakEnemy(new Vector2Int(5, 5));
            player.Projectile.AddTarget(enemy);
            enemy.Projectile.AddTarget(player);
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
            enemy.NextFrame();

        }
    }
}
