using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Cameras")] 
    public CinemachineVirtualCamera CmGameplay;
    public CinemachineVirtualCamera StartCam;
    [Space(20)] 
    
    
    
    [Header("Particle Effects")]
    public GameObject JumpSmoke;
    public GameObject RunningSmoke;
    [HideInInspector]public GameObject Animal;
    [HideInInspector]public bool isGameWin, isGameFail;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        if (Instance != null)
        {
            Destroy(Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        isGameWin = isGameFail = false;
        StartCam.gameObject.SetActive(true);
        if (UIManager.Instance.levelShowNo==1)
        {
            UIManager.Instance.Tp1.SetActive(true);
        }
        //Cage.SetActive(false);
    }

    public void SetCamTarget(GameObject Animal)
    {
        StartCam.gameObject.SetActive(false);
        CmGameplay.Follow = Animal.transform;
        CmGameplay.LookAt = Animal.transform;
    }

    public void GameWin()
    {
        if (!isGameWin && !isGameFail)
        {
            isGameWin = true;
        }
        //StartCam.gameObject.SetActive(true);
        //GameObject cage  = MainBoard.CreatePiece(Cage,MainBoard.AnimalToControl)
        //GameObject cage = Instantiate(Cage,)
    }

    public void GameFail()
    {
        if (!isGameWin && !isGameFail)
        {
            isGameFail = true;
            print("GameFail");
            CmGameplay.m_Follow = null;
            CmGameplay.m_LookAt = null;
            /*CmGameplay.Follow = null;
            CmGameplay.LookAt = null;*/
        }
    }
}
