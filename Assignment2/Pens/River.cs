using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class River
    {
        /// <summary>
        /// List of dinos in the river
        /// </summary>
        protected List<Dino> dinoList = new List<Dino>();
        /// <summary>
        /// Return number of dino's in river
        /// </summary>
        public int Count { get { return dinoList.Count; } }

        /// <summary>
        /// Add a dino to the river
        /// </summary>
        /// <param name="dino"></param>
        public void AddDino(Dino dino)
        {
            dinoList.Add(dino);
        }

        /// <summary>
        /// Calculate points based on the number of dinos in the river (does not care about species)
        /// </summary>
        /// <returns></returns>
        public int CalculatePoints()
        { 
            return dinoList.Count;
        }


        /// <summary>
        /// Draws the pen and its contents on the graphics object
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void Draw(Graphics g)
        {
            // Draw the pen as a rectangle
            g.DrawRectangle(Pens.Black, 300, 0, 40, 600);
            // Draw the dino species in the pen
            for (int i = 0; i < dinoList.Count; i++)
            {
                Dino dino = dinoList[i];
                dino.Draw(g);
            }
        }
    }
}
