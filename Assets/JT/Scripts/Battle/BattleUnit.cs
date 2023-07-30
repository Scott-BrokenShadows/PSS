using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    // Inspector------------------------------------------------------------------------

    #region Inspector
    // Get the data from the database
    [SerializeField] public HBCharacterBase _base;
    [Range(0, 100)] [SerializeField] public int level;
    public bool isPlayer;

    // Get the data from HBCharacter
    public HBCharacter HBCharacter { get; set; }
    public BattleUnitHud bUnitHud;

    // Bullet Control
    [Separator]
    [Header ("Bullet Function")]
    public GameObject bulletAsset;
    [HideInInspector] [Min(0)] public float range;
    [HideInInspector] [Min(0)] public int count;
    [HideInInspector] [Min(0)] public float reloadTimeBullet;
    private float timerBullet;

    // Player Movement Control
    Rigidbody2D _rb;

    // Player Unit Controls
    [Separator]
    [Header("Unit Function")]
    public bool subUnit;

    // Enemy Unit Controls
    [ReadOnly] public int sLane;

    // Keep Data this gameobject
    GameObject myGameObject;
    GameObject mySkillActive;

    #endregion

    // Start & Update------------------------------------------------------------------------

    #region Start & Update
    void Start()
    {
        // Set up the data
        if (_base) { SetUp(); }
    }

    void Update()
    {
        // Shoot after timer
        ReloadTimerBullet();
        // If enemy outside of bound Destroy
        if (!isPlayer) { DestroyOutside(); }
        // When HP reaches 0 Destroy
        DestroyDeath();
    }

    void FixedUpdate()
    {
        // Movemnt function
        Movement();
    }
    #endregion

    // Start Up Mechanics------------------------------------------------------------------------

    #region Set Up
    void SetUp()
    {
        // Get the data level
        HBCharacter = new HBCharacter(_base, level);
        // Get the Rigidbody2D
        _rb = GetComponent<Rigidbody2D>();
        // Player set to Player Controller position
        if (isPlayer && !subUnit) { transform.position = BattlePlayerControl._currentTransform.position; }
        // Set the Unit Data
        bUnitHud.SetData(HBCharacter);

        #region Instantiate the Asset and Name them
        this.gameObject.name = (HBCharacter.Base.Name != "") ? HBCharacter.Base.Name + ((isPlayer) ? "(Player)" : "(Enemy)") : "Default";
        myGameObject = HBCharacter.Base.Asset;
        GameObject asset = Instantiate(HBCharacter.Base.Asset, transform);
        asset.transform.position = transform.position;
        asset.transform.localRotation = Quaternion.Euler(0, ((isPlayer) ? 0 : 180), 0);
        asset.name = HBCharacter.Base.Name + "Model";
        #endregion

        // Get Data from Bullet
        range = HBCharacter.Base.UnitBullet.BulletRange;
        count = HBCharacter.Base.UnitBullet.BulletCount;
        reloadTimeBullet = HBCharacter.Base.UnitBullet.BulletReload;
    }
    #endregion

    // Movement Mechanics------------------------------------------------------------------------

    #region Player and Enemy Movement
    void Movement()
    {
        if (isPlayer)
        {
            switch (HBCharacter.Base.UnitType)
            {
                case UnitType.Character:
                    if (!subUnit) { FrontSubMovement(); } else { BackSubMovement(); }
                    break;
                case UnitType.Minion:
                    break;
                case UnitType.Boss:
                    break;
            }
        }
        else
        {
            switch (HBCharacter.Base.UnitType)
            {
                case UnitType.Character:
                    MinionMovement();
                    break;
                case UnitType.Minion:
                    MinionMovement();
                    break;
                case UnitType.Boss:
                    MinionMovement();
                    break;
            }
        }
    }
    #endregion

    #region Enemy Character and Minion Movement Function
    void MinionMovement()
    {
        if (sLane <= 0)
        {
            // Enemy Minions Movement
            _rb.velocity = Vector3.left * ((HBCharacter.Base.Speed / 999f) * 15f);
        }
        else
        {
            if (transform.position.x > BattleSystem.Remap(BattleSystem._laneSlowDown[sLane - 1], 0, 1, -BattleSystem.horizontal, BattleSystem.horizontal))
            {
                // Enemy Minions Movement
                _rb.velocity = Vector3.left * ((HBCharacter.Base.Speed / 999f) * 15f);
            }
            else
            {
                // Enemy Minions Stop Movement
                _rb.velocity = Vector3.left * 0;
                transform.position = new Vector3(BattleSystem.Remap(BattleSystem._laneSlowDown[sLane - 1], 0, 1, -BattleSystem.horizontal, BattleSystem.horizontal), transform.position.y);
            }
        }
    }
    #endregion

    #region Player Character Movement Function
    void FrontSubMovement()
    {
        // Follow the Player Control
        transform.position = BattlePlayerControl._currentTransform.position;
    }

    void BackSubMovement()
    {

    }
    #endregion

    // Shooting Mechanics------------------------------------------------------------------------

    #region Reloader Timer Shot
    void ReloadTimerBullet()
    {
        timerBullet += Time.deltaTime;

        if (timerBullet > reloadTimeBullet)
        {
            // reset timer
            timerBullet = 0;
            // Do Stuff
            ShootBullet();
        }
    }
    #endregion

    #region Shoot Bullet
    void ShootBullet()
    {
        switch (HBCharacter.Base.UnitBullet.BulletType)
        {
            case BulletType.SingleShot:
                SingleShot();
                break;
            case BulletType.MultiSpreadShot:
                MultiSpreadShot();
                break;
            case BulletType.MultiLaneShot:
                MultiStraightShot();
                break;
        }
    }
    #endregion

    #region Shot
    void SingleShot()
    {
        GameObject asset = Instantiate(bulletAsset, transform.position, Quaternion.LookRotation(transform.forward));
        asset.GetComponent<BattleBullet>().isPlayer = (isPlayer) ? true : false;
        asset.GetComponent<BattleBullet>()._base = HBCharacter.Base.UnitBullet;

        asset.GetComponent<BattleTransferDamage>().transferDamage.elements = HBCharacter.Base.Elements;
        asset.GetComponent<BattleTransferDamage>().transferDamage.atk = HBCharacter.Attack;
        asset.GetComponent<BattleTransferDamage>().transferDamage.spAtk = HBCharacter.SpAttack;
        asset.GetComponent<BattleTransferDamage>().transferDamage.crit = HBCharacter.Base.Critical;
    }

    void MultiSpreadShot()
    {
        foreach (var r in GetSpread(transform, range, count))
        {
            Debug.DrawRay(r.origin, r.direction * 5);
            GameObject asset = Instantiate(bulletAsset, r.origin, Quaternion.LookRotation(r.direction));
            asset.GetComponent<BattleBullet>().isPlayer = (isPlayer) ? true : false;
            asset.GetComponent<BattleBullet>()._base = HBCharacter.Base.UnitBullet;

            asset.GetComponent<BattleTransferDamage>().transferDamage.elements = HBCharacter.Base.Elements;
            asset.GetComponent<BattleTransferDamage>().transferDamage.atk = HBCharacter.Attack;
            asset.GetComponent<BattleTransferDamage>().transferDamage.spAtk = HBCharacter.SpAttack;
            asset.GetComponent<BattleTransferDamage>().transferDamage.crit = HBCharacter.Base.Critical;
        }
    }

    void MultiStraightShot()
    {
        foreach (var r in GetStraight(transform, range, count))
        {
            Debug.DrawRay(r.origin, r.direction * 5);
            GameObject asset = Instantiate(bulletAsset, r.origin, Quaternion.LookRotation(r.direction));
            asset.GetComponent<BattleBullet>().isPlayer = (isPlayer) ? true : false;
            asset.GetComponent<BattleBullet>()._base = HBCharacter.Base.UnitBullet;

            asset.GetComponent<BattleTransferDamage>().transferDamage.elements = HBCharacter.Base.Elements;
            asset.GetComponent<BattleTransferDamage>().transferDamage.atk = HBCharacter.Attack;
            asset.GetComponent<BattleTransferDamage>().transferDamage.spAtk = HBCharacter.SpAttack;
            asset.GetComponent<BattleTransferDamage>().transferDamage.crit = HBCharacter.Base.Critical;
        }
    }
    #endregion

    #region Multi-StraightShot Function
    Ray[] GetStraight(Transform origin, float range, int count)
    {
        if (isPlayer)
        {
            Ray[] rays = new Ray[count];
            float s = range / (count - 1);
            float a = -range / 2f;
            for (int i = 0; i < count; i++)
            {
                float ca = a + i * s;
                rays[i].origin = new Vector3(0, ca, 0) + origin.position;
                rays[i].direction = origin.right;
            }
            return rays;
        }
        else
        {
            Ray[] rays = new Ray[count];
            float s = range / (count - 1);
            float a = -range / 2f;
            for (int i = 0; i < count; i++)
            {
                float ca = a + i * s;
                rays[i].origin = new Vector3(0, ca, 0) + origin.position;
                rays[i].direction = -origin.right;
            }
            return rays;
        }
    }
    #endregion

    #region Multi-SpreadShot Function
    Ray[] GetSpread(Transform origin, float range, int count)
    {
        if (isPlayer)
        {
            Ray[] rays = new Ray[count];
            float s = range / (count - 1);
            float a = -range / 2f;
            for (int i = 0; i < count; i++)
            {
                rays[i].origin = origin.position;
                float ca = a + i * s;
                Quaternion rotation = Quaternion.AngleAxis(ca, transform.forward);
                rays[i].direction = rotation * origin.right;
            }
            return rays;
        }
        else
        {
            Ray[] rays = new Ray[count];
            float s = range / (count - 1);
            float a = -range / 2f;
            for (int i = 0; i < count; i++)
            {
                rays[i].origin = origin.position;
                float ca = a + i * s;
                Quaternion rotation = Quaternion.AngleAxis(ca, transform.forward);
                rays[i].direction = rotation * -origin.right;
            }
            return rays;
        }
    }
    #endregion

    // Skill Mechanics------------------------------------------------------------------------

    public void SkillActivation()
    {
        if (isPlayer && subUnit)
        {
            #region Instantiate the Asset and Name them
            mySkillActive = HBCharacter.Base.UnitSkill.Asset;
            GameObject assetSkill = Instantiate(HBCharacter.Base.UnitSkill.Asset, transform);
            assetSkill.transform.SetParent(null);
            assetSkill.transform.position = (HBCharacter.Base.UnitSkill.SPos) ? transform.position : new Vector3(0f, 0f, 0f); 
            assetSkill.name = HBCharacter.Base.UnitSkill.Name + "(PlayerSkill)";
            #endregion
        }
    }

    // Destroy Mechanics------------------------------------------------------------------------

    #region Destroy OutofBound Function
    void DestroyOutside()
    {
        if (transform.position.x < -BattleSystem.horizontal - BattleSystem._screenSpace.x ||
            transform.position.x > BattleSystem.horizontal + BattleSystem._screenSpace.x ||
            transform.position.y < -BattleSystem.vertical - BattleSystem._screenSpace.y ||
            transform.position.y > BattleSystem.vertical + BattleSystem._screenSpace.y)
        {
            Destroy(this.gameObject);
        }
    }

    void DestroyDeath()
    {
        if (HBCharacter.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
