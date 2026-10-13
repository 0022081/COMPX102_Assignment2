using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class KingPen : Pen
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
        public KingPen(string name, string boardSide, string penStyle, int maxDinos, int HEIGHT, int WIDTH, int xPos, int yPos) : base(name, boardSide, penStyle, maxDinos, HEIGHT, WIDTH, xPos, yPos)
        {
            this.name = name;
            this.boardSide = boardSide;
            this.penStyle = penStyle;
            this.maxDinos = maxDinos;
        }

        public override int CalculatePoints()
        {
            // If players board contains the same or more amount of the dino species in this pen
            // return 7 ponts, otherwise return 0;
            return 0;
        }

        public override bool CanPlaceDino(Dino dino, PlacementCondition condition, bool unrestricted)
        {
            // If pen is not full and the placement condition is either Woodlands or FoodCourt, return true. Otherwise, return false.
            if (DinoList.Count >= maxDinos)
            {
                return false;
            }
            else
            {
                if (unrestricted)
                {
                    return true;
                }

                if (condition == PlacementCondition.Woodlands || condition == PlacementCondition.Restrooms)
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
