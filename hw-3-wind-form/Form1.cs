namespace hw_3_wind_form
{
    public partial class Form1 : Form
    {
        private Button[,] gameGrid;
        private bool isPlayerTurn;
        private bool gameEnded;
        private const string PLAYER_MARK = "X";
        private const string AI_MARK = "O";
        private const int BUTTON_FONT_SIZE = 50;

        public Form1()
        {
            InitializeComponent();
            SetupGame();
        }

        private void SetupGame()
        {
            gameGrid = new Button[3, 3]
            {
                { button1, button2, button3 },
                { button4, button5, button6 },
                { button7, button8, button9 }
            };

            InitializeUI();
            LockBoard();
        }

        private void InitializeUI()
        {
            button10.Click += OnStartGameClick;
            radioButton1.Checked = true;
            SetButtonFonts();
        }

        private void SetButtonFonts()
        {
            foreach (Button btn in gameGrid)
            {
                btn.Font = new Font(btn.Font.FontFamily, BUTTON_FONT_SIZE, FontStyle.Bold);
            }
        }

        private void OnStartGameClick(object sender, EventArgs e)
        {
            ResetGame();
            isPlayerTurn = !checkBox1.Checked;
            gameEnded = false;

            DisableSettings();

            if (!isPlayerTurn)
                PerformAIMove();
        }

        private void ResetGame()
        {
            foreach (Button btn in gameGrid)
            {
                btn.Text = string.Empty;
                btn.Enabled = true;
                btn.Click += OnPlayerMove;
            }
            gameEnded = false;
        }

        private void LockBoard()
        {
            foreach (Button btn in gameGrid)
            {
                btn.Enabled = false;
            }
        }

        private void DisableSettings()
        {
            checkBox1.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }

        private void EnableSettings()
        {
            checkBox1.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }

        private void OnPlayerMove(object sender, EventArgs e)
        {
            if (gameEnded) return;

            Button clickedButton = sender as Button;

            if (clickedButton != null && string.IsNullOrEmpty(clickedButton.Text))
            {
                clickedButton.Text = PLAYER_MARK;

                if (CheckWinCondition(PLAYER_MARK))
                {
                    EndGame("You WIN!");
                    return;
                }

                if (CheckForDraw())
                {
                    EndGame("It's a draw!");
                    return;
                }

                isPlayerTurn = false;
                PerformAIMove();
            }
        }

        private void PerformAIMove()
        {
            if (gameEnded) return;

            Button aiMove = radioButton1.Checked ? GetRandomMove() : GetOptimalMove();

            if (aiMove != null)
            {
                aiMove.Text = AI_MARK;

                if (CheckWinCondition(AI_MARK))
                {
                    EndGame("Computer Wins!");
                    return;
                }

                if (CheckForDraw())
                {
                    EndGame("It's a draw!");
                    return;
                }

                isPlayerTurn = true;
            }
        }

        private Button GetRandomMove()
        {
            var emptyCells = gameGrid.Cast<Button>().Where(btn => string.IsNullOrEmpty(btn.Text)).ToList();
            if (emptyCells.Any())
            {
                var random = new Random();
                return emptyCells[random.Next(emptyCells.Count)];
            }
            return null;
        }

        private Button GetOptimalMove()
        {
            foreach (Button btn in gameGrid)
            {
                if (string.IsNullOrEmpty(btn.Text))
                {
                    btn.Text = AI_MARK;
                    if (CheckWinCondition(AI_MARK))
                    {
                        btn.Text = string.Empty;
                        return btn;
                    }
                    btn.Text = string.Empty;

                    btn.Text = PLAYER_MARK;
                    if (CheckWinCondition(PLAYER_MARK))
                    {
                        btn.Text = string.Empty;
                        return btn;
                    }
                    btn.Text = string.Empty;
                }
            }

            return GetRandomMove();
        }

        private bool CheckWinCondition(string mark)
        {
            for (int i = 0; i < 3; i++)
            {
                if (IsLineMatch(mark, gameGrid[i, 0], gameGrid[i, 1], gameGrid[i, 2]) ||
                    IsLineMatch(mark, gameGrid[0, i], gameGrid[1, i], gameGrid[2, i]))
                {
                    return true;
                }
            }

            return IsLineMatch(mark, gameGrid[0, 0], gameGrid[1, 1], gameGrid[2, 2]) ||
                   IsLineMatch(mark, gameGrid[0, 2], gameGrid[1, 1], gameGrid[2, 0]);
        }

        private bool IsLineMatch(string mark, params Button[] buttons)
        {
            return buttons.All(btn => btn.Text == mark);
        }

        private bool CheckForDraw()
        {
            return gameGrid.Cast<Button>().All(btn => !string.IsNullOrEmpty(btn.Text));
        }

        private void EndGame(string message)
        {
            gameEnded = true;
            MessageBox.Show(message, "Game Over");
            LockBoard();
            EnableSettings();
        }
    }
}
