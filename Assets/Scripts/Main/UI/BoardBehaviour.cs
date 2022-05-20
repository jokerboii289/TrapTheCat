
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Cinemachine;
using DG.Tweening.Core.Easing;
using Model;
using PathFind;
using UnityEngine.SceneManagement;
using Tile = Model.Tile;


public delegate void OnBoardChangeHandler(object sender);

public class BoardBehaviour : MonoBehaviour
{   
   
    public GameObject AnimalPrefab , TargetPos;
    
    public bool CanControl;
    public int animalX, animalY;
    public float shortestPathValue;
    public MovementControls AnimalToControl;
    public ExitTarget Currenttarget;
    public List<TileBehaviour> EdgeTargets;
    public List<float> DistanceValues;
    public List<ExitTarget> ExitTargetsList;
    public GameObject WallPrefab;
    public GameObject Tile;
    public GameObject Line;
    public GameObject SelectionObject;
    public GameObject animal;
    public StartBlocker blockedTiles;

    public int Width, Height;
    static float Spacing = 1.5f;
    private float tilexoffset = 1.8f; //1.8f
    private float tileZoffset = 1.565f; //1.565f

    private Transform TilesParent,visualpiceseParent;
    [HideInInspector]public Transform WaypointsHolder;
    GameObject[,] _gameBoard;
    public Game _game;

    public List<GameObject> _path;

    GamePiece _selectedPiece;
    GameObject _torus;

    List<GameObject> _gamePieces;

    List<GamePiece> EdgeTargetposList = new List<GamePiece>();
   // private GameObject tempAnimal;
    
    public List<GamePiece> TargetsBlocked;

    public bool isGameCompelted;

    //------------altered
    int count ;
    public static bool powerOn;
    public positionOfanimals[] animals;
    [System.Serializable]
    public class positionOfanimals
    {
        public GameObject animalPrefab;
        public int x, y;
    }


    private void Awake()
    {
        Currenttarget = null;
        _path.Clear();
        EdgeTargetposList.Clear();
        ExitTargetsList.Clear();
        
         
        TilesParent = transform.GetChild(1);
        visualpiceseParent = transform.GetChild(2);
        WaypointsHolder = transform.GetChild(3);
        isGameCompelted = false;
        //CanControl = true;
    }

    void Start()
    {
        powerOn = false;
        count = 0;
        CreateBoard();

        Messenger<TileBehaviour>.AddListener("Tile selected", OnTileSelected);
        Messenger<PieceBehaviour>.AddListener("Piece selected", OnPieceSelected);
    }
    //altered
    public void Power()
    {
        powerOn = true;
    }
    public void Up()
    {
        print("hello");
        if (powerOn)
        {
            count++;
        }
    }
    private void Update()
    {
        if (count <= 1)
        {
     
            CanControl = true;
        }
        else if (count > 1)
        {
            powerOn = false;
        }
    }
    //___________________
    void OnTileSelected(TileBehaviour tileBehaviour)
    {
        animal = GameObject.Find("animal");
       
        if (CanControl && !AnimalToControl.RunwayReached && !isGameCompelted)
        {
            TileChanged(tileBehaviour);
        }
        else
        {
            print("Control is not Returned to the player");
        }
    }

    private void MovePiece(TileBehaviour tileBehaviour)
    {
        _selectedPiece.Location = tileBehaviour.Tile.Location;
    }

    void OnPieceSelected(PieceBehaviour pieceBehaviour)
    {
        Destroy(_torus);

        _selectedPiece = pieceBehaviour == null || _selectedPiece == pieceBehaviour.Piece ? null : pieceBehaviour.Piece;

        DrawSelection();
    }

    private void DrawSelection()
    { 
        if (_selectedPiece == null)
            return;

        _torus = (GameObject) Instantiate(SelectionObject);
        _torus.transform.position = GetWorldCoordinates(_selectedPiece.Location.X, 0, _selectedPiece.Location.Z);
    }

    //private void createpieces()
    //{
    //    if (_gamePieces == null)
    //        _gamePieces = new List<GameObject>();

    //    _gamePieces.ForEach(Destroy);

    //    var startpiece = _game.GamePieces[0];
    //    // animal = CreatePiece(obj, startPiece);
    //    animal = CreatePiece(AnimalPrefab, startpiece);


    //    animal.name = "animal";
    //    GameManager.Instance.SetCamTarget(animal);
    //    //tempanimal = animal;
    //    AnimalToControl = animal.GetComponent<MovementControls>();
    //    AnimalToControl.BoardBehaviour = this;
    //    _gamePieces.Add(animal);
    //    if (_game.TargetgamePieces != null)
    //    {
    //        for (int i = 0; i < _game.TargetgamePieces.Count(); i++)
    //        {
    //            var destinationpiece = _game.TargetgamePieces[i];
    //            GameObject targetpoint = CreatePiece(TargetPos, destinationpiece);
    //            ExitTarget exittarget = targetpoint.GetComponent<ExitTarget>();
    //            exittarget.Board = this;
    //            exittarget.Line = Line;
    //            ExitTargetsList.Add(exittarget);
    //            _gamePieces.Add(targetpoint);
    //        }
    //    }
    //    else
    //    {
    //        print("targetgamepiece pos is empty");
    //    }

    //    Invoke("ongamestatechanged", 0.1f);
    //}


    ////altered
    private void CreatePieces()
    {
        if (_gamePieces == null)
            _gamePieces = new List<GameObject>();

        _gamePieces.ForEach(Destroy);

        for (int k = 0; k < animals.Count(); k++)
        {
            positionOfanimals tt = animals[k];
            GameObject obj = tt.animalPrefab;
            var startPiece = _game.GamePieces[k];
            print(_game.GamePieces.Count());
            animal = CreatePiece(obj, startPiece);
            animal.name = "animal";
            GameManager.Instance.SetCamTarget(animal);
            AnimalToControl = animal.GetComponent<MovementControls>();
            AnimalToControl.BoardBehaviour = this;
            _gamePieces.Add(animal);

            if (_game.TargetgamePieces != null)
            {
                for (int i = 0; i < _game.TargetgamePieces.Count(); i++)
                {
                    var destinationPiece = _game.TargetgamePieces[i];
                    GameObject TargetPoint = CreatePiece(TargetPos, destinationPiece);
                    ExitTarget exitTarget = TargetPoint.GetComponent<ExitTarget>();
                    exitTarget.Board = this;
                    exitTarget.Line = Line;
                    ExitTargetsList.Add(exitTarget);
                    _gamePieces.Add(TargetPoint);
                }
            }
            else
            {
                print("TargetGamePiece pos is Empty");
            }
            Invoke("OnGameStateChanged", 0.1f);
        }
    }

    public GameObject CreatePiece(GameObject Go, GamePiece piece)
    {        
        var visualPiece = (GameObject) Instantiate(Go);
        visualPiece.transform.SetParent(visualpiceseParent);
        visualPiece.transform.position = GetWorldCoordinates(piece.X, .7f, piece.Z);
        visualPiece.GetComponent<TileBehaviour>().BoardBehaviour = this;
        return visualPiece;
    }

    private void CreateBoard()
    {
        _game = new Game(Width, Height);
        _gameBoard = new GameObject[Width, Height];
        BlockOutTiles();

        for (var x = 0; x < Width; x++)
        {
            for (var z = 0; z < Height; z++)
            {
                var tile = (GameObject) Instantiate(Tile);
                _gameBoard[x, z] = tile;
                tile.transform.SetParent(TilesParent);
                //needed for edgeDetection
                tile.GetComponent<TileBehaviour>().BoardBehaviour = this;
                tile.GetComponent<TileBehaviour>().MyTilePos = new Vector2(x,z);
                //tile.transform.GetChild(0).GetComponent<TileBehaviour>().BoardBehaviour = this;

                var tileTransform = tile.transform;
               
                tileTransform.position = GetWorldCoordinates(x, 0, z);
                
                var tb = (TileBehaviour) tile.GetComponent<TileBehaviour>();

                tb.Tile = _game.GameBoard[x, z];
                StartCoroutine(SetTileInfo(tileTransform, x, z /*, pos*/));

                tb.SetMaterial();
            }
        }
       
        //CreateEdgeTargetPoints();
        Invoke("CreateEdgeTargetPoints", 0.2f);
    }

    IEnumerator SetTileInfo(Transform go, float x, float z)
    {
        yield return new WaitForSeconds(0.00001f);
        go.name = x + ", " + z;
    }

    public Vector3 GetWorldCoordinates(float x, float y, float z)
    {
        Vector3 pos;
        if (z % 2 == 0)
        {
            pos = new Vector3(x * tilexoffset, 0, z * tileZoffset);
        }
        else
        {
            pos = new Vector3(x * tilexoffset + tilexoffset / 2, 0, z * tileZoffset);
        }
        return pos;
    }

   
    void TileChanged(TileBehaviour tileBehaviour)
    {     
        if (!tileBehaviour.Tile.isobstacle && !tileBehaviour.Tile.isPlayerOnTop)
        {
            if (tileBehaviour.Tile.CanPass)
            {
                tileBehaviour.Tile.CanPass = !tileBehaviour.Tile.CanPass;
                tileBehaviour.SetMaterial();
            }
            else
            {
                OnGameStateChanged();
                   
                CanControl = false;     
                AnimalToControl.Invoke("MoveToPoint",0.2f);
                return;
            }   

            //double tap power up make cancontrol always true -hint
            CanControl = false;
            
            if (AnimalToControl != null)
            {
                OnGameStateChanged();
                AnimalToControl.WaypointCount = 1;
                AnimalToControl.Invoke("MoveToPoint",0.2f);
            }
        }
        else if (tileBehaviour.Tile.isobstacle)
        {
            print("Already Obstacle is Created");
        }
        else if (tileBehaviour.Tile.isPlayerOnTop)
        {
            print("Player is in top of me");
        }
    }
    private void BlockOutTiles()
    {
        for (int i = 0; i < blockedTiles.levelData.Count; i++)
        {
            _game.GameBoard[blockedTiles.levelData[i].x, blockedTiles.levelData[i].z].CanPass = false;
        }
    }
    void CreateEdgeTargetPoints()
    {
        CreatePlayerPos(animalX, animalY);


        for (int i = 0; i < EdgeTargets.Count; i++)
        {
            var TargetEdge = new GamePiece(new Point((int)EdgeTargets[i].MyTilePos.x, (int)EdgeTargets[i].MyTilePos.y));
            EdgeTargetposList.Add(TargetEdge);
        }

        _game.TargetgamePieces = EdgeTargetposList;
        //CreatePieces();
        Invoke("CreatePieces", 0.1f);
    }

   

    public void OnGameStateChanged()
    {     
        if (!AnimalToControl.RunwayReached)
        {    
            //print("Game-state changed");
            var sp = _game.GamePieces[0];
            var start = _game.AllTiles.Single(o => o.X == sp.Location.X && o.Z == sp.Location.Z);

            DistanceValues.Clear();
          
            for (int i = 0; i < ExitTargetsList.Count; i++)
            {
                var dp = _game.TargetgamePieces[i];
                var destination = _game.AllTiles.Single(o => o.X == dp.Location.X && o.Z == dp.Location.Z);
                //print(start);
                Func<Tile, Tile, double> distance = (node1, node2) => 1;
                Func<Tile, double> estimate = t =>
                    Math.Sqrt(Math.Pow(t.X - destination.X, 2) + Math.Pow(t.Z - destination.Z, 2));

                var path = PathFind.PathFind.FindPath(start, destination, distance, estimate);
                ExitTargetsList[i].DrawPath(path);
                calculateMinimumDistanceOut();
                print(dp.Location.X+" " + dp.Location.Z);
            }
            
            List<GameObject> Temp = _path;
            Temp.Reverse();
            AnimalToControl.PathWaypoint = Temp;      //pass path cordinates to movements control
            
            /*if (Currenttarget==null/*_path.Count == 0 #1#/*&& ExitTargetsList.Count==0#1#/* && WaypointsHolder.childCount<=0#1#)
            {
                isGameCompelted = true;
                print("Game Win");
                AnimalToControl.CageFall();
                GameManager.Instance.GameWin();
            }
            else
            {
                /*print(Currenttarget);
                print(_path.Count);#1#
                print("Not Valid Exit");
                //CanControl = true;
            }*/
        }
    }

    void calculateMinimumDistanceOut()
    {
        if (!animal)
        {
            //CanControl = true;
            return;
        }
        
        DistanceValues.Clear();
        ExitTargetsList.RemoveAll(x => x == null);
      
        
        for (int i = 0; i < ExitTargetsList.Count; i++)
        {
            float Distance = ExitTargetsList[i].CalculateDistance(AnimalToControl.Mypos);
            if (Distance > 0 && ExitTargetsList[i].Canpass)
            {
                DistanceValues.Add(Distance);
            }
            /*else if(Distance==0)
            {
                DistanceValues.Add(Distance);
            }*/
            else
            {
                continue;
            }
        }

        if (DistanceValues.Count != 0)
        {
            
            shortestPathValue = DistanceValues.Min();
            
                      
            int IndexNum = DistanceValues.FindIndex(a=> a == shortestPathValue);
            Currenttarget = ExitTargetsList[IndexNum];
            // print("Short Value is --  "+shortestPathValue);
            // print("Tile Number is  --  "+Currenttarget.TileiambasedOn.name);
            _path = Currenttarget._path;
            ExitTargetsList.ForEach(p=>p.PathShow(false));
            
            ExitTargetsList[IndexNum].PathShow( false); // Change True to False in final build  //change false to true to go debug mode
            
            print("Shortest path avialable is -- " + shortestPathValue);
            
            /*shortestPathValue = DistanceValues.Min();
            if (shortestPathValue > 0)
            {
                int IndexNum = DistanceValues.FindIndex(a=> a == shortestPathValue);
                Currenttarget = ExitTargetsList[IndexNum];
                _path = Currenttarget._path;
                ExitTargetsList.ForEach(p=>p.PathShow(false));
            
                ExitTargetsList[IndexNum].PathShow(true); // Change True to False in final build
            
                print("Calculation is Done For now");
            }
            else
            {
                print("Val is 0");
                int IndexNum = DistanceValues.FindIndex(a=> a == shortestPathValue);
                Currenttarget = ExitTargetsList[IndexNum];
            }*/
        }
        else
        {
            print("DistanceList is Empty");
            print("Game Win");
            if (UIManager.Instance.Tp2 != null)
            {
                UIManager.Instance.Tp2.SetActive(false);
            }
            AnimalToControl.CageFall();
            GameManager.Instance.GameWin();
        }
       
    }
    public void CreatePlayerPos(int X,int Z)
    {
        var anipos = new List<GamePiece>
        {
            new GamePiece(new Point(X,Z))
        };
        _game.GamePieces = anipos;       
    }

    public void TargetRemover(ExitTarget exitTarget)
    {
        ExitTargetsList.Remove(exitTarget);
        //TileBehaviour tile = exitTarget.TileiambasedOn;
        //print(tile.name);
        //EdgeTargets.Remove(tile);
        /*Currenttarget = null;
        _path.Clear();
        AnimalToControl.PathWaypoint.Clear();*/
        OnGameStateChanged();
    }

}
