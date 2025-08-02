using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public class PeaShooter : Plant
    {
        private int _shootCooldown;

        public PeaShooter(int x, int y) : base(x, y, PlantType.PeaShooter) 
        {
            _shootCooldown = 120;
        }

        public override void Update(List<Pea> peas, List<Sun> suns)
        {
            if (!IsAlive) return;

            if (_shootCooldown <= 0)
            {
                peas.Add(new Pea(X + Width, Y + Height / 2));
                _shootCooldown = 120;
            }
            else
            {
                _shootCooldown--;
            }

            Draw();
        }

        public override void Draw()
        {
            DrawingOptions opts = SplashKit.OptionScaleBmp(0.25f, 0.25f);
            Bitmap Peashooter = SplashKit.LoadBitmap("Peashooter", "Resources/Images/PeaShooter.png");
            SplashKit.DrawBitmap(Peashooter, X - 140, Y - 140, opts);
        }
    }
}
