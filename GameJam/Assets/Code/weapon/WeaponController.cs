using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float baseFireRate = 1f;
    private float fireCooldown = 0f;

    [Header("�¿�����")]
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

    [Header("RapidFire����")]
    private float rapidFireMultiplier = 1f;

    [Header("MultiShot ɢ������")]
    public float multiShotDamageFactor = 0f;   // Buff �ȼ� �� 0.1f
    public float multiShotSpreadAngle = 15f; // ÿ���ӵ�ƫת�Ƕ�
    void Start()
    {
        player = FindObjectOfType<PlayerCharacter>();
    }
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
                float fireRate = baseFireRate * rapidFireMultiplier;
                fireCooldown = 1f / fireRate;
            }
        } 
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
        if (multiShotDamageFactor > 0f)
        {
            // ���Ҹ�һ��
            float[] angles = { -multiShotSpreadAngle, multiShotSpreadAngle };
            foreach (var a in angles)
            {
                // ����ƫת��ķ���
                Vector3 dir3 = new Vector3(fireDirection.x, fireDirection.y, 0);
                Vector3 rotated = Quaternion.Euler(0, 0, a) * dir3;
                Vector2 extraDir = new Vector2(rotated.x, rotated.y).normalized;

                // Instantiate �����ӵ�
                GameObject eb = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                var bComp = eb.GetComponent<Bullet>();
                if (bComp != null)
                {
                    bComp.SetDirection(extraDir);
                    bComp.SetShooter(gameObject);
                    // �����������˺�
                    bComp.damage *= multiShotDamageFactor;
                }
                weaponSpriteController.FlashOnFire();
            }
        }
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
        if (multiShotDamageFactor > 0f)
        {
            // ���Ҹ�һ��
            float[] angles = { -multiShotSpreadAngle, multiShotSpreadAngle };
            foreach (var a in angles)
            {
                // ����ƫת��ķ���
                Vector3 dir3 = new Vector3(randomDirection.x, randomDirection.y, 0);
                Vector3 rotated = Quaternion.Euler(0, 0, a) * dir3;
                Vector2 extraDir = new Vector2(rotated.x, rotated.y).normalized;

                // Instantiate �����ӵ�
                GameObject eb = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                var bComp = eb.GetComponent<Bullet>();
                if (bComp != null)
                {
                    bComp.SetDirection(extraDir);
                    bComp.SetShooter(gameObject);
                    // �����������˺�
                    bComp.damage *= multiShotDamageFactor;
                }
                weaponSpriteController.FlashOnFire();
            }
        }
    }

    void FireBullet(Vector2 dir)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(dir);
        bullet.GetComponent<Bullet>().SetShooter(gameObject);
        weaponSpriteController.FlashOnFire();
    }


    public void ApplyRapidFireBuff(int level)
    {
        rapidFireMultiplier = 1f + 0.1f * level;
        Debug.Log($"[Weapon] ��Ӧ�� �����䵼�족 �ȼ� {level}������ {rapidFireMultiplier:P0}");
    }

    public void ApplyMultiShotBuff(int level)
    {
        // ÿ�ζ��������ӵ����˺��� 10%/20%/30% ����
        multiShotDamageFactor = 0.1f * level;
        Debug.Log($"[Weapon] ��Ӧ�� MultiShot ɢ�� �ȼ� {level}�������ӵ��˺� {multiShotDamageFactor:P0}");
    }
}
