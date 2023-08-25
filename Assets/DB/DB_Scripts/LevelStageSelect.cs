using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStageSelect : MonoBehaviour
{
    [HideInInspector] public GameController cController;

    public void Start()
    {
        cController = FindObjectOfType<GameController>();
    }
    public void LevelSelect()
    {
        //this.transform.SetParent(null);
        //DontDestroyOnLoad(this);
        cController._cStage = this.gameObject;
        GoToStage();
    }

    void GoToStage()
    {
        SceneManager.LoadScene($"{this.GetComponent<LevelStage>().lStageScene}", LoadSceneMode.Additive);
    }
}
