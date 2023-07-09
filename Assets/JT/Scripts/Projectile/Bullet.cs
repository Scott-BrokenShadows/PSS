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
            Destroy(this);
        }
    }

    private void OnDrawGizmos()
    {
        //var vertical = (float)Camera.main.orthographicSize;
        //var horizontal = vertical * (float)Camera.main.aspect;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3 (-BattleSystem.horizontal - BattleSystem._screenSpace.x, -BattleSystem.vertical - BattleSystem._screenSpace.y), 5);
        //Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y), 2);
    }
}
