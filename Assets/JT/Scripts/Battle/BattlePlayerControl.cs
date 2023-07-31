using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClampPlayer
{
    public float top;
    public float down;
    public float left;
    public float right;
}


public class BattlePlayerControl : MonoBehaviour
{
    public float speed;
    public float targetRange = 0.1f;
    public static float _targetRange;
    public static Transform _currentTransform;
    public Vector2 ScreenSpace;

    // Player Movement Control
    Rigidbody2D _rb;
    Vector2 _movementInput;
    Vector2 _smoothMovementInput;
    Vector2 _movementInputSmoothVelocity;

    // Player Clamping
    public ClampPlayer cPlayer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentTransform = this.transform;
        _targetRange = targetRange;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _movementInput = new Vector2(moveX, moveY).normalized;
        _smoothMovementInput = Vector2.SmoothDamp(_smoothMovementInput, _movementInput, ref _movementInputSmoothVelocity, 0.1f);
        _rb.velocity = _smoothMovementInput * speed;

        if (_currentTransform.transform.position.x >= 0 + cPlayer.right)
        {
            transform.position = new Vector3(0 + cPlayer.right, transform.position.y);
        }
        else if (_currentTransform.transform.position.x <= -BattleSystem.horizontal + cPlayer.left)
        {
            transform.position = new Vector3(-BattleSystem.horizontal + cPlayer.left, transform.position.y);
        }

        if (_currentTransform.transform.position.y >= BattleSystem.vertical + cPlayer.top)
        {
            transform.position = new Vector3(transform.position.x, BattleSystem.vertical + cPlayer.top);
        }
        else if (_currentTransform.transform.position.y <= -BattleSystem.vertical + cPlayer.down)
        {
            transform.position = new Vector3(transform.position.x, -BattleSystem.vertical + cPlayer.down);
        }
    }

    // Show Gizmos------------------------------------------------------------------------

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        #region 
        Gizmos.DrawLine(new Vector3(0 + cPlayer.right, -BattleSystem.vertical), new Vector3(0 + cPlayer.right, BattleSystem.vertical));
        Gizmos.DrawLine(new Vector3(-BattleSystem.horizontal + cPlayer.left, -BattleSystem.vertical), new Vector3(-BattleSystem.horizontal + cPlayer.left, BattleSystem.vertical));
        Gizmos.DrawLine(new Vector3(-BattleSystem.horizontal, BattleSystem.vertical + cPlayer.top), new Vector3(0, BattleSystem.vertical + cPlayer.top));
        Gizmos.DrawLine(new Vector3(-BattleSystem.horizontal, -BattleSystem.vertical + cPlayer.down), new Vector3(0, -BattleSystem.vertical + cPlayer.down));
        #endregion
    }
    #endregion
}
