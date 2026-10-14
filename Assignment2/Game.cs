using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment2
{
    internal class Game
    {
        private readonly AiStrategy aiStrategy = new AiStrategy();

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
        private const int TurnsPerRound = 3;
        private const int DinosPerHand = 6;
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
            roundNumber = 0;
        }

        /// <summary>
        /// Rolls the dice for placement condition
        /// </summary>
        /// <returns></returns>
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
                EndGame();
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
                MessageBox.Show("Player has already rolled / Completed turn / roller index out of range & roller has placed");
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
                MessageBox.Show("AI turn has already completed. Cannot run again");
                return false;
            }

            Player ai = players[1];
            AiMove move = aiStrategy.ChooseMove(ai, players[0], currentCondition, unrestricted);
            if (move == null)
            {
                return false;
            }

            if (move.Pen == null)
            {
                // No good or legal pen: use the River
                ai.DinoHandList.Remove(move.Dino);
                move.Dino.XPos = 305;
                move.Dino.YPos = 10 + (ai.Zoo.RiverCount * 25);
                ai.Zoo.AddDinoRiver(move.Dino);
            }
            else if (!AttemptPlaceDino(1, move.Dino, move.Pen, unrestricted))
            {
                return false;
            }

            if (!unrestricted) { CompleteTurn(); }
            else { rollerPlacementComplete = true; }
            return true;
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

        /// <summary>
        /// 
        /// </summary>
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

            SwapHands();              // both players have placed, so pass hands on
            turnComplete = true;
        }

        /// <summary>
        /// 
        /// </summary>
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

        public bool PlacePlayerDinoRiver(Dino dino)
        {
            if (dino == null || !hasRolled || turnComplete || (rollerIndex == 0 && rollerPlacementComplete))
                return false;

            Player p = players[0];
            p.DinoHandList.Remove(dino);
            dino.XPos = 305;
            dino.YPos = 10 + (p.Zoo.RiverCount * 25);
            p.Zoo.AddDinoRiver(dino);

            if (rollerIndex == 1)
            {
                rollerIndex = 0;
                hasRolled = false;
                CompleteTurn();
            }
            else
            {
                rollerPlacementComplete = true;
            }
            return true;
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

        /// <summary>
        /// Swaps the remaining dinos in the human's and AI's hands
        /// </summary>
        private void SwapHands()
        {
            List<Dino> temp = players[0].DinoHandList;
            players[0].DinoHandList = players[1].DinoHandList;
            players[1].DinoHandList = temp;
        }

        /// <summary>
        /// Handles the end of the game events
        /// </summary>
        public void EndGame()
        {
            Player player = players[0];
            Player opponent = players[1];

            int playerTotal = 0;
            int opponentTotal = 0;

            foreach (Pen pen in player.Zoo.PenList)
            {
                playerTotal += pen.CalculatePoints(player.Zoo, opponent.Zoo);
            }
            playerTotal += player.Zoo.RiverPoints;

            foreach (Pen pen in opponent.Zoo.PenList)
            {
                opponentTotal += pen.CalculatePoints(opponent.Zoo, player.Zoo);
            }
            opponentTotal += opponent.Zoo.RiverPoints;

            // Find winner or draw
            if (playerTotal > opponentTotal)
            {
                MessageBox.Show($"Winner: Player 1 Total Points: {playerTotal}");
            }
            else if (playerTotal < opponentTotal)
            {
                MessageBox.Show($"Winner: AI Player Total Points: {opponentTotal}");
            }
            else
            {
                MessageBox.Show($"Draw Player Points: {playerTotal} AI Points: {opponentTotal}");
            }

        }
    }

}
