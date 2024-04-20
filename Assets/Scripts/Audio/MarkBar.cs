using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBar : MonoBehaviour
{
    [SerializeField]
    private GameObject markPoints = null;
    public GameObject MarkPoints
    {
        get { return markPoints; }
    }

    [SerializeField]
    private float moveSpeed = 1f;

    private Vector3 moveDir = Vector3.zero;
    private Vector3 prevMousePosition = Vector3.zero;
    private Vector3 curMousePosition = Vector3.zero;

    private bool mouseLeftDown = false;

    void Update()
    {
        CheckMouse();
        CheckMoveDir();
        Move();
    }
    private void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseLeftDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseLeftDown = false;
        }
    }
    private void CheckMoveDir()
    {
        curMousePosition = Input.mousePosition;

        moveDir = (curMousePosition - prevMousePosition).normalized;
        moveDir.y = 0f;

        prevMousePosition = curMousePosition;
    }
    private void Move()
    {
        if (mouseLeftDown && moveDir != Vector3.zero)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            moveDir = Vector3.zero;
        }
    }
}
