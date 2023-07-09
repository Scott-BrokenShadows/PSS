using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2;
    [HideInInspector] public bool isPlayer;

    void Update()
    {
        // Movement Bullet
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        DestroyOutside();
    }

    void DestroyOutside()
    {
        if (transform.position.x < -BattleSystem.horizontal - BattleSystem._screenSpace.x || 
            transform.position.x >  BattleSystem.horizontal + BattleSystem._screenSpace.x || 
            transform.position.y < -BattleSystem.vertical   - BattleSystem._screenSpace.y || 
            transform.position.y >  BattleSystem.vertical   + BattleSystem._screenSpace.y)
        {
            //Debug.Log("Outside!");
            Destroy(this.gameObject);
        }
    }
}
