using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public class Cell
    {
        public int Row, Col;
        public int X, Y, Width, Height;
        public Plant PlantInCell = null;

        public Cell(int row, int col, int x, int y, int width, int height)
        {
            Row = row; Col = col; X = x; Y = y; Width = width; Height = height;
        }

        public void Draw()
        {
            SplashKit.FillRectangle(Color.DarkOliveGreen, X, Y, Width, Height);
            SplashKit.DrawRectangle(Color.Black, X, Y, Width, Height);

            if (PlantInCell != null)
                PlantInCell.Draw();
        }

        public bool Contains(Point2D pt)
        {
            return pt.X >= X && pt.X < X + Width && pt.Y >= Y && pt.Y < Y + Height;
        }
    }
}
