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
        private List<PictureBox> targetPictureBoxes = new List<PictureBox>();

        // Dice object for rolling the dice
        Dice dice = new Dice();
        // List of players in game
        List<Player> players = new List<Player>();
        // List of dino objects in game
        List<Dino> mainDinosList = new List<Dino>();
        // List of pens in game
        List<Pen> pensList = new List<Pen>();
        // True if it's player 1's turn, false if it's player 2's turn
        public bool playerTurn = true;

        //Max dinos for each pen
        public const int MAX_DINOS_FOREST = 8;
        // Pen instances
        private ForestPen forestPen;



        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            InitializeComponent();
            // wire up load and mouse handlers
            this.Load += Game_Load;
            this.MouseClick += Game_MouseClick;
        }

        /// <summary>
        /// Handles the Load event of the Game control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Game_Load(object sender, EventArgs e)
        {
            // Add all picture boxes to the targetPictureBoxes list for easy access
            targetPictureBoxes.Add(pictureBoxPlayer1Hand);
            targetPictureBoxes.Add(pictureBoxPlayer2Hand);
            targetPictureBoxes.Add(pictureBoxBoard);

            // Create two players
            Player player1 = new Player("Player 1", true, 0);
            Player player2 = new Player("Player 2", false, 0);
            // Add players to the list
            players.Add(player1);
            players.Add(player2);

            // Creat 8 dino objects of each species and add them to the mainDinosList
            for (int i = 0; i < 8; i++)
            {
                mainDinosList.Add(new Dino("T-Rex", 0, 0));
                mainDinosList.Add(new Dino("Triceratops", 0, 0));
                mainDinosList.Add(new Dino("Velociraptor", 0, 0));
                mainDinosList.Add(new Dino("Stegosaurus", 0, 0));
                mainDinosList.Add(new Dino("Allosaurus", 0, 0));
                mainDinosList.Add(new Dino("Dilophosaurus", 0, 0));

            }
            // create pens
            forestPen = new ForestPen("Forest", "Left", "Woodlands", MAX_DINOS_FOREST);
            pensList.Add(forestPen);

            // Each player chooses their dino hand from the mainDinosList
            player1.ChooseDinoHand(mainDinosList);
            player2.ChooseDinoHand(mainDinosList);
            // Initialize PictureBox images so we can draw directly into them
            pictureBoxPlayer1Hand.Image = new Bitmap(pictureBoxPlayer1Hand.Width, pictureBoxPlayer1Hand.Height);
            pictureBoxPlayer2Hand.Image = new Bitmap(pictureBoxPlayer2Hand.Width, pictureBoxPlayer2Hand.Height);

            // Draw initial hands into their picture boxes
            Game_Paint();

            //Console.WriteLine(player1.DinoHandList.Count());

            // Display the first player's turn
            UpdateTurn();

        }

        /// <summary>
        /// Handles the Paint event of the Game control. Draws the pens and players' dino hands on the game board.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_Paint()
        {
            DrawPensToPictureBoxBoard();
            DrawHandsToPictureBoxes();
        }
        /// <summary>
        /// Draws each player's dino hand into their PictureBox.Image so they are visible.
        /// </summary>
        private void DrawHandsToPictureBoxes()
        {
            if (pictureBoxPlayer1Hand.Image == null)
                pictureBoxPlayer1Hand.Image = new Bitmap(pictureBoxPlayer1Hand.Width, pictureBoxPlayer1Hand.Height);
            if (pictureBoxPlayer2Hand.Image == null)
                pictureBoxPlayer2Hand.Image = new Bitmap(pictureBoxPlayer2Hand.Width, pictureBoxPlayer2Hand.Height);

            using (Graphics g1 = Graphics.FromImage(pictureBoxPlayer1Hand.Image))
            {
                g1.Clear(Color.Transparent);
                players[0].DrawDinoHand(g1, pictureBoxPlayer1Hand);
            }

            using (Graphics g2 = Graphics.FromImage(pictureBoxPlayer2Hand.Image))
            {
                g2.Clear(Color.Transparent);
                players[1].DrawDinoHand(g2, pictureBoxPlayer2Hand);
            }

            pictureBoxPlayer1Hand.Refresh();
            pictureBoxPlayer2Hand.Refresh();
        }

        /// <summary>
        /// Draws each pen in the pensList to the pictureBoxBoard.Image so they are visible.
        /// </summary>
        private void DrawPensToPictureBoxBoard()
        {
            if (pictureBoxBoard.Image == null)
                pictureBoxBoard.Image = new Bitmap(pictureBoxBoard.Width, pictureBoxBoard.Height);
            using (Graphics g = Graphics.FromImage(pictureBoxBoard.Image))
            {
                g.Clear(Color.Transparent);
                foreach (Pen pen in pensList)
                {
                    pen.Draw(g); // Draw each pen at its predefined location
                }
            }
            pictureBoxBoard.Refresh();
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
                string player1RollResult = dice.Roll();
                // Display the roll result in a message box
                //MessageBox.Show($"Player 1 rolled: {player1RollResult}");
                // After rolling, the player can click to place a dino. Click events are handled by Game_MouseClick.

            }
            else
            {
                lblTurn.Text = "Player 2's Turn";
                string player2RollResult = dice.Roll();
                // Display the roll result in a message box
                //MessageBox.Show($"Player 2 rolled: {player2RollResult}");
                // After rolling, the player can click to place a dino. Click events are handled by Game_MouseClick.

            }
        }

        /// <summary>
        /// Handles the MouseClick event of the Game control. Forwards mouse clicks to the active player's placement method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBoxPlayer1Hand_Click(object sender, EventArgs e)
        {
            // get mouse click coordinates relative to the picture box
            MouseEventArgs me = (MouseEventArgs)e;


            Console.WriteLine($"Mouse clicked at: {me.X}, {me.Y}");
            // Forward mouse clicks to the active player's placement method, using the forestPen as an example.
            if (playerTurn)
            {
                // If player 1's turn
                // If player 1 clicked on dino in their hand, place it in a pen in the pictureBoxBoard if allowed
                players[0].PlaceDinoInPen(forestPen, me);


                // Redraw the picture boxes after any change
                Game_Paint();
            }
            else
            {
                // If player 2's turn
            }
            // Redraw the form to reflect any changes
            Invalidate();
        }

        private void pictureBoxPlayer2Hand_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxBoard_Click(object sender, EventArgs e)
        {

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
