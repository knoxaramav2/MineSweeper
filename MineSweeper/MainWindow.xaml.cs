
using System.Windows;
using System.Windows.Controls;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameProcessor _processor;
        public EndGameDialog EndGame;

        public MainWindow()
        {
            InitializeComponent();

            Left = (SystemParameters.PrimaryScreenWidth / 2) - (this.Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowNewGameModal();
        }

        bool ShowNewGameModal()
        {
            var dialog = new NewGameWindow();
            dialog.Left = (SystemParameters.PrimaryScreenWidth / 2) - (dialog.Width / 2);
            dialog.Top = (SystemParameters.PrimaryScreenHeight / 2) - (dialog.Height / 2);
            dialog.NewGame += NewGame;

            return dialog.ShowDialog()??false;
        }

        private void NewGame(int width, int height)
        {
            _processor = new GameProcessor(width, height, ref MainGrid, MineCounterDisplay);
            _processor.Init();
        }

        public void EndGame()
        {

        }
    }
}
