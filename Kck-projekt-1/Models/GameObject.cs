using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    abstract class GameObject
    {
        private static int idCounter = 0;
        private readonly int id = idCounter++;
        public int Id { get => id; }

        public bool IsDestroyed
        {
            get
            {
                return CurrentHealth <= 0;
            }
        }

        public int CurrentHealth { get; protected set; }
        public int MaxHealth { get; private set; } = 1;
        public Vector2Int Position { get; private set; }
        public Hitbox Hitbox { get; protected set; } = new Hitbox();

        public GameObject(Vector2Int coords, int Health = 1)
        {
            Position = coords;
            CurrentHealth = MaxHealth = Health;
            //ViewModel.Instance.GameObjectInfos.Add(new Utils.GameObjectInfo(this));
        }
        public void MoveTo(Vector2Int coords)
        {
            if ((coords + Hitbox.UpperLeftCorner).IsCorrectCoords() && (coords+Hitbox.RightDownCorner).IsCorrectCoords())
                this.Position = coords;
        }
        public virtual void Hit(int damage = 1)
        {
            CurrentHealth -= damage;
            if (IsDestroyed)
                UpdateInfo();
        }
        public void Heal(int heal = 1)
        {
            if (CurrentHealth < MaxHealth)
                CurrentHealth += heal;
        }
        public abstract void NextFrame();
        public bool Collides(GameObject other)
        {
            Vector2Int l1 = Hitbox.UpperLeftCorner + Position;
            Vector2Int r1 = Hitbox.RightDownCorner + Position;

            Vector2Int l2 = other.Hitbox.UpperLeftCorner + other.Position;
            Vector2Int r2 = other.Hitbox.RightDownCorner + other.Position;

            // if one hitbox is on left side of other
            if (l1.x > r2.x || l2.x > r1.x)
                return false;

            // if one hitbox is above other
            if (l1.y > r2.y|| l2.y > r1.y)
                return false;

            return true;
        }
        public virtual void Reincarnate(Vector2Int coords, int Health = 1)
        {
            Position = coords;
            CurrentHealth = MaxHealth = Health;
            UpdateInfo();
        }
        public abstract void UpdateInfo();
    }
}
