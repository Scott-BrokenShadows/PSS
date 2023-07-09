using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DB_Reward : MonoBehaviour
{
    public DB_RewardGacha reward;
    
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI rewardName;
    
    // Start is called before the first frame update
    void Start()
    {
        if(reward != null)
        {
        img.sprite = reward.image;
        rewardName.text = reward.itemName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
