using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public int levelNo = 0;
    void Start()
    {
        levelNo = PlayerPrefs.GetInt("levelno", 1);
        SceneManager.LoadScene(levelNo);
    }
}
