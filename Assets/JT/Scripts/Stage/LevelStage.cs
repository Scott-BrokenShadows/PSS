using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [HideInInspector] public string name;

    // Get the data from the database
    [SerializeField] public HBCharacterBase _base;
    public HBCharacter HBCharacter { get; set; }
    [Range(0, 100)] public int lv;
    [Header("Instantiate Location and Timer")]
    public float timer;
    [Min(1)] public int row;
    public float position;
    public int stopLane;
    [HideInInspector] public float cTimer;
    [HideInInspector] public bool callOnce;
}

[System.Serializable]
public class RewardLevelStage
{
    public int rCurrency;
    public int rEXP;
}

public class LevelStage : MonoBehaviour
{
    [LabelOverride("StageScene")]
    public string lStageScene;

    public Sprite backgroundImage;
    public Color backgroundImageColor;

    [Min(0)] public int gridRows = 5;

    public List<LevelWave> levelWave;

    #region Just Naming to make other people who read this would understand
    private void OnValidate()
    {
        for (int i = 0; i < levelWave.Count; i++)
        {
            levelWave[i].name = $"Wave {(i + 1).ToString()}";

            for (int b = 0; b < levelWave[i].EnemyList.Count; b++)
            {
                if (levelWave[i].EnemyList[b]._base != true) { levelWave[i].EnemyList[b].name = $"Enemy Slot {(b + 1).ToString()}"; }
            }
        }
    }
    #endregion

    public RewardLevelStage rewards;

    public List<LevelWave> LevelWave { get { return levelWave; } }
}
