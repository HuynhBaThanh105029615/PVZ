using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public enum PlantType
    {
        PeaShooter,
        Sunflower,
        Walnut
    }
    public abstract class Plant
    {
        public int X, Y, Width, Height;

        protected int Health = 100;

        public bool IsAlive { get; protected set; } = true;

        public Plant(int x, int y, PlantType type)
        {
            X = x; Y = y;
            Width = 70; Height = 90;
        }

        public abstract void Update(List<Pea> peas, List<Sun> suns);

        public virtual void Draw()
        {
            // Default draw (override in subclasses)
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                IsAlive = false;
            }
        }
    }
}
