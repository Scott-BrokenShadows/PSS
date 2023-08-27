using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [Header("Character Function")]
    // Get the data from the database
    [SerializeField] public HBCharacterBase _base;
    public bool isPlayer;

    // Bullet Control
    [Header("Bullet Function")]
    public GameObject bulletAsset;
    public float range = 10;
    public int count = 3;
    public int reloadTimeBullet = 1;
    private float timerBullet;

    // Keep Data this gameobject
    GameObject myGameObject;

    void Start()
    {
        if (_base) { SetUp(); }
    }

    void SetUp()
    {
        if (isPlayer)
        {
            #region Instantiate the Asset and Name them
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

    void ReloadTimerBullet()
    {
        timerBullet += Time.deltaTime;

        if (timerBullet > reloadTimeBullet)
        {
            timerBullet = 0;
            ShootBullet();
        }
    }

    void ShootBullet()
    { 
    
    }

    #region
    //#region Shot
    //void SingleShot()
    //{
    //    GameObject asset = Instantiate(bulletAsset, transform.position, Quaternion.LookRotation(transform.forward));
    //    asset.GetComponent<BattleTransferDamage>().isPlayer = (isPlayer) ? true : false;
    //    asset.GetComponent<BattleBullet>()._base = HBCharacter.Base.UnitBullet;
    //    if (!isPlayer)
    //    {
    //        if (!HBCharacter.Base.UnitBullet.AimAtPlayer)
    //        {
    //            asset.transform.rotation = this.transform.rotation;
    //        }
    //        else
    //        {
    //            // vector from this object towards the target location
    //            Vector3 vectorToTarget = BattlePlayerControl._currentTransform.transform.position - this.transform.position;
    //            // rotate that vector by -90 degrees around the Z axis
    //            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -90) * vectorToTarget;

    //            asset.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
    //        }
    //    }
    //    else
    //    {
    //        asset.transform.rotation = this.transform.rotation;
    //    }

    //    asset.GetComponent<BattleTransferDamage>().transferDamage.elements = HBCharacter.Base.Elements;
    //    asset.GetComponent<BattleTransferDamage>().transferDamage.atk = HBCharacter.Attack;
    //    asset.GetComponent<BattleTransferDamage>().transferDamage.spAtk = HBCharacter.SpAttack;
    //    asset.GetComponent<BattleTransferDamage>().transferDamage.crit = HBCharacter.Base.Critical;
    //}

    //void MultiSpreadShot()
    //{
    //    foreach (var r in GetSpread(transform, range, count))
    //    {
    //        Debug.DrawRay(r.origin, r.direction * 5);

    //        if (!isPlayer)
    //        {
    //            if (!HBCharacter.Base.UnitBullet.AimAtPlayer)
    //            {
    //                GameObject asset = Instantiate(bulletAsset, r.origin, Quaternion.Euler(0, 0, Mathf.Atan2(r.direction.y, r.direction.x) * Mathf.Rad2Deg));

    //                asset.GetComponent<BattleTransferDamage>().isPlayer = (isPlayer) ? true : false;
    //                asset.GetComponent<BattleBullet>()._base = HBCharacter.Base.UnitBullet;

    //                asset.GetComponent<BattleTransferDamage>().transferDamage.elements = HBCharacter.Base.Elements;
    //                asset.GetComponent<BattleTransferDamage>().transferDamage.atk = HBCharacter.Attack;
    //                asset.GetComponent<BattleTransferDamage>().transferDamage.spAtk = HBCharacter.SpAttack;
    //                asset.GetComponent<BattleTransferDamage>().transferDamage.crit = HBCharacter.Base.Critical;
    //            }
    //            else
    //            {
    //                GameObject asset = Instantiate(bulletAsset, r.origin, Quaternion.identity);

    //                Vector2 spreadDirection = r.direction; // Get the spread direction
    //                Vector2 targetDirection = BattlePlayerControl._currentTransform.transform.position - asset.transform.position;

    //                // Calculate the angle to look at the target while maintaining spread direction
    //                float angleToLook = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

    //                // Rotate the bullet to face the target while maintaining the spread direction
    //                asset.transform.rotation = Quaternion.Euler(0, 0, angleToLook + 180); // Add 180 degrees

    //                // Apply additional rotation based on the spread direction
    //                asset.transform.Rotate(0, 0, Mathf.Atan2(spreadDirection.y, spreadDirection.x) * Mathf.Rad2Deg);

    //                asset.GetComponent<BattleTransferDamage>().isPlayer = (isPlayer) ? true : false;
    //                asset.GetComponent<BattleBullet>()._base = HBCharacter.Base.UnitBullet;

    //                asset.GetComponent<BattleTransferDamage>().transferDamage.elements = HBCharacter.Base.Elements;
    //                asset.GetComponent<BattleTransferDamage>().transferDamage.atk = HBCharacter.Attack;
    //                asset.GetComponent<BattleTransferDamage>().transferDamage.spAtk = HBCharacter.SpAttack;
    //                asset.GetComponent<BattleTransferDamage>().transferDamage.crit = HBCharacter.Base.Critical;
    //            }
    //        }
    //        else
    //        {
    //            GameObject asset = Instantiate(bulletAsset, r.origin, Quaternion.Euler(0, 0, Mathf.Atan2(r.direction.y, r.direction.x) * Mathf.Rad2Deg));

    //            asset.GetComponent<BattleTransferDamage>().isPlayer = (isPlayer) ? true : false;
    //            asset.GetComponent<BattleBullet>()._base = HBCharacter.Base.UnitBullet;

    //            asset.GetComponent<BattleTransferDamage>().transferDamage.elements = HBCharacter.Base.Elements;
    //            asset.GetComponent<BattleTransferDamage>().transferDamage.atk = HBCharacter.Attack;
    //            asset.GetComponent<BattleTransferDamage>().transferDamage.spAtk = HBCharacter.SpAttack;
    //            asset.GetComponent<BattleTransferDamage>().transferDamage.crit = HBCharacter.Base.Critical;
    //        }
    //    }
    //}

    //void MultiStraightShot()
    //{
    //    foreach (var r in GetStraight(transform, range, count))
    //    {
    //        Debug.DrawRay(r.origin, r.direction * 5);
    //        GameObject asset = Instantiate(bulletAsset, r.origin, Quaternion.LookRotation(r.direction));
    //        asset.GetComponent<BattleTransferDamage>().isPlayer = (isPlayer) ? true : false;
    //        asset.GetComponent<BattleBullet>()._base = HBCharacter.Base.UnitBullet;

    //        if (!isPlayer)
    //        {
    //            if (!HBCharacter.Base.UnitBullet.AimAtPlayer)
    //            {
    //                asset.transform.rotation = this.transform.rotation;
    //            }
    //            else
    //            {
    //                // vector from this object towards the target location
    //                Vector3 vectorToTarget = BattlePlayerControl._currentTransform.transform.position - this.transform.position;
    //                // rotate that vector by -90 degrees around the Z axis
    //                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -90) * vectorToTarget;

    //                asset.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
    //            }
    //        }
    //        else
    //        {
    //            asset.transform.rotation = this.transform.rotation;
    //        }

    //        asset.GetComponent<BattleTransferDamage>().transferDamage.elements = HBCharacter.Base.Elements;
    //        asset.GetComponent<BattleTransferDamage>().transferDamage.atk = HBCharacter.Attack;
    //        asset.GetComponent<BattleTransferDamage>().transferDamage.spAtk = HBCharacter.SpAttack;
    //        asset.GetComponent<BattleTransferDamage>().transferDamage.crit = HBCharacter.Base.Critical;
    //    }
    //}
    //#endregion

    //#region Multi-StraightShot Function
    //Ray[] GetStraight(Transform origin, float range, int count)
    //{
    //    // vector from this object towards the target location
    //    Vector3 vectorToTarget = BattlePlayerControl._currentTransform.transform.position - this.transform.position;
    //    // rotate that vector by -90 degrees around the Z axis
    //    Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -90) * vectorToTarget;

    //    Ray[] rays = new Ray[count];
    //    float s = range / (count - 1);
    //    float a = -range / 2f;

    //    for (int i = 0; i < count; i++)
    //    {
    //        float ca = a + i * s;
    //        Vector3 rayOriginOffset = new Vector3(0, ca, 0);

    //        // Rotate the ray origin offset based on the object's rotation
    //        Vector3 rotatedOffset = ((isPlayer || !isPlayer && !HBCharacter.Base.UnitBullet.AimAtPlayer) ? origin.rotation : Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget)) * rayOriginOffset;
    //        //Vector3 rotatedOffset = origin.rotation * rayOriginOffset;

    //        rays[i] = new Ray(origin.position + rotatedOffset, origin.forward);
    //    }

    //    return rays;
    //}
    //#endregion

    //#region Multi-SpreadShot Function
    //Ray[] GetSpread(Transform origin, float range, int count)
    //{
    //    // vector from this object towards the target location
    //    Vector3 vectorToTarget = BattlePlayerControl._currentTransform.transform.position - this.transform.position;
    //    // rotate that vector by -90 degrees around the Z axis
    //    Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -90) * vectorToTarget;

    //    Ray[] rays = new Ray[count];
    //    float s = range / (count - 1);
    //    float a = -range / 2f;

    //    for (int i = 0; i < count; i++)
    //    {
    //        float ca = a + i * s;
    //        Quaternion rotation = Quaternion.AngleAxis(ca, transform.forward);
    //        rays[i] = new Ray(origin.position, rotation * origin.right);
    //    }

    //    return rays;
    //}
    //#endregion
    #endregion
}
