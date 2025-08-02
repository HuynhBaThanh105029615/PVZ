using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public class Sunflower : Plant
    {
        private int _sunCooldown;

        public Sunflower(int x, int y) : base(x, y, PlantType.Sunflower) 
        {
            _sunCooldown = 300;
        }

        public override void Update(List<Pea> peas, List<Sun> suns)
        {
            if (!IsAlive) return;

            if (_sunCooldown <= 0)
            {
                suns.Add(new Sun(X + Width / 2, Y));
                _sunCooldown = 300;
            }
            else
            {
                _sunCooldown--;
            }

            Draw();
        }

        public override void Draw()
        {
            DrawingOptions opts = SplashKit.OptionScaleBmp(0.3f, 0.3f);
            Bitmap Sunflower = SplashKit.LoadBitmap("Sunflower", "Resources/Images/Sunflower.png");
            SplashKit.DrawBitmap(Sunflower, X - 90, Y - 100, opts);
        }
    }
}
