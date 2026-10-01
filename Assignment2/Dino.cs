using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    internal class Dino
    {
        // Holds the species of the dinosaur
        protected string species;


        /// <summary>
        /// Gets or sets the species of the dinosaur.
        /// </summary>
        public string Species
        {
            get { return species; }
            set { species = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dino"/> class.
        /// </summary>
        /// <param name="species"></param>
        public Dino(string species)
        {
            this.species = species;
        }

        public void Draw(Graphics g, int x, int y)
        {
            // Draw the dinosaur species as text
            
        }
    }
}
