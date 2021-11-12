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
        private int score = 0;
        public int Score
        {
            get => score;
            private set
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

        private PlayerProjectile playerProjectile;

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
            GameObjectInfos.Add(new GameObjectInfo(player) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.Player});
            enemy = new WeakEnemy(new Vector2Int(5, 5));
            GameObjectInfos.Add(new GameObjectInfo(enemy) {GameObjectType = GameObjectInfo.GameObjectTypeEnum.WeakEnemy});
            playerProjectile = new PlayerProjectile(player.Position);
            GameObjectInfos.Add(new GameObjectInfo(playerProjectile) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.PlayerProjectile });
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
            playerProjectile = new PlayerProjectile(player.Position);
            Score++;
        }
        void NextFrame()
        {
            player.NextFrame();
            GameObjectInfos[0] = new GameObjectInfo(player) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.Player };
            enemy.NextFrame();
            GameObjectInfos[1] = new GameObjectInfo(enemy) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.WeakEnemy };
            if (playerProjectile != null)
            {
                playerProjectile?.NextFrame();
                GameObjectInfos[2] = new GameObjectInfo(playerProjectile) { GameObjectType = GameObjectInfo.GameObjectTypeEnum.PlayerProjectile };
            }
        }
    }
}
