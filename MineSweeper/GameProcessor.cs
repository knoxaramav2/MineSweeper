using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MineSweeper
{
    public delegate void RightClick(MineButton btn);
    public delegate void LeftClick(MineButton btn);

    class GameProcessor
    {
        private MineButton[][] grid;
        private int _width, _height;

        public GameProcessor(int width, int height, ref Grid MainGrid)
        {
            grid = new MineButton[width][];
            _width = width;
            _height = height;

            for (var i = 0; i < width; ++i)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                grid[i] = new MineButton[height];
            }

            for (var i = 0; i < height; ++i)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }

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
                    
                    Grid.SetColumn(btn, j);
                    Grid.SetRow(btn, i);
                    MainGrid.Children.Add(btn);

                    grid[i][j] = btn;
                }
            }
        }
    
        public void Init()
        {

        }

        public void MineLeftClick(MineButton btn)
        {
            btn.Content = "-";
        }
    }
}
