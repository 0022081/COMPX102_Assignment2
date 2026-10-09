using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;

namespace Assignment2
{
    class Player
    {

        // Holds the name of the player
        protected string name;
        // Holds the points of the player
        protected int points;
        // Holds the list of dino objects in the player's hand
        protected List<Dino> dinoHandList = new List<Dino>();

        protected Board zoo;

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
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

        public Board Zoo
        {
            get { return zoo; }
            set { zoo = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name"></param>
        public Player(string name)
        {
            // Assign player name
            this.name = name;

            // Create board object
            Zoo = new Board();
        }


        /// <summary>
        /// Places a dino from the player's hand into the specified pen if the dino is clicked and the pen allows dinos.
        /// </summary>
        /// <param name="dino">Dino to place</param>
        /// <param name="pen">Target pen</param>
        public bool PlaceDino(Dino dino, Pen pen)
        {
            if (dino == null || pen == null)
                return false;

            // simple capacity check - concrete pen types can additionally enforce placement rules
            if (pen.DinoList.Count < pen.MaxDinos)
            {
                // If the dino is clicked and the pen allows dinos, add the dino to the pen's dinoList
                pen.AddDino(dino);
                // Remove the dino from the player's hand
                dinoHandList.Remove(dino);
                // update positions for dinos inside the pen so they will draw correctly
                pen.assignDinoPositions(pen.XPos, pen.YPos);
                return true;
            }

            return false;
        }
        

        /// <summary>
        /// Draws the player's hand into the provided graphics surface. This method assigns
        /// positions for each dino so hit-testing with their IsClicked method works using
        /// coordinates relative to the picture box image.
        /// </summary>
        /// <param name="g">Graphics surface for the hand image</param>
        /// <param name="areaWidth">Width of the drawing area (use the PictureBox width)</param>
        public void DrawDinoHand(Graphics g, int areaWidth)
        {
            // simple layout: space dinos horizontally with a fixed spacing
            const int spacing = 50; // matches Dino drawing size used elsewhere
            const int margin = 10;

            for (int i = 0; i < dinoHandList.Count; i++)
            {
                Dino dino = dinoHandList[i];
                // position dino inside the hand area
                dino.XPos = margin + (i * spacing);
                dino.YPos = margin;
                dino.Draw(g);
            }
        }


    }
}
