using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;

public class ExitTarget : MonoBehaviour
{
    public BoardBehaviour Board;
    public bool Canpass;
    public float DistanceValue;
    public List<GameObject> _path;
    public GameObject Line;
    public MeshRenderer mesh;
    public TileBehaviour TileiambasedOn;
    public Vector2 currentPlayerTile;

    private void Awake()
    {
        Canpass = true;
    }

    private void Start()
    {
        //PathShow(false);
    }

    public void DrawPath(IEnumerable<Tile> path)
    {
        if (_path == null)
            _path = new List<GameObject>();

        _path.ForEach(Destroy);
        _path = new List<GameObject>();
        //path.ToList().ForEach(CreateLine);
        
        if (path != null)
        {
            path.ToList().ForEach(CreateLine);
        }
        else
        {
            if (Board.ExitTargetsList.Count > 0)
            {
                print("impossible to reach this point");
                Board.CanControl = true;
                //Board.OnGameStateChanged();
            }
        }    
    }
    
    void CreateLine(Tile tile)
    {
        //print("Line Created");
        var line = (GameObject) Instantiate(Line);
        line.transform.SetParent(Board.WaypointsHolder);
        line.transform.position = Board.GetWorldCoordinates(tile.Location.X, .375f, tile.Location.Z);
        _path.Add(line);
    }
    public float CalculateDistance(Vector2 animalPos)
    {
        currentPlayerTile = animalPos;
        DistanceValue = _path.Count;
        /*float Distance = Vector3.Distance(Start.position, transform.position);
        DistanceValue = Distance;*/
        return DistanceValue;
    }

    public void PathShow(bool state)
    {
        if (_path != null)
        {
            foreach (var point in _path)
            {
                point.GetComponent<MeshRenderer>().enabled = state;
            }
            mesh.enabled = state;
        }
    }

   
    public void Remover()
    {
        if (Board.ExitTargetsList.Contains(this))
        {
            print("Remover Working");
            Board.TargetRemover(this);
            //Board.OnGameStateChanged();
        }
        else
        {
            print("Remover no no    ");
        }
    }
}
