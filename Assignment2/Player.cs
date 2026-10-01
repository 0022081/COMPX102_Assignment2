using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    internal class Player
    {

        // Holds the name of the player
        protected string name;
        // Holds whether it's the player's turn or not
        protected bool isTurn;
        // Holds the points of the player
        protected int points;
        // Holds the list of dino objects in the player's hand
        protected List<Dino> dinoHandList = new List<Dino>();

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether it's the player's turn.
        /// </summary>
        public bool IsTurn
        {
            get { return isTurn; }
            set { isTurn = value; }
        }
        /// <summary>
        /// Gets or sets the points of the player.
        /// </summary>
        public int Points
        {
            get { return points; }
            set { points = value; }
        }
        /// <summary>
        /// Gets or sets the list of dino objects in the player's hand.
        /// </summary>
        public List<Dino> DinoHandList
        {
            get { return dinoHandList; }
            set { dinoHandList = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name"></param>
        public Player(string name, bool isTurn, int points)
        {
            this.name = name;
            this.isTurn = isTurn;
            this.points = points;
        }
    }
}
