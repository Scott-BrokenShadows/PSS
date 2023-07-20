using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerControl : MonoBehaviour
{
    public float speed;
    public float targetRange = 0.1f;
    public static float _targetRange;
    public static Transform _currentTransform;
    private Vector3 target;

    // Player Movement Control
    Rigidbody2D _rb;
    Vector2 _movementInput;
    Vector2 _smoothMovementInput;
    Vector2 _movementInputSmoothVelocity;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        target = transform.position;
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

    //public float targetRange = 0.1f;
    //public static float _targetRange;
    //public static Transform _currentTransform;

    //private Vector3 target;
    //void Awake()
    //{
    //    target = transform.position;
    //    _currentTransform = this.transform;
    //    _targetRange = targetRange;
    //}

    //void Update()
    //{
    //    MouseDown();
    //    MouseDrag();
    //}

    //void MouseDown()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        transform.position = new Vector3(target.x, target.y);
    //    }
    //}

    //void MouseDrag()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        transform.position = new Vector3(target.x, target.y);
    //    }
    //}
}
