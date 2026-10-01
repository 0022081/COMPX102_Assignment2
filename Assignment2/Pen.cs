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
        protected const int HEIGHT = 10;
        protected const int WIDTH = 10;
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

        /// <summary>
        /// Calculates the points of the pen based on the dinos in it
        /// </summary>
        /// <returns></returns>
        public abstract int calculatePoints();

        /// <summary>
        /// Adds a dino to the pen if allowed
        /// </summary>
        /// <param name="dino"></param>
        public virtual void addDino(Dino dino)
        {
            if (dinoList.Count < maxDinos)
            {
                dinoList.Add(dino);
                points = calculatePoints();
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
        public virtual void Draw(Graphics g, int x, int y)
        {
            // Draw the pen as a rectangle
            g.DrawRectangle(Pens.Black, x, y, WIDTH, HEIGHT);
            // Draw the name of the pen
            g.DrawString(name, new Font("Arial", 8), Brushes.Black, x + 2, y + 2);
            // Draw the dino species in the pen
            for (int i = 0; i < dinoList.Count; i++)
            {
                dinoList[i].Draw(g, x + 2, y + 12 + (i * 12));

            }
        }
    }
}
