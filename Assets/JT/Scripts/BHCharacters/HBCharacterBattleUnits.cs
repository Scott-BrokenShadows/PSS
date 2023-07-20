using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HBCharacterBattleUnits : MonoBehaviour
{
    #region Inspector
    // Get the data from the database
    [SerializeField] public HBCharacterBase _base;
    public bool isPlayer;

    // Bullet Control
    [Separator]
    [Header ("Bullet Function")]
    public GameObject bulletAsset;
    public BulletPattern currentBulletState;
    public enum BulletPattern
    { none, single, multiSpread, multiStraight ,allDirection}
    [Min(0)] public float range = 10;
    [Min(0)] public int count = 3;
    [Min(0)] public float reloadTimeBullet = 1;
    private float timerBullet;

    // Player Movement Control
    Rigidbody2D _rb;
    public Vector2 _movementInput;
    Vector2 _smoothMovementInput;
    Vector2 _movementInputSmoothVelocity;

    // Player Unit Controls
    [Separator]
    [Header("Unit Function")]
    [SerializeField] PlayerUnit pUnit;
    public enum PlayerUnit
    { frontSubUnit, backSubUnit }

    // Enemy Unit Controls
    EnemyUnit eUnit;
    public enum EnemyUnit
    { characterUnit, minionUnit }

    // Keep Data this gameobject
    GameObject myGameObject;

    #endregion

    void Start()
    {
        if (_base) { SetUp(); }
    }

    void SetUp()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (isPlayer)
        {
            transform.position = BattlePlayerControl._currentTransform.position;

            #region Instantiate the Asset and Name them
            //Debug.Log(_base);
            //Debug.Log(_base);
            if (_base.Name != "")
                this.gameObject.name = _base.Name + "(Player)";
            else
                this.gameObject.name = "Default";

            myGameObject = _base.Asset;
            GameObject asset = Instantiate(_base.Asset, transform);
            asset.transform.position = transform.position;
            asset.name = _base.Name + "Model";
            #endregion
        }
        else
        {
            #region Instantiate the Asset and Name them
            //Debug.Log(_base);
            //Debug.Log(_base);
            if (_base.Name != "")
                this.gameObject.name = _base.Name + "(Enemy)";
            else
                this.gameObject.name = "Default";

            myGameObject = _base.Asset;
            GameObject asset = Instantiate(_base.Asset, transform);
            asset.transform.position = transform.position;
            asset.transform.localRotation = Quaternion.Euler(0,180,0);
            asset.name = _base.Name + "Model";
            #endregion
        }
    }

    void Update()
    {
        ReloadTimerBullet();
        if (!isPlayer) { DestroyOutside(); }
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (isPlayer)
        {
            switch (pUnit)
            {
                case PlayerUnit.frontSubUnit:
                    FrontSubMovement();
                    break;
                case PlayerUnit.backSubUnit:
                    BackSubMovement();
                    break;
            }
        }
        else
        {
            if (transform.position.x > BattleSystem.Remap(BattleSystem._laneSlowDown[1], 0, 1, -BattleSystem.horizontal, BattleSystem.horizontal))
            {
                // Enemy Minions Movement
                _rb.velocity = Vector3.left * _base.Speed;
            }
            else
            {
                _rb.velocity = Vector3.left * 0;
            }
        }
    }

    void FrontSubMovement()
    {
        Vector3 moveDirection = new Vector3(BattlePlayerControl._currentTransform.position.x, BattlePlayerControl._currentTransform.position.y)
                              - new Vector3(transform.position.x, transform.position.y);

        // To Follow You Straight
        Vector3 movementDirection = moveDirection - (BattlePlayerControl._currentTransform.transform.forward * BattlePlayerControl._targetRange);
        _movementInput = new Vector2(movementDirection.x, movementDirection.y);

        //_smoothMovementInput = Vector2.SmoothDamp(_smoothMovementInput, _movementInput, ref _movementInputSmoothVelocity, 0.1f);
        _rb.velocity = _movementInput * _base.Speed;
    }

    void BackSubMovement()
    {

    }

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

    #region Shoot Bullet
    void ShootBullet()
    {
        switch (currentBulletState)
        {
            case BulletPattern.single:
                SingleShot();
                break;
            case BulletPattern.multiSpread:
                MultiSpreadShot();
                break;
            case BulletPattern.multiStraight:
                MultiStraightShot();
                break;
            case BulletPattern.none:
                return;
        }
    }
    #endregion

    #region Shot
    void SingleShot()
    {
        GameObject asset = Instantiate(bulletAsset, transform.position, Quaternion.LookRotation(transform.forward));
        asset.GetComponent<Bullet>().isPlayer = (isPlayer) ? true : false;
    }

    void MultiSpreadShot()
    {
        foreach (var r in GetSpread(transform, range, count))
        {
            Debug.DrawRay(r.origin, r.direction * 5);
            GameObject asset = Instantiate(bulletAsset, r.origin, Quaternion.LookRotation(r.direction));
            asset.GetComponent<Bullet>().isPlayer = (isPlayer) ? true : false;
        }
    }

    void MultiStraightShot()
    {
        foreach (var r in GetStraight(transform, range, count))
        {
            Debug.DrawRay(r.origin, r.direction * 5);
            GameObject asset = Instantiate(bulletAsset, r.origin, Quaternion.LookRotation(r.direction));
            asset.GetComponent<Bullet>().isPlayer = (isPlayer) ? true : false;
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

    void DestroyOutside()
    {
        if (transform.position.x < -BattleSystem.horizontal - BattleSystem._screenSpace.x ||
            transform.position.x > BattleSystem.horizontal + BattleSystem._screenSpace.x ||
            transform.position.y < -BattleSystem.vertical - BattleSystem._screenSpace.y ||
            transform.position.y > BattleSystem.vertical + BattleSystem._screenSpace.y)
        {
            //Debug.Log("Outside!");
            Destroy(this.gameObject);
        }
    }
}
