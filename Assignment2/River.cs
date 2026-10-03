using System;
using System.Collections.Generic;
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
    }
}
