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
        // Height of the pen's drawing area.
        protected int height;
        // Width of the pen's drawing area.
        protected int width;
        // Horizontal drawing position of the pen.
        protected int xPos;
        // Vertical drawing position of the pen.
        protected int yPos;
        // Dinosaurs currently contained by the pen.
        protected List<Dino> dinoList = new List<Dino>();
        // Points currently calculated for the pen.
        protected int points = 0;
        // Side of the board containing the pen.
        protected string boardSide;
        // Habitat style of the pen.
        protected string penStyle;
        // Maximum number of dinosaurs allowed in the pen.
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

        /// <summary>
        /// Assigns drawing positions to the dinosaurs based on their order in the pen.
        /// </summary>
        /// <param name="x">Horizontal origin of the pen.</param>
        /// <param name="y">Vertical origin of the pen.</param>
        public void assignDinoPositions(int x, int y)
        {
            int perRow = Math.Max(1, Width / 50);   // dinos that fit across the pen

            for (int i = 0; i < dinoList.Count; i++)
            {
                dinoList[i].XPos = x + (i % perRow) * 50;
                dinoList[i].YPos = y + 20 + (i / perRow) * 50;
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
        public virtual int CalculatePoints()
        {
            return CalculatePoints();
        }

        /// <summary>
        /// Calculates points using board context when a pen's scoring rule needs it.
        /// </summary>
        /// <param name="ownZoo">Board belonging to the pen's owner.</param>
        /// <param name="opponentZoo">Opponent board used for comparative scoring.</param>
        /// <returns>Points awarded by the pen.</returns>
        public virtual int CalculatePoints(Board ownZoo, Board opponentZoo)
        {
            return CalculatePoints();   // most pens don't need the boards
        }

        /// <summary>
        /// Checks if a dino can be placed in the pen based on the pen's condition and the dino's species
        /// </summary>
        /// <param name="dino">Dinosaur being tested.</param>
        /// <param name="condition">Current placement condition.</param>
        /// <param name="unrestricted">Whether the roller ignores the die condition.</param>
        /// <returns><c>true</c> when the dinosaur may be placed.</returns>
        public abstract bool CanPlaceDino(Dino dino, PlacementCondition condition, bool unrestricted);

        /// <summary>
        /// Determines whether the pen satisfies the die condition.
        /// </summary>
        /// <param name="condition">Condition rolled for the turn.</param>
        /// <param name="unrestricted">Whether the roller bypasses the condition.</param>
        /// <returns><c>true</c> when the condition is satisfied.</returns>
        protected bool MeetsPlacementCondition(PlacementCondition condition, bool unrestricted)
        {
            if (unrestricted)
                return true;

            switch (condition)
            {
                case PlacementCondition.Woodlands: return penStyle == "Woodlands";
                case PlacementCondition.Grasslands: return penStyle == "Grasslands";
                case PlacementCondition.Restrooms: return boardSide == "Right";
                case PlacementCondition.FoodCourt: return boardSide == "Left";
                case PlacementCondition.EmptyPen: return dinoList.Count == 0;
                case PlacementCondition.T_Rex: return !HasTRex();  // checked before the dino is added
                default: return false;
            }
        }

        /// <summary>
        /// Adds a dino to the pen if allowed
        /// </summary>
        /// <param name="dino">Dinosaur to add.</param>
        public abstract void AddDino(Dino dino);

        /// <summary>
        /// Draws the pen and its contents on the graphics object
        /// </summary>
        /// <param name="g">Graphics surface on which to draw.</param>
        public virtual void Draw(Graphics g)
        {
            // Draw the pen as a rectangle
            g.DrawRectangle(Pens.Black, XPos, YPos, Width, Height);
            g.DrawString(GetType().Name, SystemFonts.DefaultFont, Brushes.Black, XPos, YPos);
            // Draw the dino species in the pen
            for (int i = 0; i < dinoList.Count; i++)
            {
                Dino dino = dinoList[i];
                dino.Draw(g);
            }
        }
    }
}
