using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assignment2
{
    class Dino
    {
        // Holds the species of the dinosaur
        protected string species;

        protected int xPos;
        protected int yPos;

        protected const int DINO_SIZE = 20;


        /// <summary>
        /// Gets or sets the species of the dinosaur.
        /// </summary>
        public string Species
        {
            get { return species; }
            set { species = value; }
        }

        public int XPos
        {
            get { return xPos; }
            set { xPos = value; }
        }

        public int YPos
        {
            get { return yPos; }
            set { yPos = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dino"/> class.
        /// </summary>
        /// <param name="species"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        public Dino(string species, int xPos, int yPos)
        {
            this.species = species;
            this.xPos = xPos;
            this.yPos = yPos;
        }

        /// <summary>
        /// Determines whether the dinosaur is clicked based on the mouse coordinates.
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <returns></returns>
        public bool IsClicked(int mouseX, int mouseY)
        {
            // Check if the mouse is clicked on the dinosaur box
            if (mouseX >= XPos && mouseX <= XPos + 50 && mouseY >= YPos && mouseY <= YPos + 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Draws the dinosaur on the provided graphics object.
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            // Draw the dinosaur species as different coloured boxes on the graphics object
            Brush brush;
            switch (species.ToLower())
            {
                case "t-rex":
                    brush = Brushes.Red;
                    break;
                case "triceratops":
                    brush = Brushes.Green;
                    break;
                case "velociraptor":
                    brush = Brushes.Blue;
                    break;
                case "stegosaurus":
                    brush = Brushes.Orange;
                    break;
                default:
                    brush = Brushes.Gray;
                    break;
            }

            g.FillRectangle(brush, XPos, YPos, DINO_SIZE, DINO_SIZE);

        }
    }
}
