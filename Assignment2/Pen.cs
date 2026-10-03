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
        protected const int HEIGHT = 200;
        protected const int WIDTH = 200;
        protected const int XPos = 0;
        protected const int YPos = 0;
        // Stores the list of dino objects in the pen
        protected List<Dino> dinoList = new List<Dino>();
        // Stores the points of the pen
        protected int points;
        // Stores whether the dino being added is allowed or not
        protected bool dinoAllowed;
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
            get { return HEIGHT; }
        }
        public int Width
        {
            get { return WIDTH; }
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
        public bool DinoAllowed
        {
            get { return dinoAllowed; }
            set { dinoAllowed = value; }
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
        public Pen(string name, string boardSide, string penStyle, int maxDinos)
        {
            this.name = name;
            this.boardSide = boardSide;
            this.penStyle = penStyle;
            this.maxDinos = maxDinos;
            this.points = 0;
            this.dinoAllowed = true;
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
                if (dinoList[i].XPos > x + WIDTH - 50)
                {
                    dinoList[i].XPos = x + ((i % 4) * 50);
                    dinoList[i].YPos = y + 70;
                }
            }
        }

        /// <summary>
        /// Calculates the points of the pen based on the dinos in it
        /// </summary>
        /// <returns></returns>
        public abstract int CalculatePoints();

        /// <summary>
        /// Adds a dino to the pen if allowed
        /// </summary>
        /// <param name="dino"></param>
        public virtual void AddDino(Dino dino)
        {
            if (dinoList.Count < maxDinos)
            {
                dinoList.Add(dino);
                assignDinoPositions(XPos, YPos);
            }
            else
            {
                dinoAllowed = false;
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
            g.DrawRectangle(Pens.Black, XPos, YPos, WIDTH, HEIGHT);
            // Draw the name of the pen
            g.DrawString(name, new Font("Arial", 8), Brushes.Black, XPos + 2, YPos + 2);
            // Draw the dino species in the pen
            for (int i = 0; i < dinoList.Count; i++)
            {
                Dino dino = dinoList[i];
                dino.Draw(g);
            }
        }
    }
}
