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
        // Ai object 
        private readonly AiStrategy aiStrategy = new AiStrategy();

        // Dice object for rolling the dice
        protected Dice dice = new Dice();
        // Players participating in the game.
        public List<Player> players = new List<Player>();
        // Bag supplying dinosaurs to players.
        protected DinoBag bag = new DinoBag();
        // Current round number.
        public int roundNumber = 1;
        // Current turn number within the round.
        public int turnNumber = 1;
        // Placement condition from the current dice roll.
        protected PlacementCondition currentCondition;
        // Whether the current turn's dice roll has been completed.
        private bool hasRolled;
        // Index of the player who rolled the current condition.
        private int rollerIndex;
        // Whether both players have completed the current turn.
        private bool turnComplete;
        // Whether the roller has placed their dinosaur.
        private bool rollerPlacementComplete;
        // Whether both players have placed and the human must now discard a dinosaur.
        private bool awaitingDiscard;
        // Number of turns in each round.
        private const int TurnsPerRound = 3;
        // Number of dinosaurs dealt to each player.
        private const int DinosPerHand = 6;

        public PlacementCondition CurrentCondition
        {
            get { return currentCondition; }
        }

        // Returns the condition if player has rolled
        public bool HasRolled
        {
            get { return hasRolled; }
        }

        // Returns 
        public int RollerIndex
        {
            get { return rollerIndex; }
        }

        // Indicates whether the current turn is complete.
        public bool TurnComplete
        {
            get { return turnComplete; }
        }

        // Indicates whether the roller has placed a dinosaur.
        public bool RollerPlacementComplete
        {
            get { return rollerPlacementComplete; }
        }

        // Indicates that both players have placed and the human must choose a dinosaur to discard.
        public bool AwaitingDiscard
        {
            get { return awaitingDiscard; }
        }

        // Indicates whether the human player may place a dinosaur right now.
        public bool CanPlayerPlace
        {
            get
            {
                return hasRolled && !turnComplete && !awaitingDiscard
                    && !(rollerIndex == 0 && rollerPlacementComplete);
            }
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
            // A new roll always starts a new turn, so clear the previous turn's completion flags.
            turnComplete = false;
            rollerPlacementComplete = false;
            awaitingDiscard = false;
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

            // A new round replaces both hands before the first turn begins.
            DealHands();

            turnNumber = 0;
            rollerIndex = 0;
            hasRolled = false;
            turnComplete = false;
            rollerPlacementComplete = false;
            awaitingDiscard = false;
            return true;
        }

        /// <summary>
        /// Places the human player's selected dinosaur in a pen when the move is legal.
        /// </summary>
        /// <param name="selectedDino">Dinosaur selected from the player's hand.</param>
        /// <param name="pen">Target pen.</param>
        /// <returns><c>true</c> when the dinosaur was placed.</returns>
        public bool PlacePlayerDino(Dino selectedDino, Pen pen)
        {
            if (!CanPlayerPlace)
            {
                return false;
            }

            // The roller may use any pen; the other player must follow the die condition.
            bool unrestricted = rollerIndex == 0;
            bool placed = AttemptPlaceDino(0, selectedDino, pen, unrestricted);

            if (placed)
            {
                OnPlayerPlaced();
            }

            return placed;
        }

        /// <summary>
        /// Updates the turn state after the human player has placed a dinosaur.
        /// </summary>
        private void OnPlayerPlaced()
        {
            if (rollerIndex == 1)
            {
                // The AI rolled and has already placed, so both players have now placed.
                awaitingDiscard = true;
            }
            else
            {
                // The human rolled; the AI still needs to place.
                rollerPlacementComplete = true;
            }
        }

        /// <summary>
        /// Returns the pens in the human player's zoo where the dinosaur can legally be placed right now.
        /// </summary>
        /// <param name="dino">Dinosaur being tested.</param>
        /// <returns>Valid pens, or an empty list when the player cannot place.</returns>
        public List<Pen> GetValidPlayerPens(Dino dino)
        {
            List<Pen> validPens = new List<Pen>();
            if (dino == null || !CanPlayerPlace)
            {
                return validPens;
            }

            // The roller ignores the die; the other player must obey it. Pen rules always apply.
            bool unrestricted = rollerIndex == 0;
            foreach (Pen pen in players[0].Zoo.PenList)
            {
                if (players[0].checkDinoPlacement(dino, currentCondition, pen, unrestricted))
                {
                    validPens.Add(pen);
                }
            }
            return validPens;
        }

        /// <summary>
        /// The human discards a dinosaur from their hand (it goes back in the box), then the AI
        /// discards one too and the hands are passed on.
        /// </summary>
        /// <param name="dino">Dinosaur from the human player's hand to discard.</param>
        /// <returns><c>true</c> when the discard was completed.</returns>
        public bool DiscardPlayerDino(Dino dino)
        {
            if (!awaitingDiscard || dino == null || !players[0].DinoHandList.Contains(dino))
            {
                return false;
            }

            // Discarded dinosaurs are removed from the game (put back in the box).
            players[0].DinoHandList.Remove(dino);

            Dino aiDiscard = aiStrategy.ChooseDiscard(players[1], players[0]);
            if (aiDiscard != null)
            {
                players[1].DinoHandList.Remove(aiDiscard);
            }

            awaitingDiscard = false;

            if (rollerIndex == 1)
            {
                // The AI's rolling turn is over, so the human rolls next.
                rollerIndex = 0;
                hasRolled = false;
            }
            CompleteTurn();
            return true;
        }

        /// <summary>
        /// AI places a dino in valid pen
        /// </summary>
        /// <param name="unrestricted"></param>
        /// <returns></returns>
        public bool PlaceAiDino(bool unrestricted)
        {
            if ((unrestricted && rollerPlacementComplete) || turnComplete || awaitingDiscard)
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

            // A null pen is the AI strategy's explicit choice to use the river.
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

            // The AI was the non roller, so both players have placed and the discard phase begins.
            if (!unrestricted) { awaitingDiscard = true; }
            else { rollerPlacementComplete = true; }
            return true;
        }

        /// <summary>
        /// AI's turn to roll and place
        /// </summary>
        /// <returns></returns>
        public bool BeginAiTurn()
        {
            if (rollerIndex != 0 || !turnComplete || !hasRolled || awaitingDiscard)
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
        /// Advances the game after both players complete their placements.
        /// </summary>
        private void CompleteTurn()
        {
            // Both placements are complete, so advance the turn before checking the round boundary.
            turnNumber++;
            rollerPlacementComplete = false;
            awaitingDiscard = false;

            if (turnNumber >= TurnsPerRound)
            {
                // Start the next round with fresh hands and reset turn ownership.
                roundNumber++;
                DealHands();
                turnNumber = 0;
                rollerIndex = 0;
                hasRolled = false;
                turnComplete = false;
                return;
            }

            // Both players have placed, so pass their remaining hands to each other.
            SwapHands();
            turnComplete = true;
        }

        /// <summary>
        /// Deals a fresh hand of dinosaurs to each player.
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

        /// <summary>
        /// Places the human player's dinosaur in the river and advances turn state.
        /// </summary>
        /// <param name="dino">Dinosaur to place in the river.</param>
        /// <returns><c>true</c> when the dinosaur was moved to the river.</returns>
        public bool PlacePlayerDinoRiver(Dino dino)
        {
            if (dino == null || !CanPlayerPlace)
                return false;

            // River placement follows the same roller/non-roller state transition as pen placement.
            Player p = players[0];
            p.DinoHandList.Remove(dino);
            dino.XPos = 305;
            dino.YPos = 10 + (p.Zoo.RiverCount * 25);
            p.Zoo.AddDinoRiver(dino);

            OnPlayerPlaced();
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

            // Context-aware scoring is required for pens that compare both zoos.
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
