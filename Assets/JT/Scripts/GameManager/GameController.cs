using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] SaveLoad sData;
    [ReadOnly] [SerializeField] string currentDateTime;
    [ReadOnly] [SerializeField] string currency;

    void Start()
    {
        sData = this.GetComponent<SaveLoad>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        SetRealDateTime();
        SetCurrency();
    }

    void SetRealDateTime()
    {
        System.DateTime myTime = System.DateTime.Now;
        GameData.dateTime = myTime.ToString();
        sData.testData.dateTime = GameData.dateTime;
        currentDateTime = GameData.dateTime;
    }

    void SetCurrency()
    {
        currency = GameData.gameCurrency.ToString();
    }
}
