using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public class Zombie
    {
        public float X, Y;
        private float speed = 0.5f;
        public int Health = 100;
        private int width = 70;
        private int height = 90;
        private int _attackCooldown = 0;

        public bool IsAlive => Health > 0;
        public Rectangle Rectangle => SplashKit.RectangleFrom(X, Y, width, height);

        public Zombie(float x, float y)
        {
            X = x; Y = y;
        }

        public void Update()
        {
            X -= speed;
        }

        public void Attack(List<Plant> plants)
        {
            foreach (var plant in plants)
            {
                // Collision: same row and overlapping rectangle
                if (Math.Abs(plant.Y - Y) < 10 && SplashKit.RectanglesIntersect(Rectangle, new Rectangle()
                {
                    X = plant.X,
                    Y = plant.Y,
                    Width = plant.Width,
                    Height = plant.Height
                }))
                {
                    speed = 0; // Stop moving while attacking
                    _attackCooldown++;

                    if (_attackCooldown >= 60) // Attack every ~1 second
                    {
                        plant.TakeDamage(20);
                        _attackCooldown = 0;
                    }

                    if (!plant.IsAlive)
                    {
                        plants.Remove(plant);
                        speed = 0.5f;
                    }

                    return; // Attack only one plant at a time
                }
            }

            speed = 0.5f; // Resume walking if no plant in front
        }

        public void Draw()
        {
            DrawingOptions opts = SplashKit.OptionScaleBmp(0.08f, 0.08f);
            Bitmap zombieBmp = SplashKit.LoadBitmap("Zombie", "Resources/Images/Zombie.png");
            SplashKit.DrawBitmap(zombieBmp, X - 750, Y - 1120, opts);
        }
    }

}
