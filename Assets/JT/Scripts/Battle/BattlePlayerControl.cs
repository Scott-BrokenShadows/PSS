using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerControl : MonoBehaviour
{
    public float targetRange = 0.1f;
    public static float _targetRange;
    public static Transform _currentTransform;

    private Vector3 target;
    void Awake()
    {
        target = transform.position;
        _currentTransform = this.transform;
        _targetRange = targetRange;
    }

    void Update()
    {
        MouseDown();
        MouseDrag();
    }

    void MouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(target.x, target.y);
        }
    }

    void MouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(target.x, target.y);
        }
    }
}
