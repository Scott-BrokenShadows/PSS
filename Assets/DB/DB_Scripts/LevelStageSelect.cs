using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStageSelect : MonoBehaviour
{
    public void LevelSelect()
    {
        this.transform.SetParent(null);
        DontDestroyOnLoad(this);
        GoToStage();
    }

    void GoToStage()
    {
        SceneManager.LoadScene($"{this.GetComponent<LevelStage>().lStageScene}", LoadSceneMode.Additive);
    }
}
