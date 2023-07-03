using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserCharacters
{
    public string name;
    public GameObject character;
    public int level;
    public GameObject item;
}

[System.Serializable]
public class UserItems
{
    public GameObject item;
    public int number;
}

public class GameController : MonoBehaviour
{
    [Header("Store Information")]
    [Separator()]
    public string realTime;
    public string gameplayTime;
    public int gameCurrency;
    [Separator()]
    public List<UserCharacters> listCharacters;
    public List<UserItems> listItems;

    void Awake()
    {
        // Do not destroy
        DontDestroyOnLoad(this.gameObject);
    }
}
