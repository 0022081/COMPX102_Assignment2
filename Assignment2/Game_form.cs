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
        private Game game;
        // currently selected dino (from a player's hand) waiting to be placed
        private Dino selectedDino = null;
        private Pen selectedPen = null;
        private PictureBox pictureBoxPlayer1Board;
        private Button buttonStart;
        private Button buttonRoll;
        private PictureBox pictureBoxPlayer1Hand;
        private PictureBox pictureBoxPlayer2Hand;
        private PictureBox pictureBoxPlayer2Board;
        private Label labelRollResult;
        private Label labelPlayer1;
        private Label labelAIplayer;
        private Label labelTurn;
        private Label labelRound;
        private Label labelTurnResult;
        private Label labelRoundResult;
        private Button buttonNextTurn;
        private int selectedPlayerIndex = -1;

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
            }

            using (Graphics g2 = Graphics.FromImage(pictureBoxPlayer2Hand.Image))
            {
                g2.Clear(Color.Transparent);
                game.players[1].DrawDinoHand(g2, pictureBoxPlayer2Hand.Width);
            }

            // Update the player hand displays
            pictureBoxPlayer1Hand.Refresh();
            pictureBoxPlayer2Hand.Refresh();

            // Draw the boards for both players
            //pictureBoxPlayer1Board.Image = new Bitmap(Properties.Resources.Board);
            using (Graphics g = Graphics.FromImage(pictureBoxPlayer1Board.Image))
            {
                foreach (Pen pen in game.players[0].Zoo.PenList)
                {
                    pen.Draw(g); // Draw each pen at its predefined location
                }
            }

            //pictureBoxPlayer2Board.Image = new Bitmap(Properties.Resources.Board);
            using (Graphics g = Graphics.FromImage(pictureBoxPlayer2Board.Image))
            {
                foreach (Pen pen in game.players[1].Zoo.PenList)
                {
                    pen.Draw(g); // Draw each pen at its predefined location
                }
            }
            // Update the players boards
            pictureBoxPlayer2Board.Refresh();
            pictureBoxPlayer1Board.Refresh();


            labelTurnResult.Text = game.turnNumber.ToString() + " /6";
            labelRoundResult.Text = game.roundNumber.ToString() + " /4";
            buttonRoll.Enabled = game.RollerIndex == 0 && !game.HasRolled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxPlayerHand_MouseClick(object sender, MouseEventArgs e)
        {
            if (game == null)
                return;

            PictureBox pb = (PictureBox)sender;
            int playerIndex = pb == pictureBoxPlayer1Hand ? 0 : 1;

            if (playerIndex != 0 || !game.HasRolled)
                return;

            // find which dino in the player's hand was clicked
            foreach (Dino d in game.players[playerIndex].DinoHandList)
            {
                if (d.IsClicked(e.X, e.Y))
                {
                    selectedDino = d;
                    selectedPlayerIndex = playerIndex;
                    // optionally provide feedback to the user (e.g. redraw with selection)
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></pa>
        /// <param name="e"></param>
        private void pictureBoxBoard_MouseClick(object sender, MouseEventArgs e)
        {
            if (selectedDino == null || selectedPlayerIndex != 0 || !game.HasRolled)
                return;

            if (sender != pictureBoxPlayer1Board)
                return;

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
                        MessageBox.Show("That dino cannot be placed in this pen.");
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game_form"/> class.
        /// </summary>
        public Game_form()
        {
            InitializeComponent();
            // wire mouse event handlers for selecting dinos and placing them on the board
            pictureBoxPlayer1Hand.MouseClick += pictureBoxPlayerHand_MouseClick;
            pictureBoxPlayer2Hand.MouseClick += pictureBoxPlayerHand_MouseClick;
            pictureBoxPlayer1Board.MouseClick += pictureBoxBoard_MouseClick;
            pictureBoxPlayer2Board.MouseClick += pictureBoxBoard_MouseClick;
            // wire other control events
            this.buttonNextTurn.Click += new EventHandler(this.buttonNextTurn_Click);
            this.buttonStart.Click += new EventHandler(this.buttonStartGame_Click);
            this.buttonRoll.Click += new EventHandler(this.buttonRoll_Click);
        }

        /// <summary>
        /// Handles the event for next turn after player finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNextTurn_Click(object sender, EventArgs e)
        {
            if (game == null)
                return;

            if (game.RollerIndex == 0 && game.HasRolled && game.RollerPlacementComplete && !game.TurnComplete)
            {
                if (!game.PlaceAiDino(false))
                {
                    MessageBox.Show("The AI could not find a valid pen for the current roll.");
                    return;
                }

                DisplayGame();

                if (!game.TurnComplete)
                {
                    return;
                }
            }

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
            this.labelRollResult.Location = new System.Drawing.Point(721, 488);
            this.labelRollResult.Name = "labelRollResult";
            this.labelRollResult.Size = new System.Drawing.Size(72, 28);
            this.labelRollResult.TabIndex = 7;
            this.labelRollResult.Text = "label1";
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
