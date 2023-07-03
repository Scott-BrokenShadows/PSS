using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HBCharacterGatchaList : MonoBehaviour
{
    [SerializeField] List<HBCharacterChanceRecord> HBCharacterGatcha;

    [HideInInspector][SerializeField] int totalChance = 0;

    private void OnValidate()
    {
        totalChance = 0;

        foreach (var record in HBCharacterGatcha)
        {
            record.chanceLower = totalChance;
            record.chanceUpper = totalChance + record.chancePercentage;

            totalChance = totalChance + record.chancePercentage;
        }
    }

    public GameObject GetRandomWildMonster()
    {
        int randVal = Random.Range(1, 101);
        var HBCharacterRecord = HBCharacterGatcha.First(m => randVal >= m.chanceLower && randVal <= m.chanceUpper);

        return HBCharacterRecord.character;
    }

    [System.Serializable]
    public class HBCharacterChanceRecord
    {
        public GameObject character;
        public int chancePercentage;

        public int chanceLower { get; set; }
        public int chanceUpper { get; set; }
    }

}
