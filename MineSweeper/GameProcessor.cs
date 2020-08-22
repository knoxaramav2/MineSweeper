using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MineSweeper
{
    public delegate void RightClick(MineButton btn);
    public delegate void LeftClick(MineButton btn);
    public delegate void EndGameDialog();

    class GameProcessor
    {
        private MineButton[][] grid;
        private int _width, _height;
        private Label _counterLabel;
        public int MineCount { get; private set; }

        public GameProcessor(int width, int height, ref Grid mainGrid, Label counterLabel)
        {
            grid = new MineButton[width][];
            _width = width;
            _height = height;

            _counterLabel = counterLabel;

            for (var i = 0; i < width; ++i)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                grid[i] = new MineButton[height];
            }

            for (var i = 0; i < height; ++i)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }

            GenerateMap(width, height, ref mainGrid);
            
        }

        private void GenerateMap(int width, int height, ref Grid mainGrid)
        {
            for (var i = 0; i < width; ++i)
            {
                for (var j = 0; j < height; ++j)
                {
                    var btn = new MineButton((ushort)i, (ushort)j)
                    {
                        Name = $"cell_{i}_{j}"
                    };

                    btn.Click += btn.OnLeftClick;
                    btn.MouseRightButtonDown += btn.OnRightClick;
                    btn.LeftClick = MineLeftClick;
                    btn.RightClick = MineRightClick;
                    btn.FontWeight = FontWeights.UltraBold;

                    Grid.SetColumn(btn, j);
                    Grid.SetRow(btn, i);
                    mainGrid.Children.Add(btn);

                    grid[i][j] = btn;
                }
            }

            var mineCounter = (int) (0.2249 * (_width * _height) - 11);
            MineCount = mineCounter;

            var rand = new Random((new DateTime()).Millisecond);

            while (mineCounter > 0)
            {
                var xLoc = rand.Next(0, _width);
                var yLoc = rand.Next(0, _height);

                if(grid[xLoc][yLoc].IsBomb){
                    continue;
                }

                grid[xLoc][yLoc].IsBomb = true;
                --mineCounter;
            }

            SetRemainingMines();
        }

        public void Init()
        {
            var matrixSize = _width * _height;


        }

        public void MineLeftClick(MineButton btn)
        {
            if (btn.state == MineButton.State.Empty ||
                btn.state == MineButton.State.Number) return;

            if (btn.IsBomb)
            {
                btn.SetState(MineButton.State.Exploded);
                EndGame(false);
            } else
            {
                btn.SetState(MineButton.State.Empty);
            }

            SetRemainingMines();
        }

        public void MineRightClick(MineButton btn)
        {
            SetRemainingMines();
        }

        private void EndGame(bool gameWon)
        {
            
        }
    
        private void SetRemainingMines()
        {
            var remainder = MineCount - MineButton.FlagCounter;
            _counterLabel.Content = remainder.ToString();
        }
    }
}
