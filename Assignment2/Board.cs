using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Board
    {
        protected List<Pen> penList = new List<Pen>();
        protected River river = new River();

        public List<Pen> PenList
        {
            get { return penList; }
            set { penList = value; }
        }

        /// <summary>
        /// Initalize the board object with pens in board
        /// </summary>
        public Board()
        {
            // create pens
            ForestPen forestPen = new ForestPen("Forest", "Left", "Woodlands", 8, 150, 210, 100, 100);
            MeadowPen meadowPen = new MeadowPen("Meadow", "Right", "Grasslands", 6, 140, 225, 200, 200);
            PrairiePen prairiePen = new PrairiePen("Prairie", "Left", "Grasslands", 8, 160, 150, 100, 300);
            WoodyPen woodyPen = new WoodyPen("Woody", "Left", "Woodlands", 3, 150, 165, 50, 500);
            KingPen kingPen = new KingPen("King", "Right", "Woodlands", 1, 110, 120, 400, 50);
            SolarityPen solarityPen = new SolarityPen("Solarity", "Right", "Grasslands", 1, 150, 150, 500, 500);

            PenList.Add(forestPen);
            PenList.Add(meadowPen);
            PenList.Add(prairiePen);
            PenList.Add(woodyPen);
            PenList.Add(kingPen);
            PenList.Add(solarityPen);
        }

        /// <summary>
        /// Return a list of valid pens for dino placement
        /// </summary>
        /// <param name="dino"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
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

        public bool PlaceDino(Dino dino, Pen pen)
        {
            // Implementation for placing a dino in a pen
            return true;
        }

        public void PlaceDinoInRiver(Dino dino)
        {
            // Implementation for placing a dino in the river
        }

        public int CalculateTotalPoints(Player player)
        {
            // Implementation for calculating total points from all pens and river
            return 0;
        }
    }
}
