using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Connect_4;

namespace Connect_4_gui
{
    enum Gamemode {PlayerVsAi,AiVsAi}
    public partial class MainWindow : Window
    {

        const int rows = 6,
            cols = 7;
        int depth = 7;
        int aiStartPos = 6;
        MethodType methodType = MethodType.AlphaBeta;
        Gamemode gamemode = Gamemode.PlayerVsAi;
        HeuristicType heuristicType = HeuristicType.NoCornersSquare;

        bool playerMove = false;
        AiEngine aiEngine;
        Program program;
        private State playerAiState = new State(rows, cols);
        BackgroundWorker worker;

        private readonly Color NullColor = Color.FromArgb(255, 255, 255, 255);
        private readonly Color TrueColor = Color.FromArgb(255, 255, 69, 0);
        private readonly Color FalseColor = Color.FromArgb(255, 255, 215, 0);
        private readonly Color BackgroundColor = Color.FromArgb(255, 0, 0, 100);

        public MainWindow()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            aiEngine = new AiEngine(depth);
            program = new Program();
            InitPanels();
            GamemodeButton.Content = gamemode == Gamemode.PlayerVsAi ? "Player vs Ai" : "Ai vs Ai";
            MethodButton.Content = methodType == MethodType.MinMax ? MethodType.MinMax.ToString() : MethodType.AlphaBeta.ToString();
            AiStartDockPanel.Visibility = gamemode == Gamemode.PlayerVsAi ? Visibility.Hidden : Visibility.Visible;
            AiStart.Visibility = gamemode == Gamemode.PlayerVsAi ? Visibility.Hidden : Visibility.Visible;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            ResetButton_Click(null, null);
        }

        private void RunPlayerVsAi()
        {
            playerAiState = new State(rows, cols);
            RenderBoard(playerAiState);
            SetWhosMove("Your move:");
            playerMove = true;
        }

        private long RunAiVsAi()
        {
            long time = RunWithStopwatch(() =>
            {
                RunOnWorker((w) => AiVsAi(aiStartPos, w));
            });
            return time;
        }

        private void RunOnWorker(Action<BackgroundWorker> action)
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += (object sender, DoWorkEventArgs e) => action(worker);
            worker.RunWorkerAsync();
        }

        private void SetWhosMove(string text)
        {
            Application.Current.Dispatcher.Invoke(
            () =>
            {
                WhosMove.Text = text;
            });
        }

        private State PlayerMove(int col, State state)
        {
            if(playerMove && state.CanPlace(col))
            {
                return state.NextState(false, col);
            }
            return null;
        }

        private bool? AiVsAi(int start, BackgroundWorker w)
        {
            State state = new State(rows, cols);
            SetWhosMove("AI_1 move:");
            state = state.NextState(false, start - 1);
            RenderBoard(state);
            while (state.CanPlace())
            {                
                SetWhosMove("AI_2 move:");
                state = program.AiMove(aiEngine, state, true, methodType, heuristicType);
                if (w.CancellationPending){break;}
                RenderBoard(state);
                if (state.Points(true, heuristicType) >= State.pointsWin)
                {
                    SetWhosMove("AI_2 won");
                    return true;
                }

                SetWhosMove("AI_1 move:");
                state = program.AiMove(aiEngine, state, false, methodType, heuristicType);
                if (w.CancellationPending) {break; }
                RenderBoard(state);
                if (state.Points(false, heuristicType) >= State.pointsWin)
                {
                    SetWhosMove("AI_1 won");
                    return false;
                }
            }
            return null;
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
            if (playerMove)
            {
                Border panel = sender as Border;
                int col = Grid.GetColumn(panel);
                State playerState = PlayerMove(col, playerAiState);
                if (playerState != null)
                {
                    playerMove = false;
                    playerAiState = playerState;
                    RenderBoard(playerAiState);
                    if (playerAiState.Points(false, heuristicType) >= State.pointsWin)
                    {
                        SetWhosMove("Congratulations, you won");
                        playerMove = false;
                        return;
                    }

                    SetWhosMove("Ai move:");
                    RunOnWorker((w) =>
                    {
                        State newState = program.AiMove(aiEngine, playerAiState, true, methodType, heuristicType);
                        if (w.CancellationPending) { return; }
                        else { playerAiState = newState; }
                        RenderBoard(playerAiState);
                        if (playerAiState.Points(true, heuristicType) >= State.pointsWin)
                        {
                            SetWhosMove("Ai won");
                            playerMove = false;
                            return;
                        }
                        playerMove = true;
                        SetWhosMove("Your move:");
                    });
                }
            }
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


        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
            SetupParameters();
            switch (gamemode)
            {
                case Gamemode.PlayerVsAi:
                    {
                        RunPlayerVsAi();
                        break;
                    }
                case Gamemode.AiVsAi:
                    {
                        RunAiVsAi();
                        break;
                    }
            }
        }

        private void SetupParameters()
        {
            gamemode = ((string)GamemodeButton.Content) == "Player vs Ai" ? Gamemode.PlayerVsAi : Gamemode.AiVsAi;
            methodType = ((string)MethodButton.Content) == MethodType.AlphaBeta.ToString() ? MethodType.AlphaBeta : MethodType.MinMax;
            depth = (int)DifficultySlider.Value;
            aiEngine = new AiEngine(depth);
            if (AiStartDockPanel.Visibility == Visibility.Visible) { aiStartPos = (int)AiStartSlider.Value; } 
        }

        private void GamemodeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Content = (string)button.Content == "Player vs Ai" ? "Ai vs Ai" : "Player vs Ai";
            AiStartDockPanel.Visibility = (string)button.Content == "Player vs Ai" ? Visibility.Hidden : Visibility.Visible;
            AiStart.Visibility = (string)button.Content == "Player vs Ai" ? Visibility.Hidden : Visibility.Visible;
        }

        private void MethodButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Content = (string)button.Content == MethodType.AlphaBeta.ToString() ? MethodType.MinMax.ToString() : MethodType.AlphaBeta.ToString();
        }

    }
}
