using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFind;

namespace Model
{
    [System.Serializable]

    public class Tile : SpacialObject, IHasNeighbours<Tile>
    {
        
        public Tile(int x, int z)
            : base(x, z)
        {
            CanPass = true;
        }

        public bool CanPass { get; set; }
        public bool isobstacle = false;
        public bool isPlayerOnTop = false;
        
        public IEnumerable<Tile> AllNeighbours { get; set; }
        public IEnumerable<Tile> Neighbours { get { return AllNeighbours.Where(o => o.CanPass); } }

        public void FindNeighbours(Tile[,] gameBoard)
        {
            var neighbours = new List<Tile>();

            var possibleExits = Z % 2 == 0 ? EvenNeighbours : OddNeighbours;

            foreach (var vector in possibleExits)
            {
                var neighbourX = X + vector.X;
                var neighbourZ = Z + vector.Z;

                if (neighbourX >= 0 && neighbourX < gameBoard.GetLength(0) && neighbourZ >= 0 && neighbourZ < gameBoard.GetLength(1))
                    neighbours.Add(gameBoard[neighbourX, neighbourZ]);
            }

            AllNeighbours = neighbours;
        }

        public static List<Point> EvenNeighbours
        {
            get
            {
                return new List<Point>
                {                   
                    new Point(-1, 1),
                    new Point(0, 1),
                    new Point(1, 0),
                    new Point(0, -1),
                    new Point(-1, -1),
                    new Point(-1, 0),
                };
            }
        }

        public static List<Point> OddNeighbours
        {
            get
            {
                return new List<Point>
                {
                    new Point(0, 1),
                    new Point(1, 1),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(0, -1),
                    new Point(-1, 0),
                };
            }
        }
    }
}