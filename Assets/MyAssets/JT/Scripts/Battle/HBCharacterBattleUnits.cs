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

    // Keep Data this gameobject
    GameObject myGameObject;

    #endregion

    void Start()
    {
        if (_base) { SetUp(); }
    }

    void SetUp()
    {
        if (isPlayer)
        {
            #region Instantiate the Asset and Name them
            //Debug.Log(_base);
            //Debug.Log(_base);
            if (_base.Name != "")
                this.gameObject.name = _base.Name;
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
                this.gameObject.name = _base.Name;
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
        Movement();

        //ReloadTimerBullet();
    }

    void Movement()
    {
        if (isPlayer)
        {

        }
        else
        {
            //transform.Translate(Vector3.left * _base.Speed * Time.deltaTime);
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

                foreach (var r in GetSpread(transform, range, count))
                {
                    Debug.DrawRay(r.origin, r.direction * 5);
                    Instantiate(bulletAsset, r.origin, Quaternion.LookRotation(r.direction));
                }

                break;
            case bulletPattern.multiStraight:

                foreach (var r in GetStraight(transform, range, count))
                {
                    Debug.DrawRay(r.origin, r.direction * 5);
                    Instantiate(bulletAsset, r.origin, Quaternion.LookRotation(r.direction));
                }

                break;
            case bulletPattern.none:
                return;
        }
    }
    #endregion

    #region SingleShot Function
    void SingleShot()
    {
        if (isPlayer)
        {
            Instantiate(bulletAsset, transform.position, Quaternion.LookRotation(transform.right));
        }
        else
        {
            Instantiate(bulletAsset, transform.position, Quaternion.LookRotation(-transform.right));
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
