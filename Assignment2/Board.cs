using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Board
    {
        // Pens available on this board.
        protected List<Pen> penList = new List<Pen>();

        // River containing dinosaurs not placed in a pen.
        protected River river = new River();

        // list of Pens in current board
        public List<Pen> PenList
        {
            get { return penList; }
            set { penList = value; }
        }

        // get the number of dinos in the river
        public int RiverCount { get { return river.Count; } }

        public int RiverPoints { get { return river.CalculatePoints(); } }

        /// <summary>
        /// Initalize the board object with pens in board
        /// </summary>
        public Board()
        {
            // create pens
            ForestPen forestPen = new ForestPen("Forest", "Left", "Woodlands", 8, 150, 210, 15, 15);
            MeadowPen meadowPen = new MeadowPen("Meadow", "Right", "Grasslands", 6, 140, 225, 350, 230);
            PrairiePen prairiePen = new PrairiePen("Prairie", "Left", "Grasslands", 10, 175, 150, 45, 390);
            WoodyPen woodyPen = new WoodyPen("Woody", "Left", "Woodlands", 3, 150, 165, 35, 230);
            KingPen kingPen = new KingPen("King", "Right", "Woodlands", 1, 110, 120, 393, 40);
            SolarityPen solarityPen = new SolarityPen("Solarity", "Right", "Grasslands", 1, 150, 150, 440, 400);

            // add created pens to boards pen's list
            PenList.Add(forestPen);
            PenList.Add(meadowPen);
            PenList.Add(prairiePen);
            PenList.Add(woodyPen);
            PenList.Add(kingPen);
            PenList.Add(solarityPen);
        }

        /// <summary>
        /// Returns the pens that accept the dinosaur under the current condition.
        /// </summary>
        /// <param name="dino">Dinosaur being tested.</param>
        /// <param name="condition">Current placement condition.</param>
        /// <returns>Pens where the dinosaur may be placed.</returns>
        public List<Pen> GetValidPens(Dino dino, PlacementCondition condition)
        {
            List<Pen> validPens = new List<Pen>();
            foreach (Pen pen in PenList)
            {
                if (pen.CanPlaceDino(dino, condition, false))
                {
                    validPens.Add(pen);
                }
            }
            
            // Add river pen to valid pens list
            return validPens;
        }

        /// <summary>
        /// Counts the specified species in the river and every pen on the board.
        /// </summary>
        /// <param name="species">Species to count.</param>
        /// <returns>Total number of matching dinosaurs.</returns>
        public int CountSpecies(DinoSpecies species)
        {
            int count = river.CountSpecies(species);
            foreach (Pen pen in penList)
            {
                count += pen.DinoList.Count(d => d.Species == species);
            }
            return count;
        }

        /// <summary>
        /// Add dino to river
        /// </summary>
        /// <param name="dino">Dinosaur to add to the river.</param>
        public void AddDinoRiver(Dino dino)
        {
            river.AddDino(dino);
        }

        /// <summary>
        /// Display river pen
        /// </summary>
        /// <param name="g">Graphics surface on which to draw the river.</param>
        public void DisplayRiver(Graphics g)
        {
            river.Draw(g);
        }
    }
}
