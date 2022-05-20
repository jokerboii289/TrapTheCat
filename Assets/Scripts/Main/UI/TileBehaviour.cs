using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using DG.Tweening;
using Model;
using UnityEngine.Tilemaps;
using Tile = Model.Tile;

public class TileBehaviour : MonoBehaviour
{
    public Tile Tile;
    public Vector2 MyTilePos;
    public BoardBehaviour BoardBehaviour;
    public bool isPlayer,targetPos;
    public bool isEdgeCollider;
    public ExitTarget exittarget;
    //public List<GamePiece> Mypos;
    
    void Start()
    {
        //SetMaterial();
        CheckNeighbour();
    }
    
    public void SetMaterial()
    {
        if (Tile.CanPass == false)
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("click");
            }
           
            GameObject Wall = Instantiate(BoardBehaviour.WallPrefab, transform.position+new Vector3(0,0.2f,0), BoardBehaviour.WallPrefab.transform.rotation);
            Wall.transform.DOScaleY(0.5f, 0.2f).SetEase(Ease.InOutBounce);

            if (isEdgeCollider)
            {
                exittarget.Canpass = false;
                exittarget.Remover();
                //BoardBehaviour.OnGameStateChanged();
                //print("ssdfd");
            }
        }
        
        else
        {
            //print("Cannot break wall coming from TileBehave");
        }
    }

    void OnMouseUp()  // touch tile to create block
    {
        BoardBehaviour.Up(); //power
        if (!isPlayer && !targetPos)
        { 
            //Vibration.Vibrate(20);
            //print("Vibrate");
            Messenger<TileBehaviour>.Broadcast("Tile selected", this);          
        }
        else
        {
            print("This tile is unaccesabile");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExitTarget"))
        {
            //targetPos = true;
            exittarget = other.GetComponent<ExitTarget>();
            isEdgeCollider = true;
            exittarget.TileiambasedOn = this;
        }        
    }

    void CheckNeighbour()
    {
        if ((MyTilePos.x == 0 || MyTilePos.x == BoardBehaviour.Width - 1 ||MyTilePos.y == 0 ||MyTilePos.y == BoardBehaviour.Width - 1 )&& !isPlayer && !targetPos)
        {
            var gamepiece = new GamePiece(new Point((int)MyTilePos.x,(int)MyTilePos.y));
            BoardBehaviour.EdgeTargets.Add(this);
        }
    }
}
