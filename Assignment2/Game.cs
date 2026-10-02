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
    public partial class Game : Form
    {
        // Dice object for rolling the dice
        Dice dice = new Dice();
        // List of players in game
        List<Player> players = new List<Player>();
        // List of dino objects in game
        List<Dino> mainDinosList = new List<Dino>();
        // True if it's player 1's turn, false if it's player 2's turn
        public bool playerTurn = true;

        //Max dinos for each pen
        public const int MAX_DINOS_FOREST = 8;



        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            InitializeComponent();
            Game_Load(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the Load event of the Game control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Game_Load(object sender, EventArgs e)
        {
            // Create two players
            Player player1 = new Player("Player 1", true, 0);
            Player player2 = new Player("Player 2", false, 0);
            // Add players to the list
            players.Add(player1);
            players.Add(player2);

            // Creat 8 dino objects of each species and add them to the mainDinosList
            for (int i = 0; i < 8; i++)
            {
                mainDinosList.Add(new Dino("T-Rex"));
                mainDinosList.Add(new Dino("Triceratops"));
                mainDinosList.Add(new Dino("Velociraptor"));
                mainDinosList.Add(new Dino("Stegosaurus"));
                mainDinosList.Add(new Dino("Allosaurus"));
                mainDinosList.Add(new Dino("Dilophosaurus"));

            }
            // Display the first player's turn
            UpdateTurn(MouseEventArgs e);
        }

        /// <summary>
        /// Updates the which player's turn it is.
        /// </summary>
        public void UpdateTurn()
        {
            // Update the label to show which player's turn it is
            if (playerTurn)
            {
                lblTurn.Text = "Player 1's Turn";
                // enable the player to roll the dice and place a dino in a pen
                string rollResult = dice.Roll();
                // Display the roll result in a message box
                MessageBox.Show($"Player 1 rolled: {rollResult}");
                // Allow player 1 to place a dino in any pen
                players[0].PlaceDinoInPen(MouseEventArgs e)

            }
            else
            {
                lblTurn.Text = "Player 2's Turn";
            }
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
