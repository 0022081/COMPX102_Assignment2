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
        // List of players in game
        List<Player> players = new List<Player>();
        // List of dino objects in game
        List<Dino> dinos = new List<Dino>();
        // True if it's player 1's turn, false if it's player 2's turn
        public bool playerTurn = true; 



        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            InitializeComponent();
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
            // Create dino objects and add them to the list
            dinos.Add(new Dino("T-Rex"));
            dinos.Add(new Dino("Triceratops"));
            dinos.Add(new Dino("Velociraptor"));
            dinos.Add(new Dino("Stegosaurus"));
            dinos.Add(new Dino("Brachiosaurus"));
            // Display the first player's turn
            UpdateTurnLabel();
        }


    }
}
