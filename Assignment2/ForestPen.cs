using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class ForestPen : Pen
    {
        protected const int XPos = 20;
        protected const int YPos = 20;

        /// <summary>
        /// Initializes a new instance of the ForestPen class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="boardSide"></param>
        /// <param name="penStyle"></param>
        /// <param name="maxDinos"></param>
        public ForestPen(string name, string boardSide, string penStyle, int maxDinos) : base(name, boardSide, penStyle, maxDinos)
        {
            this.name = name;
            this.boardSide = boardSide;
            this.penStyle = penStyle;
            this.maxDinos = maxDinos;
        }

        public override int CalculatePoints()
        {
            // Calculate points based on the number of dinos in the pen (does not care about species)
            Points = DinoList.Count;
            return Points;
        }

    }
}
