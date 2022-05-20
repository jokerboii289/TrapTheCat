using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Model;

public class MovementControls : MonoBehaviour
{
   private Transform t;
   private Animator animator;
   public BoardBehaviour BoardBehaviour;
   public List<GameObject> PathWaypoint;
   public int WaypointCount;
   public Transform Cage;
   public bool RunwayReached;
   public int EneX, EneZ;
   public Vector2 Mypos;
   
   
   public void Start()
   {
      t = transform;
      PathWaypoint.Clear();
      RunwayReached = false;
      WaypointCount = 1;
      animator = GetComponent<Animator>();
      Cage.gameObject.SetActive(false);
      BoardBehaviour.CanControl = true;

   }

   public void LateUpdate()
   {
      if(RunwayReached)
      {
         animator.SetTrigger("Run");
         transform.position+=transform.forward*1f*Time.deltaTime;
      }
   }

    public void MoveToPoint()
    {
        if (WaypointCount <= PathWaypoint.Count /*&& WaypointCount!=null*/ && !RunwayReached && !BoardBehaviour.powerOn)
        {
            Vector3 pointpos;

            pointpos = PathWaypoint[WaypointCount].transform.position;

            //for(int i=0;i<PathWaypoint.Count();i++)
            //{
            //    print(PathWaypoint[i].transform.position.x+" "+ PathWaypoint[i].transform.position.x);
            //}
            /*if (BoardBehaviour.shortestPathValue > 0)
            {
               pointpos = PathWaypoint[WaypointCount].transform.position;
            }
            else
            {
               pointpos = BoardBehaviour.Currenttarget.transform.position;
            }*/

            t.DOLookAt(pointpos, 0.2f).OnComplete(() =>
            {
              //t.DOMove(pointpos, 0.1f).OnComplete(() =>
              animator.SetTrigger("Fly");
                t.DOLocalJump(new Vector3(pointpos.x, pointpos.y, pointpos.z), 1.35f, 1, 0.4f, false).OnComplete(() =>
               {
                   if (AudioManager.instance)
                   {
                       AudioManager.instance.Play("squeeze");
                   }
                   animator.SetTrigger("Bounce");

                   Instantiate(GameManager.Instance.JumpSmoke, transform.position + new Vector3(0, 0.2f, 0), GameManager.Instance.JumpSmoke.transform.rotation);
                   BoardBehaviour.CreatePlayerPos(EneX, EneZ);
                   BoardBehaviour.OnGameStateChanged();
                   BoardBehaviour.CanControl = true;
               });
            });
        }
        else if (RunwayReached)
        {
            print("Tata Bye - Game Over");
        }
        else if (PathWaypoint == null)
        {
            print("Path is Null serach for next nearset point");
        }
        else
        {
            WaypointCount = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Tile"))
      {
         /*string Rawname = other.transform.parent.name;
         var array = Rawname.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray();
         EneX = array[0];
         EneZ = array[1];*/
         TileBehaviour tb = other.GetComponent<TileBehaviour>();
         EneX =(int) tb.MyTilePos.x;
         EneZ =(int) tb.MyTilePos.y;
         Mypos = new Vector2(EneX,EneZ);
         tb.Tile.isPlayerOnTop = true;
         if (tb.isEdgeCollider)
         {
            StartCoroutine(WaitRunAway(0.1f));
         }
         else
         {
            //print("End Not Reached");
         }
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.CompareTag("Tile"))
      {
         TileBehaviour tb = other.GetComponent<TileBehaviour>();
         tb.Tile.isPlayerOnTop = false;
      }
   }

   IEnumerator WaitRunAway(float time)
   {
      yield return new WaitForSeconds(time);
      BoardBehaviour.isGameCompelted = true;
      GameManager.Instance.GameFail();
      GameObject Tail = Instantiate(GameManager.Instance.RunningSmoke, transform.position + new Vector3(0, 0.2f, 0),
         transform.rotation);
      Tail.transform.SetParent(transform);
      Tail.GetComponent<ParticleSystem>().Play();
      if(AudioManager.instance)
      {
         Vibration.Vibrate(20);
      }
      RunwayReached = true;
   }

   public void CageFall()
   {
      print("CageFallCalled");
      Cage.gameObject.SetActive(true);
      Cage.transform.DOMoveY(1f, 2f, false).SetEase(Ease.InOutCubic).OnComplete(() =>
      {
          if (AudioManager.instance)
          {
              Vibration.Vibrate(20);
          }
          print("cage Fall Done");
         GameManager.Instance.StartCam.gameObject.SetActive(true);
      });
   }
   
}
