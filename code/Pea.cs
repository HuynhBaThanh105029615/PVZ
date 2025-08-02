using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public class Pea
    {
        public float X, Y;
        private float speed = 7f;
        private int radius = 15;
        public bool IsAlive = true;

        public Rectangle Rectangle => SplashKit.RectangleFrom(X - radius, Y - radius, radius * 2, radius * 2);

        public Pea(float x, float y)
        {
            X = x; Y = y;
        }

        public void Update()
        {
            X += speed;
            if (X > 1600) IsAlive = false;
        }

        public void Draw()
        {
            SplashKit.FillCircle(Color.LawnGreen, X + 60, Y, radius);
        }
    }
}
