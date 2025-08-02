using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.code
{
    public class Field
    {
        private const int Rows = 5;
        private const int Cols = 9;
        private const int CellWidth = 1600 / Cols;
        private const int CellHeight = 750 / Rows;

        private Window _window;
        private Cell[,] _grid;
        private List<Pea> _peas;
        private List<Zombie> _zombies;
        private List<Sun> _suns;
        private Random _rand;
        private int _zombieSpawnCooldown;
        private int _sunCount;
        private PlantType _selectedPlant;

        private int _zombiesSpawned;
        private const int MaxZombies = 20;

        public Field()
        {
            _window = new Window("Plants vs Zombies", 1600, 900);
            _grid = new Cell[Rows, Cols];
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    _grid[r, c] = new Cell(r, c, c * CellWidth, r * CellHeight + 150, CellWidth, CellHeight);

            _peas = new List<Pea>();
            _zombies = new List<Zombie>();
            _suns = new List<Sun>();
            _rand = new Random();
            _sunCount = 150;
            _selectedPlant = PlantType.PeaShooter;
            _zombiesSpawned = 0;
        }

        public void Run()
        {
            while (!_window.CloseRequested)
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen(Color.LawnGreen);

                DrawGrid();
                DrawUI();
                SpawnZombies();
                UpdateZombies();
                UpdatePlants();
                UpdatePeas();
                UpdateSuns();
                CheckCollisions();
                CheckGameOver();
                HandleInput();

                _window.Refresh(60);
            }
        }

        private void DrawGrid()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    _grid[r, c].Draw();
        }

        private void DrawUI()
        {
            SplashKit.FillRectangle(Color.SaddleBrown, 0, 0, 1600, 150);
            Bitmap psCard = SplashKit.LoadBitmap("peashooterCard", "Resources/Images/peashooterCard.png");
            Bitmap sCard = SplashKit.LoadBitmap("sunflowerCard", "Resources/Images/sunflowerCard.png");
            Bitmap sun = SplashKit.LoadBitmap("Sun", "Resources/Images/sun.png");
            Bitmap wCard = SplashKit.LoadBitmap("walnutCard", "Resources/Images/walnutCard.png");

            SplashKit.FillRectangle(Color.SeaGreen, 30, 10, 80, 130);
            SplashKit.DrawBitmap(sun, 24, 10);
            SplashKit.DrawText($"{_sunCount}", Color.LightGoldenrodYellow, 65, 120);

            DrawingOptions opts = SplashKit.OptionScaleBmp(1.6f, 1.71f);
            SplashKit.DrawBitmap(psCard, 200, 40, opts);
            SplashKit.DrawBitmap(sCard, 320, 40, opts);
            SplashKit.DrawBitmap(wCard, 440, 40, opts);
        }

        private void SpawnZombies()
        {   
            if (_zombieSpawnCooldown <= 0)
            {
                int row = _rand.Next(Rows);
                _zombies.Add(new Zombie(1600, row * CellHeight + 150 + 10));
                _zombiesSpawned++;
                _zombieSpawnCooldown = 600;
            }
            else _zombieSpawnCooldown--;
        }

        private void UpdateZombies()
        {
            foreach (var z in _zombies)
            {
                bool isBlocked = false;
                foreach (var cell in _grid)
                {
                    Plant plant = cell.PlantInCell;
                    if (plant != null && plant.IsAlive && SplashKit.RectanglesIntersect(z.Rectangle,
                        SplashKit.RectangleFrom(plant.X, plant.Y, plant.Width, plant.Height)))
                    {
                        isBlocked = true;
                        plant.TakeDamage(1); // Zombies attack plant
                        break;
                    }
                }

                if (!isBlocked)
                    z.Update();
                z.Draw();
            }
        }

        private void UpdatePlants()
        {
            foreach (var cell in _grid)
            {
                var plant = cell.PlantInCell;
                if (plant != null && plant.IsAlive)
                {
                    plant.Update(_peas, _suns);
                }
                else
                {
                    cell.PlantInCell = null; // Remove dead plant
                }
            }
        }

        private void UpdatePeas()
        {
            foreach (var p in _peas) p.Update();
            foreach (var p in _peas) p.Draw();
            _peas.RemoveAll(p => !p.IsAlive);
        }

        private void UpdateSuns()
        {
            foreach (var s in _suns) s.Update();
            foreach (var s in _suns) s.Draw();
        }

        private void CheckCollisions()
        {
            foreach (var p in _peas)
            {
                foreach (var z in _zombies)
                {
                    if (z.IsAlive && SplashKit.PointInRectangle(new Point2D { X = p.X, Y = p.Y }, z.Rectangle))
                    {
                        z.Health -= 20;
                        p.IsAlive = false;
                        break;
                    }
                }
            }

            _zombies.RemoveAll(z => !z.IsAlive);
        }

        private void CheckGameOver()
        {
            foreach (var z in _zombies)
            {
                if (z.X < 0)
                {
                    Console.WriteLine("Game Over");
                    _window.Refresh(60);
                    Environment.Exit(0);
                }
            }

            // Win condition: all zombies have been spawned AND all are dead
            if (_zombiesSpawned >= MaxZombies && _zombies.Count == 0)
            {
                Console.WriteLine("You Win!");
                _window.Refresh(60);
                Environment.Exit(0);
            }
        }

        private void HandleInput()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D clickPos = SplashKit.MousePosition();
                float mouseX = SplashKit.MouseX();
                float mouseY = SplashKit.MouseY();

                //plant chose
                if (mouseX >= 200 && mouseX <= 280)
                {
                    if (mouseY >= 40 && mouseY <= 160)
                    {
                        _selectedPlant = PlantType.PeaShooter;
                    }
                }
                else if (mouseX >= 320 && mouseX <= 400)
                {
                    if (mouseY >= 40 && mouseY <= 160)
                    {
                        _selectedPlant = PlantType.Sunflower;
                    }
                }
                else if (mouseX >= 440 && mouseX <= 520)
                {
                    if (mouseY >= 40 && mouseY <= 160)
                    {
                        _selectedPlant = PlantType.Walnut;
                    }    
                }

                // Try collecting suns
                foreach (var sun in _suns)
                {
                    if (!sun.IsCollected && sun.IsClicked(clickPos))
                    {
                        sun.IsCollected = true;
                        _sunCount += 50;
                    }
                }

                _suns.RemoveAll(s => s.IsCollected);

                // Try planting
                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Cols; c++)
                    {
                        var cell = _grid[r, c];
                        if (cell.Contains(clickPos) && cell.PlantInCell == null)
                        {
                            int cost = _selectedPlant == PlantType.PeaShooter ? 100 :
                                       _selectedPlant == PlantType.Sunflower ? 50 : 50;
                            if (_sunCount >= cost)
                            {
                                if (_selectedPlant == PlantType.PeaShooter)
                                {
                                    cell.PlantInCell = new PeaShooter(cell.X, cell.Y);
                                    _sunCount -= cost;
                                }
                                else if (_selectedPlant == PlantType.Sunflower)
                                {
                                    cell.PlantInCell = new Sunflower(cell.X, cell.Y);
                                    _sunCount -= cost;
                                }
                                else if (_selectedPlant == PlantType.Walnut)
                                {
                                    cell.PlantInCell = new WalnutWall(cell.X, cell.Y);
                                    _sunCount -= cost;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
