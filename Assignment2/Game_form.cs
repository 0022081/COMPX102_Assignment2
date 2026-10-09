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
        private int selectedPlayerIndex = -1;

        public void StartNewGame()
        {
            // Reset the game state
            game = new Game();
            // initialize players and other game data
            game.LoadGame(this, EventArgs.Empty);
            // Start new round
            game.PlayRound();
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
                game.players[0].DrawDinoHand(g1, pictureBoxPlayer1Hand.Width);
            }

            using (Graphics g2 = Graphics.FromImage(pictureBoxPlayer2Hand.Image))
            {
                g2.Clear(Color.Transparent);
                game.players[1].DrawDinoHand(g2, pictureBoxPlayer2Hand.Width);
            }

            pictureBoxPlayer1Hand.Refresh();
            pictureBoxPlayer2Hand.Refresh();


            if (pictureBoxBoard.Image == null)
                pictureBoxBoard.Image = new Bitmap(pictureBoxBoard.Width, pictureBoxBoard.Height);
            using (Graphics g = Graphics.FromImage(pictureBoxBoard.Image))
            {
                g.Clear(Color.Transparent);
                foreach (Pen pen in game.players[0].Zoo.PenList)
                {
                    pen.Draw(g); // Draw each pen at its predefined location
                }
            }
            pictureBoxBoard.Refresh();
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
            if (selectedDino == null || selectedPlayerIndex < 0)
                return;

            Player p = game.players[selectedPlayerIndex];
            // check which pen was clicked on the board
            foreach (Pen pen in p.Zoo.PenList)
            {
                if (e.X >= pen.XPos && e.X <= pen.XPos + pen.Width && e.Y >= pen.YPos && e.Y <= pen.YPos + pen.Height)
                {
                    // Attempt placement via the game rules (enforces roller/non-roller restrictions)
                    bool placed = game.AttemptPlaceDino(selectedPlayerIndex, selectedDino, pen);
                    if (placed)
                    {
                        // update pen dino positions and refresh display
                        pen.assignDinoPositions(pen.XPos, pen.YPos);
                        selectedDino = null;
                        selectedPlayerIndex = -1;
                        DisplayGame();
                    }
                    break;
                }
            }
        }

        public void HandleHumanPlayerTurn()
        {
            // Handle the human player's turn
            // The form's mouse handlers already allow selecting a dino and clicking a pen.
            // This method can be used to provide UI feedback when the game is waiting for a placement.
            if (game != null && game.IsHumanPlayerTurn())
            {
                // For now, simply ensure the form is active so user can interact
                this.Activate();
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
            pictureBoxBoard.MouseClick += pictureBoxBoard_MouseClick;
            // wire other control events
            this.buttonNextTurn.Click += new EventHandler(this.buttonNextTurn_Click);
        }

        private void buttonNextTurn_Click(object sender, EventArgs e)
        {
            if (game == null)
                return;

            // If game is waiting for human placement, ask AI to complete remaining placements
            if (game.IsHumanPlayerTurn())
            {
                // Let AI perform automated placements for any remaining placements this turn
                game.AutoPlayTurn();
                DisplayGame();
                return;
            }

            // If not currently in a placement phase, start the next turn: alternate roller between players
            int currentRoller = game.GetCurrentRoller();
            int nextRoller = (currentRoller >= 0) ? 1 - currentRoller : 0;
            game.StartTurn(nextRoller);
            DisplayGame();
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

        private void buttonStartGame_Click(object sender, EventArgs e)
        {
            StartNewGame();
            // start with player 1 as the roller
            game.StartTurn(0);
            DisplayGame();
        }
    }
}
