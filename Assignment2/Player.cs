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

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name"></param>
        public Player(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Select dino from player hand list and return it to be placed in a pen.
        /// </summary>
        /// <param name="mainDinosList"></param>
        public Dino ChooseDino()
        {
            // Check if the player has any dinos in their hand
            if (dinoHandList.Count > 0)
            {
                foreach(Dino dino in dinoHandList)
                {
                    if(dino.IsClicked(zoo.MouseX, zoo.MouseY))
                    {
                        // If the dino is clicked, return it
                        return dino;
                    }
                }
            }
            else
            {
                // If the player has no dinos, return null
                return null;
            }
            return null;

        }

        /// <summary>
        /// Places a dino from the player's hand into the specified pen if the dino is clicked and the pen allows dinos.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="e"></param>
        public bool PlaceDino(Dino dino, Pen pen)
        {
            if(dino != null && pen.DinoAllowed)
            {
                // If the dino is clicked and the pen allows dinos, add the dino to the pen's dinoList
                pen.AddDino(dino);
                // Remove the dino from the player's hand
                dinoHandList.Remove(dino);
                return true;
            }
            else
            {
                // If the dino is not clicked or the pen does not allow dinos, do nothing
                return false;
            }
        }

        /// <summary>
        /// Draws the dino objects in the player's dinoHandList on the provided graphics object.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pictureBox"></param>
        public void DrawDinoHand(Graphics g)
        {
            // Draw the dino objects in the player's dinoHandList on the graphics object
            for (int i = 0; i < dinoHandList.Count; i++)
            {
                Dino dino = dinoHandList[i];
                dino.Draw(g);
            }
        }


    }
}
