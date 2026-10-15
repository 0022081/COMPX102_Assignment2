using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2
{
    /// <summary>
    /// A move chosen by the AI. Pen == null means "place in the River".
    /// </summary>
    class AiMove
    {
        // Dinosaur selected for the move.
        public Dino Dino;
        // Pen selected for the move; null means the river.
        public Pen Pen;
        // Estimated points contributed by the move.
        public double Score;
    }

    /// <summary>
    /// Medium difficulty AI: scores every legal (dino, pen) pair using the rulebook scoring,
    /// then usually plays the best move but sometimes picks one of the top 3 at random.
    /// </summary>
    class AiStrategy
    {
        // Shared random so results are not repeated
        private static readonly Random rand = new Random();

        // Chance (0-1) that the AI plays a random move from its top 3 instead of the best one
        private const double MistakeChance = 0.25;

        // Marginal points gained by adding the Nth dino (from rulebook scoring tables)
        // Point gain for adding each successive dinosaur to a forest.
        private static readonly int[] ForestGain = { 2, 2, 4, 4, 6, 6, 6, 6 };
        // Point gain for adding each successive dinosaur to a meadow.
        private static readonly int[] MeadowGain = { 1, 2, 3, 4, 5, 6 };

        /// <summary>
        /// Chooses a dino from the AI's hand and where to put it.
        /// </summary>
        /// <param name="ai">The AI player</param>
        /// <param name="opponent">The human player (used for King of the Jungle)</param>
        /// <param name="condition">Current die condition</param>
        /// <param name="unrestricted">True when the AI rolled the die (ignores the condition)</param>
        public AiMove ChooseMove(Player ai, Player opponent, PlacementCondition condition, bool unrestricted)
        {
            List<AiMove> moves = new List<AiMove>();

            foreach (Dino dino in ai.DinoHandList)
            {
                // River is always allowed
                moves.Add(new AiMove { Dino = dino, Pen = null, Score = ScoreRiver(dino) });

                foreach (Pen pen in ai.Zoo.PenList)
                {
                    if (IsLegal(ai, dino, pen, condition, unrestricted))
                    {
                        moves.Add(new AiMove { Dino = dino, Pen = pen, Score = ScorePen(ai, opponent, dino, pen) });
                    }
                }
            }

            if (moves.Count == 0)
            {
                return null;
            }

            // Rank by expected value, then randomize equal scores so the AI is not predictable.
            List<AiMove> ranked = moves.OrderByDescending(m => m.Score).ThenBy(m => rand.Next()).ToList();

            // Occasionally choose among the top three to model imperfect play.
            if (ranked.Count > 1 && rand.NextDouble() < MistakeChance)
            {
                return ranked[rand.Next(Math.Min(3, ranked.Count))];
            }
            return ranked[0];
        }

        /// <summary>
        /// Chooses which dinosaur the AI puts back in the box after placing.
        /// The hand is passed to the opponent next, so the AI discards the dinosaur
        /// that would be most valuable to the opponent's zoo.
        /// </summary>
        /// <param name="ai">The AI player</param>
        /// <param name="opponent">The human player who will receive the rest of the hand</param>
        /// <returns>The dinosaur to discard, or null when the hand is empty.</returns>
        public Dino ChooseDiscard(Player ai, Player opponent)
        {
            Dino best = null;
            double bestValue = double.MinValue;

            // Shuffle first so equal values are not always discarded in the same order.
            foreach (Dino dino in ai.DinoHandList.OrderBy(d => rand.Next()).ToList())
            {
                // The opponent can always use the river
                double value = ScoreRiver(dino);

                foreach (Pen pen in opponent.Zoo.PenList)
                {
                    // The die is not known yet, so assume the opponent could use any pen
                    if (IsLegal(opponent, dino, pen, PlacementCondition.Woodlands, true))
                    {
                        value = Math.Max(value, ScorePen(opponent, ai, dino, pen));
                    }
                }

                if (value > bestValue)
                {
                    bestValue = value;
                    best = dino;
                }
            }
            return best;
        }

        /// <summary>
        /// Checks the placement is legal. Uses the pen's own check plus the rules
        /// (same/different species, Empty Pen, T-Rex die) so the AI never breaks them.
        /// </summary>
        private bool IsLegal(Player ai, Dino dino, Pen pen, PlacementCondition condition, bool unrestricted)
        {
            if (!ai.checkDinoPlacement(dino, condition, pen, unrestricted))
                return false;

            if (pen is ForestPen && pen.DinoList.Count > 0 && pen.DinoList[0].Species != dino.Species)
                return false;

            if (pen is MeadowPen && pen.DinoList.Any(d => d.Species == dino.Species))
                return false;
            return true;
        }

        /// <summary>
        /// Returns the fixed point value assigned to placing a dinosaur in the river.
        /// </summary>
        private double ScoreRiver(Dino dino)
        {
            // River gives 1 point per dino; keep it as the last resort
            return 1.0;
        }

        /// <summary>
        /// Estimated points gained by placing the dino in the pen.
        /// </summary>
        private double ScorePen(Player ai, Player opponent, Dino dino, Pen pen)
        {
            int n = pen.DinoList.Count;
            double score;

            if (pen is ForestPen)
            {
                score = ForestGain[Math.Min(n, ForestGain.Length - 1)];
            }
            else if (pen is MeadowPen)
            {
                score = MeadowGain[Math.Min(n, MeadowGain.Length - 1)];
            }
            else if (pen is PrairiePen)
            {
                int same = pen.DinoList.Count(d => d.Species == dino.Species);
                // Odd count means this dino completes a couple (5 points)
                score = (same % 2 == 1) ? 5.0 : 1.5;
            }
            else if (pen is WoodyPen)
            {
                // 7 points only when it ends with exactly 3 dinos
                score = (n == 2) ? 3.0 : 2.0;
            }
            else if (pen is KingPen)
            {
                int mine = CountSpecies(ai, dino.Species) + 1;
                int theirs = CountSpecies(opponent, dino.Species);
                score = (mine >= theirs) ? 6.0 : 1.0;
            }
            else if (pen is SolarityPen)
            {
                // Needs to be the only one of its species in the zoo
                score = (CountSpecies(ai, dino.Species) == 0) ? 6.0 : -2.0;
            }
            else
            {
                score = 0;
            }

            // Apply the T-Rex bonus only when this move creates a new T-Rex-containing pen.
            if (dino.Species == DinoSpecies.T_Rex && !pen.HasTRex())
            {
                score += 1.0;
            }

            // Penalize moves that would invalidate another solitary species pen.
            foreach (Pen other in ai.Zoo.PenList)
            {
                if (other is SolarityPen && other != pen && other.DinoList.Count > 0
                    && other.DinoList[0].Species == dino.Species)
                {
                    score -= 6.0;
                }
            }

            return score;
        }

        /// <summary>
        /// Counts how many dinos of a species a player has in their pens.
        /// </summary>
        /// <summary>
        /// Counts a species across the player's zoo.
        /// </summary>
        private int CountSpecies(Player player, DinoSpecies species)
        {
            return player.Zoo.CountSpecies(species);
        }
    }
}