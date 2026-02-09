using System.Windows;
using System.Windows.Controls;

namespace Puzzle15
{
    public partial class MainWindow : Window
    {
        private readonly PuzzleBoard _puzzle;
        private readonly Button[] _tiles;
        private int _steps;

        public MainWindow()
        {
            InitializeComponent();
            _puzzle = new PuzzleBoard();
            _tiles = new Button[16];
            _steps = 0;
            BuildTiles();
            ResetGame();
        }

        private void BuildTiles()
        {
            for (int i = 0; i < 16; i++)
            {
                Button btn = new Button
                {
                    Width = 70,
                    Height = 70,
                    Margin = new Thickness(3),
                    Style = (Style)Application.Current.Resources["TileButtonStyle"],
                    Tag = i
                };
                btn.Click += OnTileClicked;
                PuzzleGrid.Children.Add(btn);
                _tiles[i] = btn;
            }
        }

        private void RefreshBoard()
        {
            for (int i = 0; i < 16; i++)
            {
                int row = i / 4;
                int col = i % 4;
                int value = _puzzle.GetValue(row, col);
                Button btn = _tiles[i];

                if (value == PuzzleBoard.EmptyTileValue)
                {
                    btn.Content = "";
                    btn.IsEnabled = false;
                }
                else
                {
                    btn.Content = value.ToString();
                    btn.IsEnabled = true;
                }
            }

            MoveCounterText.Text = $"Tahy: {_steps}";
        }

        private void OnTileClicked(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = (Button)sender;
            int index = (int)clickedBtn.Tag;
            int row = index / 4;
            int col = index % 4;

            if (_puzzle.MoveTile(row, col))
            {
                _steps++;
                RefreshBoard();

                if (_puzzle.IsSolved())
                {
                    DisplayVictory();
                }
            }
        }

        private void OnNewGame(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ResetGame()
        {
            _puzzle.Shuffle(1000);
            _steps = 0;
            RefreshBoard();
        }

        private void DisplayVictory()
        {
            string message = $"Gratulace!\n\nVyresili jste hlavolam na {_steps} tahu!\n\nKliknete OK pro pokracovani.";

            MessageBox.Show(
                message,
                "Vitezstvi!",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}
