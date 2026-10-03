using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class ForestPen : Pen
    {

        /// <summary>
        /// Initializes a new instance of the ForestPen class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="boardSide"></param>
        /// <param name="penStyle"></param>
        /// <param name="maxDinos"></param>
        /// <param name="HEIGHT"></param>
        /// <param name="WIDTH"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        public ForestPen(string name, string boardSide, string penStyle, int maxDinos, int HEIGHT, int WIDTH, int xPos, int yPos) : base(name, boardSide, penStyle, maxDinos, HEIGHT, WIDTH, xPos, yPos )
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

        public override bool CanPlaceDino(Dino dino, PlacementCondition condition)
        {
            // If pen is not full and the placement condition is either Woodlands or FoodCourt, return true. Otherwise, return false.
            if (DinoList.Count >= maxDinos)
            {
                return false;
            }
            else
            {
                if (condition == PlacementCondition.Woodlands || condition == PlacementCondition.FoodCourt)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override void AddDino(Dino dino)
        {
            // Add the dino to the pen if it can be placed
            DinoList.Add(dino);

        }

    }
}
