﻿using Kck_projekt_1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kck_projekt_1.Utils
{
    class GameObjectInfo : INotifyPropertyChanged
    {
        public enum GameObjectTypeEnum
        {
            Player,
            EnemyTierI,
            EnemyTierII,
            EnemyTierIII,
            EnemyTierIV,
            Obstacle,
            PlayerProjectile,
            EnemyProjectile
        }
        public GameObjectTypeEnum GameObjectType = GameObjectTypeEnum.EnemyTierI;

        private Vector2Int position;
        public Vector2Int Position {
            get => position;
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChange(nameof(Position));
                }
            }
        }
        public int HP { get; set; }
        public Hitbox Hitbox { get; set; }

        public GameObjectInfo(GameObject gameObject)
        {
            Position = new Vector2Int(gameObject.Position);
            Hitbox = gameObject.Hitbox;
            HP = gameObject.CurrentHealth;
        }


        public override string ToString()
        {
            return $"GameObjectInfo Position: {Position} Hp: {HP}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
