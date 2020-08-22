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

        public EndGameDialog EndGameEvent;

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
                //update numeric values
                for(var i = -1; i <= 1; ++i)
                {
                    if (xLoc + i < 0 || xLoc + i >= _width) continue;
                    for (var j=-1; j<=1; ++j)
                    {
                        if (yLoc + j < 0 || yLoc + j >= _height) continue;
                        ++grid[xLoc+i][yLoc+j].Value;
                    }
                }

                --mineCounter;
            }

            SetRemainingMines();

            //Test view
            for(int i=0; i < _width && false; ++i)
            {
                for (int j =0; j < _height; ++j)
                {
                    var btn = grid[i][j];
                    if (btn.IsBomb)
                    {
                        btn.Content = "B";
                        //btn.Content = btn.Value.ToString();
                    } else
                    {
                        btn.Content = btn.Value.ToString();
                    }

                }
            }
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
                if(btn.Value == 0)
                {
                    btn.SetState(MineButton.State.Empty);
                } else
                {
                    btn.SetState(MineButton.State.Number);
                }

                ClearOpenCells(btn.X, btn.Y);
            }

            SetRemainingMines();
        }

        public void MineRightClick(MineButton btn)
        {
            SetRemainingMines();
        }

        private void ClearOpenCells(int x, int y)
        {
            var queue = new List<MineButton>();
            queue.Add(grid[x][y]);

            ClearOpenRec(x - 1, y, queue);
            ClearOpenRec(x + 1, y, queue);
            ClearOpenRec(x, y - 1, queue);
            ClearOpenRec(x, y + 1, queue);
        }

        private void ClearOpenRec(int x, int y, List<MineButton> queue)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height) return;
            if (grid[x][y].IsBomb || grid[x][y].state != MineButton.State.Hidden) return;

            var btn = grid[x][y];

            if (btn.Value > 0)
            {
                btn.SetState(MineButton.State.Number);
                return;
            }
            else btn.SetState(MineButton.State.Empty);

            ClearOpenRec(x - 1, y, queue);
            ClearOpenRec(x + 1, y, queue);
            ClearOpenRec(x, y - 1, queue);
            ClearOpenRec(x, y + 1, queue);

        }
        private void EndGame(bool gameWon)
        {
            EndGameEvent();
        }
    
        private void SetRemainingMines()
        {
            var remainder = MineCount - MineButton.FlagCounter;
            _counterLabel.Content = remainder.ToString();
        }
    }
}
