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

        public void StartNewGame()
        {
            // Reset the game state
            game = new Game();
            // Redraw the game board and hands

        }

        public void DisplayGame()
        {
            // Show the game form
            this.Show();
        }

        public void HandleHumanPlayerTurn()
        {
            // Handle the human player's turn
            if (game.IsHumanPlayerTurn())
            {
                // Wait for the player to click on a dino in their hand and place it in a pen
                // This is handled by the Game_MouseClick event
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game_form"/> class.
        /// </summary>
        public Game_form()
        {
            InitializeComponent();
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
    }
}
