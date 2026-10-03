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

        public List<Pen> GetValidPens(Dino dino, bool condition)
        {
            // Implementation for getting valid pens based on dino and condition
            return new List<Pen>();
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
