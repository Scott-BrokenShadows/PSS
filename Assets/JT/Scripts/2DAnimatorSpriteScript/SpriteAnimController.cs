using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimController : MonoBehaviour
{
    public float MoveX;
    public float MoveY;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool isDeath;
    [HideInInspector] public bool stopAnim = false;
    public bool flipLR;

    [Header ("Idle Animation")]
    [SerializeField] List<Sprite> DownIdleSprites;
    [SerializeField] List<Sprite> UpIdleSprites;
    [SerializeField] List<Sprite> RightIdleSprites;
    [SerializeField] List<Sprite> LeftIdleSprites;

    [Header("Move Animation")]
    [SerializeField] List<Sprite> DownMoveSprites;
    [SerializeField] List<Sprite> UpMoveSprites;
    [SerializeField] List<Sprite> RightMoveSprites;
    [SerializeField] List<Sprite> LeftMoveSprites;

    [Header("Attack Animation")]
    [SerializeField] List<Sprite> DownAttackSprites;
    [SerializeField] List<Sprite> UpAttackSprites;
    [SerializeField] List<Sprite> RightAttackSprites;
    [SerializeField] List<Sprite> LeftAttackSprites;

    [Header("Hit Animation")]
    Color defaultColor;
    [SerializeField] Color hitColor = Color.red;
    public float hitTimer = 0.1f;
    float moveTimer;

    [Header("Death Animation")]
    [SerializeField] List<Sprite> DeathSprites;

    // States
    SpriteAnimator DownIdleAnim;
    SpriteAnimator UpIdleAnim;
    SpriteAnimator RightIdleAnim;
    SpriteAnimator LeftIdleAnim;

    SpriteAnimator DownMoveAnim;
    SpriteAnimator UpMoveAnim;
    SpriteAnimator RightMoveAnim;
    SpriteAnimator LeftMoveAnim;

    SpriteAnimator DownAttackAnim;
    SpriteAnimator UpAttackAnim;
    SpriteAnimator RightAttackAnim;
    SpriteAnimator LeftAttackAnim;

    SpriteAnimator DeathAnim;

    // Current State
    [HideInInspector] public SpriteAnimator currentAnim;

    // Checking Previous Move
    bool wasPreviouslyMoving;

    // Refrences
    SpriteRenderer spriteRenderer;

    // Face Direction
    [HideInInspector] public string facing;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        #region IdleSprites
        DownIdleAnim = new SpriteAnimator(DownIdleSprites, spriteRenderer);
        UpIdleAnim = new SpriteAnimator(UpIdleSprites, spriteRenderer);
        RightIdleAnim = new SpriteAnimator(RightIdleSprites, spriteRenderer);

        if (flipLR)
        { LeftIdleAnim = new SpriteAnimator(RightIdleSprites, spriteRenderer); }
        else
        { LeftIdleAnim = new SpriteAnimator(LeftIdleSprites, spriteRenderer); }
        #endregion

        #region MoveSprites
        DownMoveAnim = new SpriteAnimator(DownMoveSprites, spriteRenderer);
        UpMoveAnim = new SpriteAnimator(UpMoveSprites, spriteRenderer);
        RightMoveAnim = new SpriteAnimator(RightMoveSprites, spriteRenderer);

        if (flipLR)
        { LeftMoveAnim = new SpriteAnimator(RightMoveSprites, spriteRenderer); }
        else
        { LeftMoveAnim = new SpriteAnimator(LeftMoveSprites, spriteRenderer); }
        #endregion

        #region AttackSprites
        DownAttackAnim = new SpriteAnimator(DownAttackSprites, spriteRenderer);
        UpAttackAnim = new SpriteAnimator(UpAttackSprites, spriteRenderer);
        RightAttackAnim = new SpriteAnimator(RightAttackSprites, spriteRenderer);

        if (flipLR)
        { LeftAttackAnim = new SpriteAnimator(RightAttackSprites, spriteRenderer); }
        else
        { LeftAttackAnim = new SpriteAnimator(LeftAttackSprites, spriteRenderer); }
        #endregion

        defaultColor = GetComponent<SpriteRenderer>().color;

        #region DeathSprites
        DeathAnim = new SpriteAnimator(DeathSprites, spriteRenderer);
        #endregion

        currentAnim = DownIdleAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;

        FaceDirection();

        ReturntoDefaultAnimation();

        if (isMoving == true)
        { MoveAnimation(); }
        else
        { IdleAnimation(); }

        if (isAttacking == true)
        { AttackAnimation(); }

        HitAnimation();

        if (isDeath == true)
        { DeathAnimation(); }

        FlipLR();

        if (currentAnim != prevAnim || isMoving != wasPreviouslyMoving)
        { currentAnim.Start(); }

        if (stopAnim == false)
        { currentAnim.HandleUpdate(); }

        wasPreviouslyMoving = isMoving;
    }

    void IdleAnimation()
    {
        if (currentAnim == RightMoveAnim)
        { currentAnim = RightIdleAnim; }
        else if (currentAnim == LeftMoveAnim)
        { currentAnim = LeftIdleAnim; }
        else if (currentAnim == UpMoveAnim)
        { currentAnim = UpIdleAnim; }
        else if (currentAnim == DownMoveAnim)
        { currentAnim = DownIdleAnim; }
    }

    void MoveAnimation()
    {
        if (MoveX > 0)
        { currentAnim = RightMoveAnim; }
        else if (MoveX < 0)
        { currentAnim = LeftMoveAnim; }
        else if (MoveY > 0)
        { currentAnim = UpMoveAnim; }
        else if (MoveY < 0)
        { currentAnim = DownMoveAnim; }
    }

    void AttackAnimation()
    {
        if (currentAnim == RightMoveAnim || currentAnim == RightIdleAnim)
        { currentAnim = RightAttackAnim; }
        else if (currentAnim == LeftMoveAnim || currentAnim == LeftIdleAnim)
        { currentAnim = LeftAttackAnim; }
        else if (currentAnim == UpMoveAnim || currentAnim == UpIdleAnim)
        { currentAnim = UpAttackAnim; }
        else if (currentAnim == DownMoveAnim || currentAnim == DownIdleAnim)
        { currentAnim = DownAttackAnim; }
    }

    void HitAnimation()
    {
        if (isHit == true)
        {
            moveTimer += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = hitColor;

            if (moveTimer >= hitTimer)
            {
                GetComponent<SpriteRenderer>().color = defaultColor;
                moveTimer = 0f;
                isHit = false;
            }
        }
    }


    void DeathAnimation()
    {
        currentAnim = DeathAnim;
    }

    void ReturntoDefaultAnimation()
    {
        #region Return Attacking to Idle State
        if (currentAnim == DownAttackAnim)
        {
            if (currentAnim.CurrentFrame == DownAttackSprites.Count - 1)
            {
                currentAnim = DownIdleAnim;
                isAttacking = false;
            }
        }
        else if (currentAnim == UpAttackAnim)
        {
            if (currentAnim.CurrentFrame == UpAttackSprites.Count - 1)
            {
                currentAnim = UpIdleAnim;
                isAttacking = false;
            }
        }
        else if (currentAnim == RightAttackAnim)
        {
            if (currentAnim.CurrentFrame == RightAttackSprites.Count - 1)
            {
                currentAnim = RightIdleAnim;
                isAttacking = false;
            }
        }
        else if (currentAnim == LeftAttackAnim)
        {
            if (flipLR == true)
            {
                if (currentAnim.CurrentFrame == RightAttackSprites.Count - 1)
                {
                    currentAnim = LeftIdleAnim;
                    isAttacking = false;
                }
            }
            else
            {
                if (currentAnim.CurrentFrame == LeftAttackSprites.Count - 1)
                {
                    currentAnim = LeftIdleAnim;
                    isAttacking = false;
                }
            }
        }
        #endregion

        if (currentAnim == DeathAnim)
        {
            if (currentAnim.CurrentFrame == DeathSprites.Count - 1)
            {
                stopAnim = true;
            }
        }
    }

    public void FaceDirection()
    {
        if (currentAnim == RightMoveAnim || currentAnim == RightIdleAnim || currentAnim == RightAttackAnim)
        { facing = "Right"; }
        else if (currentAnim == LeftMoveAnim || currentAnim == LeftIdleAnim || currentAnim == LeftAttackAnim)
        { facing = "Left"; }
        else if (currentAnim == UpMoveAnim || currentAnim == UpIdleAnim || currentAnim == UpAttackAnim)
        { facing = "Up"; }
        else if (currentAnim == DownMoveAnim || currentAnim == DownIdleAnim || currentAnim == DownAttackAnim)
        { facing = "Down"; }
    }

    void FlipLR() // Flip the Sprite when you do not have any left sprites
    {
        if (flipLR == true && currentAnim == LeftIdleAnim || flipLR == true && currentAnim == LeftMoveAnim || flipLR == true && currentAnim == LeftAttackAnim)
        { spriteRenderer.flipX = true; }
        else
        { spriteRenderer.flipX = false; }
    }
}
