using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelWave
{
    [HideInInspector] public string name;
    public float countDownTimer;
    public List<HBEnemyList> EnemyList;
}

[System.Serializable]
public class HBEnemyList
{
    // Get the data from the database
    [SerializeField] public UserCharactersSlot _base;
    public SpawnHBEnemy spawnPoints;
}

[System.Serializable]
public class SpawnHBEnemy
{
    [Min(1)] public int row;
    public float position;
    public float timer;
}

public class LevelStage : MonoBehaviour
{
    [LabelOverride("StageScene")]
    public string lStageScene;
    [Min(0)] public int gridRows = 5;
    public LevelWave levelWave;

    public string LStageScene { get { return lStageScene; } }
    public int GridRows { get { return gridRows; } }
    public LevelWave LevelWave { get { return levelWave; } }
}
