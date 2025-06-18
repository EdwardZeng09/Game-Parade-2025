using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotator : MonoBehaviour
{
    private bool isOverheated = false;
    private Vector2 overheatDirection = Vector2.right;

    void Update()
    {
        if (isOverheated)
        {
            RotateToDirection(overheatDirection);
        }
        else
        {
            RotateToMouse();
        }
    }

    void RotateToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - transform.position;
        direction.z = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void RotateToDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void EnterOverheat(Vector2 firingDir)
    {
        isOverheated = true;
        overheatDirection = firingDir.normalized;
    }

    public void ExitOverheat()
    {
        isOverheated = false;
    }
}
