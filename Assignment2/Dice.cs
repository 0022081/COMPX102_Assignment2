using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Dice
    {
        /// <summary>
        /// Holds the list of roll types for the dice
        /// </summary>
        protected List<PlacementCondition> rollTypes = new List<PlacementCondition>
        {
            PlacementCondition.Woodlands,
            PlacementCondition.Grasslands,
            PlacementCondition.Restrooms,
            PlacementCondition.FoodCourt,
            PlacementCondition.EmptyPen,
            PlacementCondition.T_Rex
        };

        /// <summary>
        /// Holds the current placement condition rolled by the dice.
        /// </summary>
        protected PlacementCondition currentCondition = PlacementCondition.Woodlands;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dice"/> class.
        /// </summary>
        public Dice()
        {
            // Constructor for the Dice class
        }

        /// <summary>
        /// Rolls the dice and returns a random roll type from the list of roll types.
        /// </summary>
        /// <returns>PlacementCondition of the rolled type</returns>
        public PlacementCondition Roll()
        {
            Random rand = new Random();
            int rollIndex = rand.Next(rollTypes.Count);
            currentCondition = rollTypes[rollIndex];
            return currentCondition;
        }
    }
}
