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
    public bulletPattern currentBulletState;
    public enum bulletPattern
    { none, single, multiSpread, multiStraight ,allDirection}
    [Min(0)] public float range = 10;
    [Min(0)] public int count = 3;
    [Min(0)] public int reloadTimeBullet = 1;
    private float timerBullet;

    // Movement Control
    Rigidbody2D _rb;
    Vector2 _movementInput;
    Vector2 _smoothMovementInput;
    Vector2 _movementInputSmoothVelocity;

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
            asset.transform.localScale = new Vector3(asset.transform.localScale.x * -1, asset.transform.localScale.y * 1, asset.transform.localScale.z * 1);
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
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            _movementInput = new Vector2(moveX, moveY).normalized;

            _smoothMovementInput = Vector2.SmoothDamp( _smoothMovementInput, _movementInput, ref _movementInputSmoothVelocity, 0.1f);

            _rb.velocity = _smoothMovementInput * _base.Speed;
        }
        else
        {
            //transform.Translate(Vector3.left * _base.Speed * Time.deltaTime);
            _rb.velocity = Vector3.left * _base.Speed;
        }
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
            case bulletPattern.single:
                SingleShot();
                break;
            case bulletPattern.multiSpread:
                MultiSpreadShot();
                break;
            case bulletPattern.multiStraight:
                MultiStraightShot();
                break;
            case bulletPattern.none:
                return;
        }
    }
    #endregion

    #region Shot
    void SingleShot()
    {
        GameObject asset = Instantiate(bulletAsset, transform.position, Quaternion.LookRotation((isPlayer) ? transform.right : -transform.right));
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

    #region Editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_base != null)
        {

        }
    }
#endif
    #endregion
}
