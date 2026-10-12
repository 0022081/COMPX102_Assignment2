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
        public List<Player> players = new List<Player>();
        // List of dino objects in game
        protected DinoBag bag = new DinoBag();
        // Number of rounds and turns in the game
        public int roundNumber = 1;
        // Number of turns in the game
        public int turnNumber = 1;
        // Holds the current roll condition from the dice roll
        protected PlacementCondition currentCondition;
        public bool player1Turn = false; // True if it's player 1's turn, false if it's player 2's turn
        private int currentRoller = 1;
        private bool unrestrictedPlacementComplete;
        private bool restrictedPlacementComplete;


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
            // Create two players
            Player player1 = new Player("Player 1");
            Player player2 = new Player("Player 2");
            // Add players to the list
            players.Add(player1);
            players.Add(player2);

            // Create dice object
            Dice dice = new Dice();
            // Create dino bag object
            DinoBag dinoBag = new DinoBag();
        }

        /// <summary>
        /// Runs the current turn for the player's, allowing the non roller player to place a dino in a pen based on the current placement condition and the roller player to place in any pen.
        /// </summary>
        public void PlayTurn(Dino selectedDino, Pen pen)
        {
            // Set the roller
            if (currentRoller == 0)
            {
                currentRoller = 1;
            }
            else
            {
                currentRoller = 0;
            }
            // reset placement flags
            unrestrictedPlacementComplete = false;
            restrictedPlacementComplete = false;

            // Roll the dice to determine the placement condition for this turn
            currentCondition = dice.Roll();
            // Display the current placement condition to the players
            MessageBox.Show($"Current Placement Condition: {currentCondition}");

            // Let roller place dino unrestricted
            if (currentRoller == 0)
            {
                bool unrestricted = true;
                AttemptPlaceDino(currentRoller, selectedDino, pen, unrestricted);
            }


            // Let nonroller place dino restricted

            // Increment turn
            turnNumber++;
            // Swap player's dino hands
            SwapHands();
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void AutoPlayTurn()
        {

        }


        public bool CheckRound()
        {
            if (roundNumber > 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roller"></param>
        public void StartRound(int roller)
        {
            if(CheckRound())
            {
                // Clear current hand lists for both players
                foreach(Player player in players)
                {
                    player.DinoHandList.Clear();
                    player.DinoHandList = bag.Draw(6);
                }

            }

            // Draw new dino hands for both players
            // Conintue with normal turns 

            // Increment round counter
            roundNumber++;
        }

        /// <summary>
        /// Swaps the hands between player 1 and player 2 after both have played 1 turn
        /// </summary>
        public void SwapHands()
        { 
            List<Dino> swap1DinoList = players[0].DinoHandList;
            List<Dino> swap2DinoList = players[1].DinoHandList;
            players[0].DinoHandList = swap2DinoList;
            players[1].DinoHandList = swap1DinoList;
            swap1DinoList.Clear();
            swap2DinoList.Clear();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerindex"></param>
        /// <param name="selectedDino"></param>
        /// <param name="pen"></param>
        public void AttemptPlaceDino(int playerindex, Dino selectedDino, Pen pen, bool unrestricted)
        {
            bool valid = false;

            valid = players[playerindex].checkDinoPlacement(selectedDino, currentCondition, pen, unrestricted);

            if(valid)
            {
                players[playerindex].PlaceDino(selectedDino, pen);
            }
            
        }
    }

}
