using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment2
{
    public partial class Game_form : Form
    {
        // Game state controlled by this form.
        private Game game;
        // currently selected dino (from a player's hand) waiting to be placed
        private Dino selectedDino = null;
        // Pen currently selected as the placement target.
        private Pen selectedPen = null;
        // Board displaying player one's pens.
        private PictureBox pictureBoxPlayer1Board;
        // Button that starts a new game.
        private Button buttonStart;
        // Button that rolls the placement die.
        private Button buttonRoll;
        // Picture box displaying player one's hand.
        private PictureBox pictureBoxPlayer1Hand;
        // Picture box displaying player two's hand.
        private PictureBox pictureBoxPlayer2Hand;
        // Board displaying player two's pens.
        private PictureBox pictureBoxPlayer2Board;
        // Label showing the current placement condition.
        private Label labelRollResult;
        // Label showing player one's name.
        private Label labelPlayer1;
        // Label showing the AI player's name.
        private Label labelAIplayer;
        // Label identifying the current turn.
        private Label labelTurn;
        // Label identifying the current round.
        private Label labelRound;
        // Label showing the turn counter.
        private Label labelTurnResult;
        // Label showing the round counter.
        private Label labelRoundResult;
        // Label describing the action expected from the user.
        private Label labelPlayerDo;
        // Button that advances the game to the next turn.
        private Button buttonNextTurn;
        // Index of the player whose hand supplied the selected dinosaur.
        private int selectedPlayerIndex = -1;
        // Clean copy of player one's board image, redrawn from each time so highlights can be removed.
        private Bitmap player1BoardBackground;
        // Clean copy of player two's board image.
        private Bitmap player2BoardBackground;

        /// <summary>
        /// Creates a new game, initializes its players, and clears the current selection.
        /// </summary>
        public void StartNewGame()
        {
            // Reset the game state
            game = new Game();
            // initialize players and other game data
            game.LoadGame(this, EventArgs.Empty);
            game.StartRound();
            selectedDino = null;
            selectedPen = null;
            selectedPlayerIndex = -1;
        }

        /// <summary>
        /// Refreshes the hands, boards, counters, and available actions shown in the form.
        /// </summary>
        public void DisplayGame()
        {
            // Show the game form
            this.Show();

            if (pictureBoxPlayer1Hand.Image == null)
                pictureBoxPlayer1Hand.Image = new Bitmap(pictureBoxPlayer1Hand.Width, pictureBoxPlayer1Hand.Height);
            if (pictureBoxPlayer2Hand.Image == null)
                pictureBoxPlayer2Hand.Image = new Bitmap(pictureBoxPlayer2Hand.Width, pictureBoxPlayer2Hand.Height);

            using (Graphics g1 = Graphics.FromImage(pictureBoxPlayer1Hand.Image))
            {
                g1.Clear(Color.Transparent);
                game.players[0].DrawDinoHand(g1, pictureBoxPlayer2Hand.Width);

                // Draw a green border around the currently selected dino
                if (selectedDino != null && game.players[0].DinoHandList.Contains(selectedDino))
                {
                    selectedDino.DrawSelection(g1);
                }
            }

            using (Graphics g2 = Graphics.FromImage(pictureBoxPlayer2Hand.Image))
            {
                g2.Clear(Color.Transparent);
                game.players[1].DrawDinoHand(g2, pictureBoxPlayer2Hand.Width);
            }

            // Update the player hand displays
            pictureBoxPlayer1Hand.Refresh();
            pictureBoxPlayer2Hand.Refresh();

            // Draw the boards for both players. Pens that are valid for the selected dino get a green border.
            List<Pen> validPens = game.GetValidPlayerPens(selectedDino);
            RedrawBoard(pictureBoxPlayer1Board, player1BoardBackground, game.players[0], validPens);
            RedrawBoard(pictureBoxPlayer2Board, player2BoardBackground, game.players[1], null);

            // Update the players boards
            pictureBoxPlayer2Board.Refresh();
            pictureBoxPlayer1Board.Refresh();


            labelTurnResult.Text = game.turnNumber.ToString() + " /6";
            labelRoundResult.Text = game.roundNumber.ToString() + " /4";
            buttonRoll.Enabled = game.RollerIndex == 0 && !game.HasRolled;
            UpdatePlayerActionLabel();
        }

        /// <summary>
        /// Redraws a board from its clean background so old highlights never remain on screen.
        /// </summary>
        /// <param name="box">Picture box showing the board.</param>
        /// <param name="background">Clean copy of the board image.</param>
        /// <param name="player">Player who owns the board.</param>
        /// <param name="highlighted">Pens to outline in green, or null for none.</param>
        private void RedrawBoard(PictureBox box, Bitmap background, Player player, List<Pen> highlighted)
        {
            Bitmap frame = new Bitmap(background);
            using (Graphics g = Graphics.FromImage(frame))
            {
                foreach (Pen pen in player.Zoo.PenList)
                {
                    pen.Draw(g); // Draw each pen at its predefined location
                    if (highlighted != null && highlighted.Contains(pen))
                    {
                        pen.DrawHighlight(g);
                    }
                }
                player.Zoo.DisplayRiver(g);
            }

            Image old = box.Image;
            box.Image = frame;
            if (old != null && old != background)
            {
                old.Dispose();
            }
        }

        /// <summary>
        /// Clears the selected dino (and pen) and redraws so the green borders disappear.
        /// </summary>
        private void ClearSelection()
        {
            if (selectedDino == null && selectedPen == null)
                return;

            selectedDino = null;
            selectedPen = null;
            selectedPlayerIndex = -1;

            if (game != null)
                DisplayGame();
        }

        /// <summary>
        /// Deselects the current dino when the player clicks anywhere else on the form.
        /// </summary>
        private void DeselectOnClick(object sender, EventArgs e)
        {
            ClearSelection();
        }

        /// <summary>
        /// Hooks the deselect click handler to every control except the ones that handle
        /// their own clicks (player one's hand and board).
        /// </summary>
        private void HookDeselectClicks(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c != pictureBoxPlayer1Hand && c != pictureBoxPlayer1Board)
                {
                    c.Click += DeselectOnClick;
                }
                HookDeselectClicks(c);
            }
        }

        /// <summary>
        /// Updates the action prompt to match the current game state.
        /// </summary>
        private void UpdatePlayerActionLabel()
        {
            if (game == null)
            {
                labelPlayerDo.Text = "Player 1: Start game";
            }
            else if (game.AwaitingDiscard)
            {
                labelPlayerDo.Text = "Player 1: Discard a dino";
            }
            else if (game.RollerIndex == 0 && !game.HasRolled)
            {
                labelPlayerDo.Text = "Player 1: Roll the dice";
            }
            else if (game.RollerIndex == 0 && game.HasRolled && game.RollerPlacementComplete)
            {
                labelPlayerDo.Text = "AI: Place a dino";
            }
            else
            {
                labelPlayerDo.Text = "Player 1: Place a dino";
            }
        }

        /// <summary>
        /// Selects the dinosaur under the mouse pointer in the human player's hand.
        /// </summary>
        /// <param name="sender">Hand control that received the click.</param>
        /// <param name="e">Mouse coordinates of the click.</param>
        private void pictureBoxPlayerHand_MouseClick(object sender, MouseEventArgs e)
        {
            if (game == null)
                return;

            PictureBox pb = (PictureBox)sender;

            // Clicking the opponent's hand is handled by the general deselect click handler
            if (pb != pictureBoxPlayer1Hand)
                return;

            // find which dino in the player's hand was clicked
            Dino clickedDino = null;
            foreach (Dino d in game.players[0].DinoHandList)
            {
                if (d.IsClicked(e.X, e.Y))
                {
                    clickedDino = d;
                    break;
                }
            }

            // Discard phase: the clicked dino goes back in the box
            if (game.AwaitingDiscard)
            {
                if (clickedDino != null && game.DiscardPlayerDino(clickedDino))
                {
                    selectedDino = null;
                    selectedPen = null;
                    selectedPlayerIndex = -1;
                    DisplayGame();
                }
                return;
            }

            if (!game.CanPlayerPlace)
            {
                ClearSelection();
                return;
            }

            // Clicking empty space in the hand deselects
            if (clickedDino == null)
            {
                ClearSelection();
                return;
            }

            // Clicking the selected dino again deselects it; clicking another dino switches to it
            if (clickedDino == selectedDino)
            {
                selectedDino = null;
                selectedPlayerIndex = -1;
            }
            else
            {
                selectedDino = clickedDino;
                selectedPlayerIndex = 0;
            }
            selectedPen = null;
            DisplayGame();
        }

        /// <summary>
        /// Places the selected dinosaur in the clicked pen or river location.
        /// </summary>
        /// <param name="sender">Board control that received the click.</param>
        /// <param name="e">Mouse coordinates of the click.</param>
        private void pictureBoxBoard_MouseClick(object sender, MouseEventArgs e)
        {
            if (game == null)
                return;

            // Clicking the opponent's board is handled by the general deselect click handler
            if (sender != pictureBoxPlayer1Board)
                return;

            if (selectedDino == null || selectedPlayerIndex != 0 || !game.CanPlayerPlace)
                return;

            // River is drawn at x 300-340, y 0-600
            if (e.X >= 300 && e.X <= 340 && e.Y >= 0 && e.Y <= 600)
            {
                if (game.PlacePlayerDinoRiver(selectedDino))
                {
                    selectedDino = null;
                    selectedPen = null;
                    selectedPlayerIndex = -1;
                    DisplayGame();
                }
                return;
            }

            Player p = game.players[0];

            // check which pen was clicked on the board
            foreach (Pen pen in p.Zoo.PenList)
            {
                if (e.X >= pen.XPos && e.X <= pen.XPos + pen.Width && e.Y >= pen.YPos && e.Y <= pen.YPos + pen.Height)
                {
                    selectedPen = pen;
                    if (game.PlacePlayerDino(selectedDino, selectedPen))
                    {
                        selectedDino = null;
                        selectedPen = null;
                        selectedPlayerIndex = -1;
                        DisplayGame();
                    }
                    else
                    {
                        selectedPen = null;
                        MessageBox.Show("That dino cannot be placed in this pen.");
                    }
                    return;
                }
            }

            // The click was on the board but not on a pen or the river, so deselect the dino
            ClearSelection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game_form"/> class.
        /// </summary>
        public Game_form()
        {
            InitializeComponent();

            // keep clean copies of the board images so the boards can be redrawn without old highlights
            player1BoardBackground = pictureBoxPlayer1Board.Image != null ? new Bitmap(pictureBoxPlayer1Board.Image) : new Bitmap(600, 600);
            player2BoardBackground = pictureBoxPlayer2Board.Image != null ? new Bitmap(pictureBoxPlayer2Board.Image) : new Bitmap(600, 600);

            // wire mouse event handlers for selecting dinos and placing them on the board
            pictureBoxPlayer1Hand.MouseClick += pictureBoxPlayerHand_MouseClick;
            pictureBoxPlayer2Hand.MouseClick += pictureBoxPlayerHand_MouseClick;
            pictureBoxPlayer1Board.MouseClick += pictureBoxBoard_MouseClick;
            pictureBoxPlayer2Board.MouseClick += pictureBoxBoard_MouseClick;
            // wire other control events
            this.buttonNextTurn.Click += new EventHandler(this.buttonNextTurn_Click);
            this.buttonStart.Click += new EventHandler(this.buttonStartGame_Click);
            this.buttonRoll.Click += new EventHandler(this.buttonRoll_Click);
            // clicking anywhere else on the form deselects the selected dino
            this.Click += DeselectOnClick;
            HookDeselectClicks(this);
        }

        /// <summary>
        /// Handles the event for next turn after player finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNextTurn_Click(object sender, EventArgs e)
        {
            if (game == null)
            {
                MessageBox.Show("Game not started");
                return;
            }

            // Both players must put a dino back in the box before the hands are passed on
            if (game.AwaitingDiscard)
            {
                MessageBox.Show("Click a dino in your hand to discard it before continuing.");
                return;
            }

            // Human rolled and placed: the AI places next. The human then discards.
            if (game.RollerIndex == 0 && game.HasRolled && game.RollerPlacementComplete && !game.TurnComplete)
            {
                if (!game.PlaceAiDino(false))
                {
                    MessageBox.Show("The AI could not find a valid pen for the current roll.");
                    return;
                }

                selectedDino = null;
                selectedPen = null;
                selectedPlayerIndex = -1;
                DisplayGame();
                return;
            }

            // Otherwise it is the AI's turn to roll and place
            if (!game.BeginAiTurn())
            {
                DisplayGame();
                return;
            }

            labelRollResult.Text = game.CurrentCondition.ToString();
            selectedDino = null;
            selectedPen = null;
            selectedPlayerIndex = -1;
            DisplayGame();
        }

        /// <summary>
        /// Rolls the dice for the next turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRoll_Click(object sender, EventArgs e)
        {
            if (game == null)
                return;

            if (game.RollerIndex != 0 || game.HasRolled)
                return;

            PlacementCondition condition = game.RollDice();
            labelRollResult.Text = condition.ToString();
            buttonRoll.Enabled = false;
            UpdatePlayerActionLabel();
        }

        /// <summary>
        /// Handles the click event for exiting the game. Closes application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Starts the game if the start button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartGame_Click(object sender, EventArgs e)
        {
            StartNewGame();
            DisplayGame();
        }

        /// <summary>
        /// Initializes the components of the game for the game class
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game_form));
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonRoll = new System.Windows.Forms.Button();
            this.labelRollResult = new System.Windows.Forms.Label();
            this.labelPlayer1 = new System.Windows.Forms.Label();
            this.labelAIplayer = new System.Windows.Forms.Label();
            this.labelTurn = new System.Windows.Forms.Label();
            this.labelRound = new System.Windows.Forms.Label();
            this.labelTurnResult = new System.Windows.Forms.Label();
            this.labelRoundResult = new System.Windows.Forms.Label();
            this.labelPlayerDo = new System.Windows.Forms.Label();
            this.buttonNextTurn = new System.Windows.Forms.Button();
            this.pictureBoxPlayer2Board = new System.Windows.Forms.PictureBox();
            this.pictureBoxPlayer2Hand = new System.Windows.Forms.PictureBox();
            this.pictureBoxPlayer1Hand = new System.Windows.Forms.PictureBox();
            this.pictureBoxPlayer1Board = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer2Board)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer2Hand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer1Hand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer1Board)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(693, 29);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(139, 60);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            // 
            // buttonRoll
            // 
            this.buttonRoll.Location = new System.Drawing.Point(693, 537);
            this.buttonRoll.Name = "buttonRoll";
            this.buttonRoll.Size = new System.Drawing.Size(139, 60);
            this.buttonRoll.TabIndex = 3;
            this.buttonRoll.Text = "Roll Dice";
            this.buttonRoll.UseVisualStyleBackColor = true;
            // 
            // labelRollResult
            // 
            this.labelRollResult.AutoSize = true;
            this.labelRollResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRollResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRollResult.Location = new System.Drawing.Point(702, 487);
            this.labelRollResult.Name = "labelRollResult";
            this.labelRollResult.Size = new System.Drawing.Size(120, 28);
            this.labelRollResult.TabIndex = 7;
            this.labelRollResult.Text = "Roll Result";
            // 
            // labelPlayer1
            // 
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1.Location = new System.Drawing.Point(12, 29);
            this.labelPlayer1.Name = "labelPlayer1";
            this.labelPlayer1.Size = new System.Drawing.Size(98, 26);
            this.labelPlayer1.TabIndex = 8;
            this.labelPlayer1.Text = "Player 1:";
            // 
            // labelAIplayer
            // 
            this.labelAIplayer.AutoSize = true;
            this.labelAIplayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAIplayer.Location = new System.Drawing.Point(866, 29);
            this.labelAIplayer.Name = "labelAIplayer";
            this.labelAIplayer.Size = new System.Drawing.Size(101, 26);
            this.labelAIplayer.TabIndex = 9;
            this.labelAIplayer.Text = "AI Player";
            // 
            // labelTurn
            // 
            this.labelTurn.AutoSize = true;
            this.labelTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTurn.Location = new System.Drawing.Point(688, 129);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(55, 26);
            this.labelTurn.TabIndex = 10;
            this.labelTurn.Text = "Turn";
            // 
            // labelRound
            // 
            this.labelRound.AutoSize = true;
            this.labelRound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRound.Location = new System.Drawing.Point(688, 182);
            this.labelRound.Name = "labelRound";
            this.labelRound.Size = new System.Drawing.Size(76, 26);
            this.labelRound.TabIndex = 11;
            this.labelRound.Text = "Round";
            // 
            // labelTurnResult
            // 
            this.labelTurnResult.AutoSize = true;
            this.labelTurnResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTurnResult.Location = new System.Drawing.Point(775, 129);
            this.labelTurnResult.Name = "labelTurnResult";
            this.labelTurnResult.Size = new System.Drawing.Size(70, 26);
            this.labelTurnResult.TabIndex = 12;
            this.labelTurnResult.Text = "label6";
            // 
            // labelRoundResult
            // 
            this.labelRoundResult.AutoSize = true;
            this.labelRoundResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRoundResult.Location = new System.Drawing.Point(775, 182);
            this.labelRoundResult.Name = "labelRoundResult";
            this.labelRoundResult.Size = new System.Drawing.Size(70, 26);
            this.labelRoundResult.TabIndex = 13;
            this.labelRoundResult.Text = "label7";
            // 
            // labelPlayerDo
            // 
            this.labelPlayerDo.AutoSize = true;
            this.labelPlayerDo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayerDo.Location = new System.Drawing.Point(688, 235);
            this.labelPlayerDo.Name = "labelPlayerDo";
            this.labelPlayerDo.Size = new System.Drawing.Size(150, 26);
            this.labelPlayerDo.TabIndex = 15;
            this.labelPlayerDo.Text = "Player 1: Start game";
            // 
            // buttonNextTurn
            // 
            this.buttonNextTurn.Location = new System.Drawing.Point(693, 685);
            this.buttonNextTurn.Name = "buttonNextTurn";
            this.buttonNextTurn.Size = new System.Drawing.Size(139, 60);
            this.buttonNextTurn.TabIndex = 14;
            this.buttonNextTurn.Text = "Next Turn";
            this.buttonNextTurn.UseVisualStyleBackColor = true;
            // 
            // pictureBoxPlayer2Board
            // 
            this.pictureBoxPlayer2Board.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBoxPlayer2Board.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPlayer2Board.Image")));
            this.pictureBoxPlayer2Board.InitialImage = global::Assignment2.Properties.Resources.Board_600_600;
            this.pictureBoxPlayer2Board.Location = new System.Drawing.Point(871, 68);
            this.pictureBoxPlayer2Board.Name = "pictureBoxPlayer2Board";
            this.pictureBoxPlayer2Board.Size = new System.Drawing.Size(600, 600);
            this.pictureBoxPlayer2Board.TabIndex = 6;
            this.pictureBoxPlayer2Board.TabStop = false;
            // 
            // pictureBoxPlayer2Hand
            // 
            this.pictureBoxPlayer2Hand.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBoxPlayer2Hand.Location = new System.Drawing.Point(871, 685);
            this.pictureBoxPlayer2Hand.Name = "pictureBoxPlayer2Hand";
            this.pictureBoxPlayer2Hand.Size = new System.Drawing.Size(600, 101);
            this.pictureBoxPlayer2Hand.TabIndex = 5;
            this.pictureBoxPlayer2Hand.TabStop = false;
            // 
            // pictureBoxPlayer1Hand
            // 
            this.pictureBoxPlayer1Hand.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBoxPlayer1Hand.Location = new System.Drawing.Point(16, 685);
            this.pictureBoxPlayer1Hand.Name = "pictureBoxPlayer1Hand";
            this.pictureBoxPlayer1Hand.Size = new System.Drawing.Size(600, 101);
            this.pictureBoxPlayer1Hand.TabIndex = 4;
            this.pictureBoxPlayer1Hand.TabStop = false;
            // 
            // pictureBoxPlayer1Board
            // 
            this.pictureBoxPlayer1Board.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBoxPlayer1Board.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPlayer1Board.Image")));
            this.pictureBoxPlayer1Board.InitialImage = global::Assignment2.Properties.Resources.Board_600_600;
            this.pictureBoxPlayer1Board.Location = new System.Drawing.Point(16, 68);
            this.pictureBoxPlayer1Board.Name = "pictureBoxPlayer1Board";
            this.pictureBoxPlayer1Board.Size = new System.Drawing.Size(600, 600);
            this.pictureBoxPlayer1Board.TabIndex = 0;
            this.pictureBoxPlayer1Board.TabStop = false;
            // 
            // Game_form
            // 
            this.ClientSize = new System.Drawing.Size(1552, 967);
            this.Controls.Add(this.buttonNextTurn);
            this.Controls.Add(this.labelRoundResult);
            this.Controls.Add(this.labelPlayerDo);
            this.Controls.Add(this.labelTurnResult);
            this.Controls.Add(this.labelRound);
            this.Controls.Add(this.labelTurn);
            this.Controls.Add(this.labelAIplayer);
            this.Controls.Add(this.labelPlayer1);
            this.Controls.Add(this.labelRollResult);
            this.Controls.Add(this.pictureBoxPlayer2Board);
            this.Controls.Add(this.pictureBoxPlayer2Hand);
            this.Controls.Add(this.pictureBoxPlayer1Hand);
            this.Controls.Add(this.buttonRoll);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.pictureBoxPlayer1Board);
            this.Name = "Game_form";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer2Board)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer2Hand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer1Hand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer1Board)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
