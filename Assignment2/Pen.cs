using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    internal class Pen
    {
        // Holds the name of the pen
        protected string name;
        // Holds the height and width of the pen
        protected const int HEIGHT = 10;
        protected const int WIDTH = 10;
        // Stores the list of dino objects in the pen
        protected List<Dino> dinoList = new List<Dino>();
        // Stores the points of the pen
        protected int points;
        // Stores whether the dino being added is allowed or not
        protected bool dinoAllowed;
        // Stores the side of the board the pen is (e.g. "Restroom" or "Cafeteria")
        protected string boardSide;
        // Stores the style of the pen (e.g. "Woodlands" or "Grasslands")
        protected string penStyle;
        // Stores the max number of dinos allowed in the pen
        protected int maxDinos;
    }
}
