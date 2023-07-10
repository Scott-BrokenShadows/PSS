using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerControl : MonoBehaviour
{
    public float speed;
    public float targetRange = 0.1f;
    public static float _targetRange;
    public static Transform _currentTransform;

    // Player Movement Control
    Rigidbody2D _rb;
    Vector2 _movementInput;
    Vector2 _smoothMovementInput;
    Vector2 _movementInputSmoothVelocity;

    // Start is called before the first frame update
    void Start()
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
    }
}
