using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game2048
{
    public partial class MainWindow : Window
    {
        private const int BoardSize = 4;
        private const int CellSize = 100;

        private int[,] board;
        private List<Grid> tiles;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            DrawBoard();
            GenerateNewTile();
        }

        private void InitializeBoard()
        {
            board = new int[BoardSize, BoardSize];
            tiles = new List<Grid>();
        }

        private void DrawBoard()
        {
            BoardCanvas.Children.Clear();
            tiles.Clear();

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Grid tile = new Grid
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Background = Brushes.LightGray,
                        
                    };

                    BoardCanvas.Children.Add(tile);
                    Canvas.SetLeft(tile, j * CellSize);
                    Canvas.SetTop(tile, i * CellSize);

                    tiles.Add(tile);
                }
            }
        }

        private void GenerateNewTile()
        {
            List<int> availableTiles = new List<int>();

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (board[i, j] == 0)
                    {
                        availableTiles.Add(i * BoardSize + j);
                    }
                }
            }

            if (availableTiles.Count > 0)
            {
                Random random = new Random();
                int index = availableTiles[random.Next(availableTiles.Count)];
                int value = random.Next(10) == 0 ? 4 : 2;

                int row = index / BoardSize;
                int col = index % BoardSize;

                board[row, col] = value;
                UpdateTile(row, col);
            }
        }

        private void UpdateTile(int row, int col)
        {
            int value = board[row, col];
            Grid tile = tiles[row * BoardSize + col];

            Rectangle rect = new Rectangle
            {
                Width = CellSize,
                Height = CellSize,
                Fill = GetTileColor(value),
                Stroke = Brushes.Black,
                RadiusX = 10,
                RadiusY = 10
            };

            TextBlock textBlock = new TextBlock
            {
                Text = value > 0 ? value.ToString() : string.Empty,
                FontSize = value < 1024 ? 36 : 24,
                FontWeight = value < 1024 ? FontWeights.Bold : FontWeights.Normal,
                Foreground = value < 8 ? Brushes.Gray : Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            tile.Children.Clear();
            tile.Children.Add(rect);
            tile.Children.Add(textBlock);
        }


        private Brush GetTileColor(int value)
        {
            switch (value)
            {
                case 2:
                    return Brushes.LightYellow;
                case 4:
                    return Brushes.LightGoldenrodYellow;
                case 8:
                    return Brushes.LightSalmon;
                case 16:
                    return Brushes.LightCoral;
                case 32:
                    return Brushes.LightPink;
                case 64:
                    return Brushes.LightBlue;
                case 128:
                    return Brushes.LightSkyBlue;
                case 256:
                    return Brushes.LightGreen;
                case 512:
                    return Brushes.LightSeaGreen;
                case 1024:
                    return Brushes.LightSteelBlue;
                case 2048:
                    return Brushes.LightSlateGray;
                default:
                    return Brushes.White;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;

            switch (e.Key)
            {
                case Key.Up:
                    moved = MoveTilesUp();
                    break;
                case Key.Down:
                    moved = MoveTilesDown();
                    break;
                case Key.Left:
                    moved = MoveTilesLeft();
                    break;
                case Key.Right:
                    moved = MoveTilesRight();
                    break;
            }

            
                CheckGameWon();
                CheckGameOver();
                GenerateNewTile();
        }

        private bool MoveTilesUp()
        {
            bool moved = false;

            for (int j = 0; j < BoardSize; j++)
            {
                int i = 1;
                while (i < BoardSize)
                {
                    if (board[i, j] != 0)
                    {
                        int row = i;
                        while (row > 0 && board[row - 1, j] == 0)
                        {
                            board[row - 1, j] = board[row, j];
                            board[row, j] = 0;
                            row--;
                            moved = true;
                        }

                        if (row > 0 && board[row - 1, j] == board[row, j])
                        {
                            board[row - 1, j] *= 2;
                            board[row, j] = 0;
                            moved = true;
                        }
                    }

                    i++;
                }
            }

            UpdateTiles();
            return moved;
        }

        private bool MoveTilesDown()
        {
            bool moved = false;

            for (int j = 0; j < BoardSize; j++)
            {
                int i = BoardSize - 2;
                while (i >= 0)
                {
                    if (board[i, j] != 0)
                    {
                        int row = i;
                        while (row < BoardSize - 1 && board[row + 1, j] == 0)
                        {
                            board[row + 1, j] = board[row, j];
                            board[row, j] = 0;
                            row++;
                            moved = true;
                        }

                        if (row < BoardSize - 1 && board[row + 1, j] == board[row, j])
                        {
                            board[row + 1, j] *= 2;
                            board[row, j] = 0;
                            moved = true;
                        }
                    }

                    i--;
                }
            }

            UpdateTiles();
            return moved;
        }

        private bool MoveTilesLeft()
        {
            bool moved = false;


            for (int i = 0; i < BoardSize; i++)
            {
                int j = 1;
                while (j < BoardSize)
                {
                    if (board[i, j] != 0)
                    {
                        int col = j;
                        while (col > 0 && board[i, col - 1] == 0)
                        {
                            board[i, col - 1] = board[i, col];
                            board[i, col] = 0;
                            col--;
                            moved = true;
                        }

                        if (col > 0 && board[i, col - 1] == board[i, col])
                        {
                            board[i, col - 1] *= 2;
                            board[i, col] = 0;
                            moved = true;
                        }
                    }

                    j++;
                }
            }

            UpdateTiles();
            return moved;
        }

        private bool MoveTilesRight()
        {
            bool moved = false;

            for (int i = 0; i < BoardSize; i++)
            {
                int j = BoardSize - 2;
                while (j >= 0)
                {
                    if (board[i, j] != 0)
                    {
                        int col = j;
                        while (col < BoardSize - 1 && board[i, col + 1] == 0)
                        {
                            board[i, col + 1] = board[i, col];
                            board[i, col] = 0;
                            col++;
                            moved = true;
                        }

                        if (col < BoardSize - 1 && board[i, col + 1] == board[i, col])
                        {
                            board[i, col + 1] *= 2;
                            board[i, col] = 0;
                            moved = true;
                        }
                    }

                    j--;
                }
            }

            UpdateTiles();
            return moved;
        }

        private void UpdateTiles()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    UpdateTile(i, j);
                }
            }
        }

        private bool IsGameOver()
        {
            // Проверка, есть ли пустые ячейки
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (board[i, j] == 0)
                    {
                        return false; 
                    }
                }
            }

            // Проверка, есть ли соседние ячейки с одинаковыми значениями
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    int value = board[i, j];

                    if ((i < BoardSize - 1 && board[i + 1, j] == value) ||
                        (j < BoardSize - 1 && board[i, j + 1] == value))
                    {
                        return false; 
                    }
                }
            }

            return true; 
        }

        private bool IsGameWon()
        {
            
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (board[i, j] == 2048)
                    {
                        return true; 
                    }
                }
            }

            return false; 
        }

        private void CheckGameOver()
        {
            if (IsGameOver())
            {
                
                MessageBox.Show("Игра окончена. Вы не можете выполнить ходы.");
              
            }
        }

        private void CheckGameWon()
        {
            if (IsGameWon())
            {
                
                MessageBox.Show("Поздравляем! Вы достигли числа 2048 и выиграли игру!");
            }
        }

        private void restart_button_Click(object sender, RoutedEventArgs e)
        {
            InitializeBoard();
            DrawBoard();
            GenerateNewTile();
        }
    }
}