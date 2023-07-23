using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2;
    public bool isPlayer;

    public int atk;
    public int spAtk;

    void Update()
    {
        // Movement Bullet
        transform.Translate(((isPlayer) ? Vector3.right : -Vector3.right) * speed * Time.deltaTime);

        DestroyOutside();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<BattleUnit>() != null && collision.GetComponent<BattleUnit>().isPlayer != isPlayer)
        {
            Destroy(this.gameObject);
        }
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
