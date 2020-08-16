using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MineSweeper
{
    public class MineButton : Button
    {
        public ushort X, Y;
        public LeftClick LeftClick;

        public enum State
        {
            Hidden,
            Flagged,
            Question,

            Empty,
            Number,
            Exploded
        }

        public bool IsBomb;
        public State state;

        public MineButton(ushort x, ushort y)
        {
            X = x;
            Y = y;

            IsBomb = false;
            state = State.Hidden;
            Content = " ";
        }

        public void OnLeftClick(object sender, System.Windows.RoutedEventArgs e)
        {
            LeftClick(this);
        }

        public void OnRightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (state == State.Empty ||
                state == State.Number) return;

            switch (state)
            {
                case State.Hidden:
                    state = State.Flagged;
                    Content = "!";
                    break;
                case State.Flagged:
                    state = State.Question;
                    Content = "?";
                    break;
                case State.Question:
                    state = State.Hidden;
                    Content = " ";
                    break;
            }
            //RightClick(this);
        }
    }
}
