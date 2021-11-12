using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class GameObject
    {
        public bool IsDestroyed
        {
            get
            {
                return MaxHealth <= 0;
            }
        }
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; } = 1;
        public Vector2Int Position { get; private set; }
        public Hitbox Hitbox { get; protected set; } = new Hitbox();

        public GameObject(Vector2Int coords, int Health = 1)
        {
            Position = coords;
            CurrentHealth = MaxHealth = Health;
        }
        public void MoveTo(Vector2Int coords)
        {
            if ((coords + Hitbox.UpperLeftCorner).IsCorrectCoords() && (coords+Hitbox.RightDownCorner).IsCorrectCoords())
                this.Position = coords;
        }
        public void Hit(int damage = 1)
        {
            CurrentHealth -= damage;
        }
        public void Heal(int heal = 1)
        {
            if (CurrentHealth < MaxHealth)
                CurrentHealth += heal;
        }
        public virtual void NextFrame()
        {

        }
    }
}
