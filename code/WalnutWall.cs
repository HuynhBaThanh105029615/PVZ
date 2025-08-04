using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public class WalnutWall : Plant
    {
        private int _health;

        public WalnutWall(int x, int y) : base(x, y, PlantType.Walnut)
        {
            _health = 500; // Higher health for tanking
        }

        public override void Update(List<Pea> peas, List<Sun> suns) 
        {
            if (!IsAlive) return;

            Draw();
        }

        public override void Draw()
        {
            DrawingOptions opts = SplashKit.OptionScaleBmp(0.25f, 0.25f);
            Bitmap Walnut = SplashKit.LoadBitmap("Walnut", "Resources/Images/Walnut.png");
            SplashKit.DrawBitmap(Walnut, X - 120, Y - 170, opts);
        }

        public override void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
}
