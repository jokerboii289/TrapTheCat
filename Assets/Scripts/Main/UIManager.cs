using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject InGamePanel, WinPanel, LostPanel;
    public GameObject Tp1, Tp2;
    
    [Space(30)] [Header("LevelText")] 
    public TextMeshProUGUI LevelText;
    public int levelNo, levelShowNo = 1;

    private void Awake()
    {
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

    void Start()
    {
        InGamePanel.SetActive(true);
        WinPanel.SetActive(false);
        LostPanel.SetActive(false);
        
        levelNo = PlayerPrefs.GetInt("levelno", 1);
        levelShowNo = PlayerPrefs.GetInt("levelshow", 1);
        LevelText.text = "LEVEL " + levelShowNo;

        if (Tp1 != null)
        {
            Tp1.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameWin)
        {
            StartCoroutine(WaitWinCall(3f));
            GameManager.Instance.isGameWin = false;
        }
        else if(GameManager.Instance.isGameFail)
        {
            StartCoroutine(WaitFailCall(1.2f));
            GameManager.Instance.isGameFail = false;
        }

        if (Input.GetMouseButtonDown(0) && Tp1 != null )
        {
            if (Tp1.activeInHierarchy)
            {
                Tp1.SetActive(false);
                Tp2.SetActive(true);
            }
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        print("NextLevel");
        levelShowNo++;
        if (levelShowNo > 12)
        {
            levelNo = Random.Range(2, 12);
        }
        else
        {
            levelNo++;
        }
        PlayerPrefs.SetInt("levelshow", levelShowNo);
        PlayerPrefs.SetInt("levelno", levelNo);
        SceneManager.LoadScene(levelNo);
    }


        IEnumerator WaitWinCall(float time)
    {
        yield return new WaitForSeconds(time);
        InGamePanel.SetActive(false);
        LostPanel.SetActive(false);
        WinPanel.SetActive(true);
        if (Tp1 != null)
        {
            Tp1.SetActive(false);
        }

        if (Tp2 != null)
        {
            Tp2.SetActive(false);
        }
    }
    
    IEnumerator WaitFailCall(float time)
    {
        yield return new WaitForSeconds(time);
        InGamePanel.SetActive(false);
        WinPanel.SetActive(false);
        LostPanel.SetActive(true);
        if (Tp1 != null)
        {
            Tp1.SetActive(false);
        }

        if (Tp2 != null)
        {
            Tp2.SetActive(false);
        }
    }
}
