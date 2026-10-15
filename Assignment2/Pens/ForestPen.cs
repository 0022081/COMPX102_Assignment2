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
            // Forest of Sameness scoring table (rulebook): 1 to 6 dinos = 2, 4, 8, 12, 18, 24 points
            int[] table = { 0, 2, 4, 8, 12, 18, 24 };
            Points = table[Math.Min(DinoList.Count, table.Length - 1)];
            return Points;
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
                // Forest of Sameness: the pen can only hold dinosaurs of the SAME species,
                // so reject a dino whose species differs from the ones already in the pen.
                if (dinoList.Count > 0 && dinoList[0].Species != dino.Species)
                {
                    return false;
                }

                if(unrestricted)
                {
                    return true;
                }

                return MeetsPlacementCondition(condition, unrestricted);
            }
        }

        public override void AddDino(Dino dino)
        {
            // Add the dino to the pen if it can be placed
            dinoList.Add(dino);

        }

    }
}
