using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;
    private float fireCooldown = 0f;

    [Header("Œ¬øÿ…Ë÷√")]
    public float currentHeat = 0f;
    public float maxHeat = 100f;
    public float heatPerShot = 5f;
    public float coolRate = 0.1f;
    public bool isOverheated = false;

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        HandleCooling();
        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    private void HandleCooling()
    {
        if (currentHeat > 0)
        {
            currentHeat -= coolRate * Time.deltaTime;
            currentHeat = Mathf.Clamp(currentHeat, 0, maxHeat);

            if (isOverheated && currentHeat <= 0f)
            {
                isOverheated = false;
            }
        }
    }
    void Shoot()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 fireDirection = (mouseWorldPos - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(fireDirection);

        currentHeat += heatPerShot;
        if (currentHeat >= maxHeat)
        {
            isOverheated = true;
        }
    }

}
