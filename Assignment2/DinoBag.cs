using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class DinoBag
    {
        protected List<Dino> dinoList = new List<Dino>();

        /// <summary>
        /// Creates a bag of dinos by creating 8 dino objects of each species and adding them to the mainDinosList
        /// </summary>
        /// <param name="dino"></param>
        public void CreateBag(Dino dino)
        {
            // Creat 8 dino objects of each species in dinoSpecies enum
            foreach (DinoSpecies species in Enum.GetValues(typeof(DinoSpecies)))
            {
                for (int i = 0; i < 8; i++)
                {
                    Dino newDino = new Dino(species, 0, 0);
                    dinoList.Add(newDino);
                }
            }
        }

        /// <summary>
        /// Draws a specified number of dinos from the bag and returns them as a list
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Dino> Draw(int count)
        {
            // Create a new list to hold the drawn dinos
            List<Dino> drawnDinos = new List<Dino>();
            // Randomly select 'count' number of dinos from the dinoList
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                if (dinoList.Count == 0)
                {
                    break; // No more dinos to draw
                }
                int index = rand.Next(dinoList.Count);
                drawnDinos.Add(dinoList[index]);
                dinoList.RemoveAt(index); // Remove the drawn dino from the bag
            }
            return drawnDinos;
        }

        /// <summary>
        /// Returns the list of dinos in the bag
        /// </summary>
        /// <returns></returns>
        public List<Dino> GetDinos()
        {
            return dinoList;
        }
    }
}
