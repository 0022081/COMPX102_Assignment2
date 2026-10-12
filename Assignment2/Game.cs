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
        private bool hasRolled;
        private int rollerIndex;
        private bool turnComplete;
        private bool rollerPlacementComplete;
        private const int TurnsPerRound = 6;
        private const int DinosPerHand = 8;
        public PlacementCondition CurrentCondition
        {
            get { return currentCondition; }
        }

        public bool HasRolled
        {
            get { return hasRolled; }
        }

        public int RollerIndex
        {
            get { return rollerIndex; }
        }

        public bool TurnComplete
        {
            get { return turnComplete; }
        }

        public bool RollerPlacementComplete
        {
            get { return rollerPlacementComplete; }
        }


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

            bag.CreateBag(new Dino(DinoSpecies.T_Rex, 0, 0));
        }


        public PlacementCondition RollDice()
        {
            // Roll the dice to determine the placement condition for this turn
            currentCondition = dice.Roll();
            hasRolled = true;
            return currentCondition;
        }

        /// <summary>
        /// Runs the current turn for the player's, allowing the non roller player to place a dino in a pen based on the current placement condition and the roller player to place in any pen.
        /// </summary>
        public bool StartRound()
        {
            if (!CheckRound())
            {
                return false;
            }

            DealHands();

            turnNumber = 0;
            rollerIndex = 0;
            hasRolled = false;
            turnComplete = false;
            rollerPlacementComplete = false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedDino"></param>
        /// <param name="pen"></param>
        /// <returns></returns>
        public bool PlacePlayerDino(Dino selectedDino, Pen pen)
        {
            if (!hasRolled || turnComplete || (rollerIndex == 0 && rollerPlacementComplete))
            {
                return false;
            }

            bool unrestricted = rollerIndex == 0;
            bool placed = AttemptPlaceDino(0, selectedDino, pen, unrestricted);

            if (placed && rollerIndex == 1)
            {
                rollerIndex = 0;
                hasRolled = false;
                CompleteTurn();
            }
            else if (placed)
            {
                rollerPlacementComplete = true;
            }

            return placed;
        }

        /// <summary>
        /// AI places a dino in valid pen
        /// </summary>
        /// <param name="unrestricted"></param>
        /// <returns></returns>
        public bool PlaceAiDino(bool unrestricted)
        {
            if ((unrestricted && rollerPlacementComplete) || turnComplete)
            {
                return false;
            }

            Player ai = players[1];
            foreach (Dino dino in ai.DinoHandList)
            {
                List<Pen> pens = unrestricted
                    ? ai.Zoo.PenList
                    : ai.Zoo.GetValidPens(dino, currentCondition);

                foreach (Pen pen in pens)
                {
                    if (AttemptPlaceDino(1, dino, pen, unrestricted))
                    {
                        if (!unrestricted)
                        {
                            CompleteTurn();
                        }
                        else
                        {
                            rollerPlacementComplete = true;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// AI's turn to roll and place
        /// </summary>
        /// <returns></returns>
        public bool BeginAiTurn()
        {
            if (rollerIndex != 0 || !turnComplete || !hasRolled)
            {
                return false;
            }

            rollerIndex = 1;
            turnComplete = false;
            rollerPlacementComplete = false;
            RollDice();
            return PlaceAiDino(true);
        }

        private void CompleteTurn()
        {
            turnNumber++;
            rollerPlacementComplete = false;

            if (turnNumber >= TurnsPerRound)
            {
                roundNumber++;
                DealHands();
                turnNumber = 0;
                rollerIndex = 0;
                hasRolled = false;
                turnComplete = false;
                return;
            }

            turnComplete = true;
        }

        private void DealHands()
        {
            foreach (Player player in players)
            {
                player.DinoHandList.Clear();
                player.DinoHandList.AddRange(bag.Draw(DinosPerHand));
            }
        }

        /// <summary>
        /// Checks if 4 or more rounds have been played
        /// </summary>
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
        /// Player can attempt to place selected dino in selected pen
        /// </summary>
        public bool AttemptPlaceDino(int playerindex, Dino selectedDino, Pen pen, bool unrestricted)
        {
            if (players[playerindex].checkDinoPlacement(selectedDino, currentCondition, pen, unrestricted))
            {
                players[playerindex].PlaceDino(selectedDino, pen);
                return true;
            }

            return false;
        }
    }

}
