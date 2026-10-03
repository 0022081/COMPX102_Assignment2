using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Assignment2
{
    class Player
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

        /// <summary>
        /// Chooses 6 random dino objects from the mainDinosList and adds them to the player's dinoHandList.
        /// </summary>
        /// <param name="mainDinosList"></param>
        public void ChooseDinoHand(List<Dino> mainDinosList)
        {
            // Choose 6 random dino objects from the mainDinosList and add them to the player's dinoHandList
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                int index = rand.Next(mainDinosList.Count);
                Dino chosenDino = mainDinosList[index];
                // change dino's position to be in the player's hand area (e.g., x=50, y=50)
                chosenDino.XPos = 50 + (i * 40); // Adjust the x position based on the index
                chosenDino.YPos = 50; // Set the y position
                dinoHandList.Add(chosenDino);
                mainDinosList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Places a dino from the player's hand into the specified pen if the dino is clicked and the pen allows dinos.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="e"></param>
        public void PlaceDinoInPen(Pen pen, MouseEventArgs e)
        {
            // Iterate backwards so we can safely remove items while iterating
            for (int i = dinoHandList.Count - 1; i >= 0; i--)
            {
                Dino dino = dinoHandList[i];
                if (dino.IsClicked(e.X, e.Y))
                {
                    // Place the dino in the specified pen if allowed
                    if (pen.DinoAllowed)
                    {
                        pen.AddDino(dino);
                        dinoHandList.RemoveAt(i);
                    }
                    // We handled the click on one dino; stop further processing
                    break;
                }
            }

        }

        /// <summary>
        /// Draws the dino objects in the player's dinoHandList on the provided graphics object.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pictureBox"></param>
        public void DrawDinoHand(Graphics g, PictureBox pictureBox)
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
