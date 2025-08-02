using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public class Sun
    {
        public float X, Y;
        private float fallSpeed = 0.5f;
        private int radius = 25;
        public bool IsCollected = false;

        public Sun(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Update()
        {
            Y += fallSpeed;

            // Optional: limit Y so it doesn't fall forever
            if (Y > 875) Y = 875;
        }

        public void Draw()
        {
            SplashKit.FillCircle(Color.Orange, X, Y, radius);
            SplashKit.DrawText("+50", Color.White, X - 12, Y - 6);
        }

        public bool IsClicked(Point2D point)
        {
            return SplashKit.PointInCircle(point, new Circle { Center = new Point2D { X = X, Y = Y }, Radius = radius });
        }
    }
}
