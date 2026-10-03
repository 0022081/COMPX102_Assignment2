using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment2
{
    internal class Game
    {
        // Dice object for rolling the dice
        protected Dice dice = new Dice();
        // List of players in game
        protected List<Player> players = new List<Player>();
        // List of dino objects in game
        protected DinoBag bag = new DinoBag();
        // Number of rounds and turns in the game
        protected int roundNumber = 1;
        // Number of turns in the game
        protected int turnNumber = 1;
        // Holds the current roll condition from the dice roll
        protected PlacementCondition currentCondition;
        protected bool player1Turn = false; // True if it's player 1's turn, false if it's player 2's turn



        //Max dinos for each pen
        protected const int MAX_DINOS_FOREST = 8;



        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
        }

        /// <summary>
        /// Handles the Load event of the Game control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadGame(object sender, EventArgs e)
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

        public void PlayRound()
        {
            // Each player has 3 turns per round and 4 rounds in the game
            for (int i = 0; i < 3; i++)
            {
                // Each player takes a turn
                foreach (Player player in players)
                {
                    // Switch player turn bool to keep track whos turn it is
                    player1Turn = !player1Turn;
                    // Allow both players to have a turn
                    PlayTurn();
                }
            }
            // After both players have taken their turn, increment the round number
            roundNumber++;
        }

        /// <summary>
        /// Runs the current turn for the player's, allowing the non roller player to place a dino in a pen based on the current placement condition and the roller player to place in any pen.
        /// </summary>
        public void PlayTurn()
        {
            // Roll the dice to determine the placement condition for this turn
            currentCondition = dice.Roll();
            // Display the current placement condition to the players
            MessageBox.Show($"Current Placement Condition: {currentCondition}");

            if (player1Turn)
            {
                // If player 1's turn

                // Let player 1 place dino anywhere
                // Get dino that player clicked on from hand
                // If player clicked on pen && pen allowed to place dino in it, place dino in that pen
                // Let player 2 now place

                // Let player 2 place dino depending on roll conditon
                // Get dino that player clicked on from hand
                // If player clicked on pen && pen allowed to place dino in it, place dino in that pen
            }
            else
            {
                // If player 2's turn
                // Let player 2 place dino anywhere
                // Get dino that player clicked on from hand
                // If player clicked on pen && pen allowed to place dino in it, place dino in that pen
                // Let player 1 now place

                // Let player 1 place dino depending on roll conditon
                // Get dino that player clicked on from hand
                // If player clicked on pen && pen allowed to place dino in it, place dino in that pen
            }
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

    }
}
