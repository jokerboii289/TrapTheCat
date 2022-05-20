using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSelector",menuName = "ScriptableObject/StartBlocks",order = 1)]
public class StartBlocker : ScriptableObject
{
    public List<levelData> levelData;
}

[System.Serializable]
public class levelData
{
    public int x, z;
}
