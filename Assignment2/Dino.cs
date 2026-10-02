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
        public Dino(string species)
        {
            this.species = species;
        }

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

            g.FillRectangle(brush, XPos, YPos, 50, 50);

        }
    }
}
