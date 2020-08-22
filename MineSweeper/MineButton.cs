using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MineSweeper
{
    public class MineButton : Button
    {
        public ushort X, Y;
        public LeftClick LeftClick;
        public RightClick RightClick;
        public int Value;

        public static int FlagCounter = 0;

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
        public State state { get; private set; }

        public MineButton(ushort x, ushort y)
        {
            X = x;
            Y = y;

            IsBomb = false;
            Content = " ";
            SetState(State.Hidden);

            Value = 0;
        }

        public void SetState(State state)
        {
            this.state = state;
            Foreground = Brushes.White;

            switch (state)
            {
                case State.Empty:
                    Background = Brushes.DarkGray;
                    Content = " ";
                    break;
                case State.Hidden:
                    Background = Brushes.Gray;
                    Content = " ";
                    break;
                case State.Flagged:
                    Background = Brushes.Yellow;
                    Content = "!";
                    break;
                case State.Question:
                    Background = Brushes.MediumPurple;
                    Content = "?";
                    break;
                case State.Number:
                    Background = Brushes.DarkGray;
                    Content = $"{Value}";
                    break;
                case State.Exploded:
                    Background = Brushes.DarkRed;
                    Content = "X";
                    break;
            }
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
                    ++FlagCounter;
                    Content = "!";
                    Foreground = Brushes.Red;
                    break;
                case State.Flagged:
                    state = State.Question;
                    --FlagCounter;
                    Content = "?";
                    Foreground = Brushes.Cyan;
                    break;
                case State.Question:
                    state = State.Hidden;
                    Content = " ";
                    Foreground = Brushes.White;
                    break;
            }
            RightClick(this);
        }
    }
}
