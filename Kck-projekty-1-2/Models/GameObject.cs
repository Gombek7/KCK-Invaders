using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekty_1_2.Models
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
        public Coords Coords { get; private set; }
        public GameObject(Coords coords, int Health = 1)
        {
            this.Coords = coords;
            CurrentHealth = MaxHealth = Health;
        }
        public void MoveTo(Coords coords)
        {
            this.Coords = coords;
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
    }
}
