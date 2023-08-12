using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBullet : MonoBehaviour
{
    [LabelOverride("Bullet Base")]
    [SerializeField] public BulletBase _base;
    [SerializeField] public BattleTransferDamage transferDamage;

    // Get the Data from Bullet Base
    [ReadOnly] public float speed;

    // Check if this is a Player or Enemy
    public bool isPlayer;

    // Keep Data this gameobject
    GameObject myGameObject;

    void Start()
    {
        // Set up the data
        if (_base) { SetUp(); }
    }

    void Update()
    {
        // Movement Bullet
        transform.Translate(((isPlayer) ? Vector3.right : -Vector3.right) * speed * Time.deltaTime);

        DestroyOutside();
        DestroyEndGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<BattleUnit>() != null && collision.GetComponent<BattleUnit>().isPlayer != isPlayer)
        {
            // target unit damage calculation
            DamageDetails damageDetails = collision.transform.GetComponent<BattleUnit>().HBCharacter.TakeDamage(transferDamage);
            // HP Refection
            collision.transform.GetComponent<BattleUnit>().bUnitHud.UpdateHP();
            // Destroy Bullet
            Destroy(this.gameObject);
        }
    }

    void SetUp()
    {
        isPlayer = transferDamage.isPlayer;
        speed = _base.BulletSpeed;

        #region Instantiate the Asset and Name them
        this.gameObject.name = (_base.Name != "") ? _base.Name + ((isPlayer) ? "(Player)" : "(Enemy)") : "Default";
        myGameObject = _base.Asset;
        GameObject asset = Instantiate(_base.Asset, transform);
        asset.transform.position = transform.position;
        asset.transform.localRotation = Quaternion.Euler(0, ((isPlayer) ? 0 : 180), 0);
        asset.name = _base.Name + "Model";
        #endregion
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

    void DestroyEndGame()
    {
        if (BattleSystem.gBattleOver)
        {
            Destroy(this.gameObject);
        }
    }
}
