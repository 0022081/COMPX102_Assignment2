using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Dice
    {
        // Holds the list of roll types for the dice
        protected List<string> rollTypes = new List<string> { "woodlands", "grasslands", "restrooms", "food court", "empty pen", "t-rex"};

        /// <summary>
        /// Gets or sets the list of roll types for the dice.
        /// </summary>
        public List<string> RollTypes
        {
            get { return rollTypes; }
            set { rollTypes = value; }
        }

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
        /// <returns>string of the rolled type</returns>
        public string Roll()
        {
            // Generate a random number between 0 and the number of roll types
            Random rand = new Random();
            int rollIndex = rand.Next(rollTypes.Count);
            // Return the roll type corresponding to the random number
            return rollTypes[rollIndex];
        }
    }
}
