using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assignment2
{
    abstract class Pen
    {
        // Holds the name of the pen
        protected string name;
        // Holds the height and width of the pen
        protected int height;
        protected int width;
        protected int xPos;
        protected int yPos;
        // Stores the list of dino objects in the pen
        protected List<Dino> dinoList = new List<Dino>();
        // Stores the points of the pen
        protected int points = 0;
        // Stores the side of the board the pen is (e.g. "Restroom" or "Cafeteria")
        protected string boardSide;
        // Stores the style of the pen (e.g. "Woodlands" or "Grasslands")
        protected string penStyle;
        // Stores the max number of dinos allowed in the pen
        protected int maxDinos;


        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
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
        public List<Dino> DinoList
        {
            get { return dinoList; }
            set { dinoList = value; }
        }
        public int Points
        {
            get { return points; }
            set { points = value; }
        }
        public string BoardSide
        {
            get { return boardSide; }
            set { boardSide = value; }
        }
        public string PenStyle
        {
            get { return penStyle; }
            set { penStyle = value; }
        }
        public int MaxDinos
        {
            get { return maxDinos; }
            set { maxDinos = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="boardSide"></param>
        /// <param name="penStyle"></param>
        /// <param name="maxDinos"></param>
        /// <param name="HEIGHT"></param>
        /// <param name="WIDTH"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        public Pen(string name, string boardSide, string penStyle, int maxDinos, int HEIGHT, int WIDTH, int xPos, int yPos)
        {
            this.Name = name;
            this.BoardSide = boardSide;
            this.PenStyle = penStyle;
            this.MaxDinos = maxDinos;
            this.Height = HEIGHT;
            this.Width = WIDTH;
            this.XPos = xPos;
            this.YPos = yPos;
        }

        // Assign x and y values for all the dinos in the pen based on their index in the list
        public void assignDinoPositions(int x, int y)
        {
            // Move the dinos added to pen along x coordinates for forest class
            for (int i = 0; i < dinoList.Count; i++)
            {
                dinoList[i].XPos = x + (i * 50);
                dinoList[i].YPos = y + 20;

                // check if dino out of bounds of pen, if so move to next row
                if (dinoList[i].XPos > x + Width - 50)
                {
                    dinoList[i].XPos = x + ((i % 4) * 50);
                    dinoList[i].YPos = y + 70;
                }
            }
        }

        /// <summary>
        /// Checks if the pen has a T-Rex in it
        /// </summary>
        /// <returns></returns>
        public bool HasTRex()
        {
            foreach (Dino dino in dinoList)
            {
                if (dino.Species == DinoSpecies.T_Rex)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Calculates the points of the pen based on the dinos in it
        /// </summary>
        /// <returns></returns>
        public abstract int CalculatePoints();

        /// <summary>
        /// Checks if a dino can be placed in the pen based on the pen's condition and the dino's species
        /// </summary>
        /// <param name="dino"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public abstract bool CanPlaceDino(Dino dino, PlacementCondition condition);

        /// <summary>
        /// Adds a dino to the pen if allowed
        /// </summary>
        /// <param name="dino"></param>
        public abstract void AddDino(Dino dino);

        /// <summary>
        /// Draws the pen and its contents on the graphics object
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void Draw(Graphics g)
        {
            // Draw the pen as a rectangle
            g.DrawRectangle(Pens.Black, XPos, YPos, Width, Height);
            // Draw the dino species in the pen
            for (int i = 0; i < dinoList.Count; i++)
            {
                Dino dino = dinoList[i];
                dino.Draw(g);
            }
        }
    }
}
