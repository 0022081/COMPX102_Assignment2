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
        // Dinosaurs currently placed in the river.
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
        /// Counts dinosaurs of the specified species in the river.
        /// </summary>
        /// <param name="species">Species to count.</param>
        /// <returns>Number of matching dinosaurs.</returns>
        public int CountSpecies(DinoSpecies species)
        {
            return dinoList.Count(d => d.Species == species);
        }

        /// <summary>
        /// Draws a visible green border around the river to show it is a valid placement.
        /// </summary>
        /// <param name="g">Graphics surface on which to draw.</param>
        public void DrawHighlight(Graphics g)
        {
            // System.Drawing.Pen must be fully qualified because this project has its own Pen class.
            // The border is inset slightly so it is not clipped by the edges of the board image.
            using (System.Drawing.Pen highlight = new System.Drawing.Pen(Color.LimeGreen, 4))
            {
                g.DrawRectangle(highlight, 302, 2, 36, 596);
            }
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
