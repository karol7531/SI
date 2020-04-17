using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Connect_4;

namespace Connect_4_gui
{
    public partial class MainWindow : Window
    {
        const int depth = 7,
            rows = 6,
            cols = 7;
        bool playerMove = false;
        const MethodType methodType = MethodType.AlphaBeta;
        public MainWindow()
        {
            InitializeComponent();
            InitPanels();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            AiEngine minMax = new AiEngine(depth);
            long time = RunWithStopwatch(() =>
            {
                //PlayerVsAi(minMax);
                new Thread(()=> {
                    Program.AiVsAi(minMax, 7, (state) => RenderBoard(state), rows, cols, methodType);
                }).Start();
                
            });
            Console.WriteLine($"Game time: {time}");
        }

        public void InitPanels()
        {
            CreateRows();
            CreateColumns();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Border panel = CreatePanel(r, c);
                    BoardGrid.Children.Add(panel);
                }
            }
        }

        private void RenderBoard(State state)
        {
            Application.Current.Dispatcher.Invoke(
            () =>
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        ChangePanel(GetPosition(r, c), r, c, state.GetValue(r, c));
                    }
                }
            });
            
        }

        private int GetPosition(int row, int col)
        {
            return BoardGrid.Children.IndexOf(GetPannel(row, col));
        }

        private Border GetPannel(int row, int col)
        {
            return BoardGrid.Children
                .Cast<Border>()
                .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
        }

        private void CreateColumns()
        {
            BoardGrid.ColumnDefinitions.Clear();
            for (int c = 0; c < cols; c++)
                BoardGrid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = GridLength.Auto });
        }

        private void CreateRows()
        {
            BoardGrid.RowDefinitions.Clear();
            for (int r = 0; r < rows; r++)
                BoardGrid.RowDefinitions.Add(
                    new RowDefinition() { Height = GridLength.Auto });
        }

        private Border CreatePanel(int rowNum, int colNum, bool? player = null)
        {
            Border panel = new Border();
            Grid.SetRow(panel, rowNum);
            Grid.SetColumn(panel, colNum);

            panel.Child = GetCoin(player);
            panel.Background = new SolidColorBrush(BackgroundColor);
            panel.MouseLeftButtonUp += ColPanelClickHandler;
            return panel;
        }

        private void ColPanelClickHandler(object sender, MouseButtonEventArgs e)
        {
            Border panel = sender as Border;
            int col = Grid.GetColumn(panel);
            int row = Grid.GetRow(panel); //calculate row

            int pos = BoardGrid.Children.IndexOf(panel);
            //ChangePanel(pos, row, col, true);
        }

        private void ChangePanel(int pos, int row, int col, bool? player)
        {
            BoardGrid.Children.RemoveAt(pos);
            BoardGrid.Children.Add(CreatePanel(row, col, player));
        }

        private Border GetCoin(bool? player)
        {
            Border border = new Border();
            border.Margin = new Thickness(5);

            Ellipse coin = new Ellipse();
            SolidColorBrush scb = new SolidColorBrush();
            scb.Color = player == null ? NullColor : player == true ? TrueColor : FalseColor;
            coin.Fill = scb;
            coin.Width = 70;
            coin.Height = 70;

            border.Child = coin;
            return border;
        }

        private static long RunWithStopwatch(Action action)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private readonly Color NullColor = Color.FromArgb(255, 255, 255, 255);
        private readonly Color TrueColor = Color.FromArgb(255, 255, 69, 0);
        private readonly Color FalseColor = Color.FromArgb(255, 255, 215, 0);
        private readonly Color BackgroundColor = Color.FromArgb(255, 0, 0, 100);
    }
}
