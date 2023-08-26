using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DB_MenuLoader : MonoBehaviour
{
    public string sceneName;
    public bool load_Unload;

    public List<GameObject> objActive;
    public List<GameObject> objInactive;

    public void MenuLoadUnload()
    {
        if (load_Unload)
        {
            AddScene();
        }
        else
        {
            RemoveScene();
        }
    }

    public void MenuLoadSingle()
    {
        SceneManager.LoadScene($"{sceneName}", LoadSceneMode.Single);
    }

    void AddScene()
    {
        SceneManager.LoadScene($"{sceneName}", LoadSceneMode.Additive);
    }

    void RemoveScene()
    {
        SceneManager.UnloadSceneAsync($"{sceneName}");
    }

    void ObjectActive()
    {
        if (objActive != null)
        {
            foreach (var var in objActive)
            {
                var.SetActive(true);
            }
        }
    }

    void ObjectInactive()
    {
        if (objInactive != null)
        {
            foreach (var var in objInactive)
            {
                var.SetActive(false);
            }
        }
    }
}