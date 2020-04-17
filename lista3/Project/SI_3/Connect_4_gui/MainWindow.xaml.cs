using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Connect_4_gui
{
    public partial class MainWindow : Window
    {
        const int depth = 7,
            rows = 6,
            cols = 7;
        public MainWindow()
        {
            InitializeComponent();
            InitPanels();
        }

        public void InitPanels()
        {
            CreateRows();
            CreateColumns();
            int c = 0;
            foreach (ColumnDefinition col in BoardGrid.ColumnDefinitions)
            {
                int r = 0;
                foreach (RowDefinition row in BoardGrid.RowDefinitions)
                {
                    Border panel = GetPanel(r, c);
                    int id = BoardGrid.Children.Add(panel);

                    r++;
                }

                c++;
            }
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

        private Border GetPanel(int rowNum, int colNum, bool? player = null)
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
            BoardGrid.Children.RemoveAt(pos);
            BoardGrid.Children.Add(GetPanel(row, col, true));
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

        private readonly Color NullColor = Color.FromArgb(255, 255, 255, 255);
        private readonly Color TrueColor = Color.FromArgb(255, 255, 69, 0);
        private readonly Color FalseColor = Color.FromArgb(255, 255, 215, 0);
        private readonly Color BackgroundColor = Color.FromArgb(255, 0, 0, 100);
    }
}
