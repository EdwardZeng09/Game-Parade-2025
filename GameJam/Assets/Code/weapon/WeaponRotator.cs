using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // 拖入这把枪的 SpriteRenderer

    private bool isOverheated;
    private Vector2 overheatDir;

    void Update()
    {
        if (isOverheated)
            RotateToDirection(overheatDir);
        else
            RotateToMouse();
    }

    void RotateToMouse()
    {
        // 获取鼠标世界坐标
        Vector3 mp = Input.mousePosition;
        mp.z = -Camera.main.transform.position.z;
        Vector3 world = Camera.main.ScreenToWorldPoint(mp);

        // 方向向量
        Vector2 dir = (world - transform.position).normalized;
        if (dir.sqrMagnitude < 0.01f) return;

        // 完整旋转
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 当枪口在左半边(角度>90 or <–90)时，flipY，否则不 flip
        bool flip = angle > 90f || angle < -90f;
        spriteRenderer.flipY = flip;
    }

    void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        bool flip = angle > 90f || angle < -90f;
        spriteRenderer.flipY = flip;
    }

    public void EnterOverheat(Vector2 dir)
    {
        isOverheated = true;
        overheatDir = dir.normalized;
    }

    public void ExitOverheat()
    {
        isOverheated = false;
    }
}
