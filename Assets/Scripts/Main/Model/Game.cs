using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using UnityEngine.Rendering;

namespace Model
{
    public class Game 
    {
        public Tile[,] GameBoard;
        public List<GamePiece> GamePieces = new List<GamePiece>(); //Animal Tile Pos
        public List<GamePiece> TargetgamePieces = new List<GamePiece>();// target game pice data

        public int Width;
        public int Height;

        public Game(int width, int height)
        {
            /*
            if (GamePieces != null || TargetgamePieces!=null)
            {
                GamePieces.Clear();
                TargetgamePieces.Clear();
            }
            */
            
            Width = width;
            Height = height;
            
            InitialiseGameBoard();
            //BlockOutTiles();
        }

        private void InitialiseGameBoard()
        {
            GameBoard = new Tile[Width, Height];
        
            for (var x = 0; x < Width; x++)
            {
                for (var z = 0; z < Height; z++)
                {
                    GameBoard[x, z] = new Tile(x, z);
                }
            }

            AllTiles.ToList().ForEach(o => o.FindNeighbours(GameBoard));
        }

        private void BlockOutTiles()
        {
            /*
            BoardBehaviour ss = 
            //GameBoard[0, 0].CanPass = false;
            for (int i = 0; i < BoardBehaviour.; i++)
            {
                
            }
            */
          
            GameBoard[2, 5].CanPass = false;
            GameBoard[2, 4].CanPass = false;
            GameBoard[2, 2].CanPass = false;
            GameBoard[3, 2].CanPass = false;
            GameBoard[4, 5].CanPass = false;
            GameBoard[5, 5].CanPass = false;
            GameBoard[5, 3].CanPass = false;
            GameBoard[5, 2].CanPass = false;
        }

        public IEnumerable<Tile> AllTiles
        {
            get
            {
                for (var x = 0; x < Width; x++)
                    for (var z = 0; z < Height; z++)
                        yield return GameBoard[x, z];
            }
        }

    }
}

