using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    public enum PlacementCondition
    {
        /// <summary>
        /// Dino has to be placed in a woodlands pen
        /// </summary>
        Woodlands,
        /// <summary>
        /// Dino has to be placed in a grasslands pen
        /// </summary>
        Grasslands,
        /// <summary>
        /// Dino has to be placed in a pen on the restroom side of the board
        /// </summary>
        Restrooms,
        /// <summary>
        /// Dino has to be placed in a pen on the food court side of the board
        /// </summary>
        FoodCourt,
        /// <summary>
        /// Dino has to be placed in an empty pen
        /// </summary>
        EmptyPen,
        /// <summary>
        /// Dino cannot be placed in a pen with another T-Rex
        /// </summary>
        T_Rex
    }
}
