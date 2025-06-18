using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;
    private float fireCooldown = 0f;

    [Header("温控设置")]
    public float currentHeat = 0f;
    public float maxHeat = 100f;
    public float heatPerShot = 5f;
    public bool isOverheated = false;
    public float coolInterval = 2f;
    public float coolAmountPerTick = 5f; 
    private float coolTimer = 0f;

    public float overheatFireRate = 0.2f;

    public PlayerCharacter player;
    public WeaponRotator weaponRotator;
    public WeaponSpriteController weaponSpriteController;
    public OverheatBarSwitcher overheatBar;
    void Update()
    {
        fireCooldown -= Time.deltaTime;
        HandleCooling();
        if (isOverheated)
        {
            if (fireCooldown <= 0f)
            {
                ShootRandom();
                fireCooldown = overheatFireRate;
            }
        }
        else {
           if (Input.GetMouseButton(0) && fireCooldown <= 0f)
           {
                Shoot();
                fireCooldown = fireRate;
           }
        } 
        Debug.Log($"当前温度: {currentHeat}");
    }

    private void HandleCooling()
    {
        if (currentHeat > 0)
        {
            float currentInterval = isOverheated ? 0.5f : coolInterval;

            coolTimer += Time.deltaTime;

            if (coolTimer >= currentInterval)
            {
                currentHeat -= coolAmountPerTick;
                currentHeat = Mathf.Clamp(currentHeat, 0, maxHeat);
                coolTimer = 0f;

                if (isOverheated && currentHeat <= 0)
                {
                    isOverheated = false;
                    player.ExitOverheatMovement();
                    weaponRotator.ExitOverheat();
                    overheatBar.SetOverheatState(false);
                }
            }
        }
    }
    void Shoot()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 fireDirection = (mouseWorldPos - firePoint.position).normalized;
        FireBullet(fireDirection);


        if (currentHeat == maxHeat)
        {
            isOverheated = true;
            player.EnterOverheatMovement();
            overheatBar.SetOverheatState(true);
        }
        else
        {
            currentHeat += heatPerShot;
        }
    }
    void ShootRandom()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        weaponRotator.EnterOverheat(randomDirection);
        FireBullet(randomDirection);

    }

    void FireBullet(Vector2 dir)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(dir);
        bullet.GetComponent<Bullet>().SetShooter(gameObject);
        weaponSpriteController.FlashOnFire();
    }
}
