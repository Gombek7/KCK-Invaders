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

        private bool movingRight;
        private bool movingLeft;
        private Player player;
        private WeakEnemy enemy;

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
            player = new Player(new Vector2Int(0, 20), 3);
            GameObjectInfos.Add(new GameObjectInfo(player));
            enemy = new WeakEnemy(new Vector2Int(5, 5));
            GameObjectInfos.Add(new GameObjectInfo(enemy));

        }

        void MoveRight()
        {
            movingRight = true;
            if (movingLeft)
                movingLeft = false;
            NextFrame();
        }
        void MoveLeft()
        {
            movingLeft = true;
            if (movingRight)
                movingRight = false;
            NextFrame();
        }
        void Shoot()
        {
            enemy.MoveTo(GameObjectInfos[0].Position + new Vector2Int(1, 0));
            GameObjectInfos[1] = new GameObjectInfo(enemy);
        }
        void NextFrame()
        {
            if (movingRight)
            {
                player.MoveTo(player.Position + new Vector2Int(1, 0));
                GameObjectInfos[0] = new GameObjectInfo(player);
                //PlayerInfo = new GameObjectInfo(player);
                //PlayerInfo.Position = new Vector2Int(player.Position);
                OnPropertyChange(nameof(PlayerInfo));
                movingRight = false;
                Score++;
            }
            else if (movingLeft)
            {
                player.MoveTo(player.Position + new Vector2Int(-1, 0));
                GameObjectInfos[0] = new GameObjectInfo(player);
                //PlayerInfo = new GameObjectInfo(player);
                //PlayerInfo.Position = new Vector2Int(player.Position);
                OnPropertyChange(nameof(PlayerInfo));
                movingLeft = false;
                Score++;
            }
        }
    }
}
