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
    private UpgradeUI upgradeUI;

    [Header("RapidFire����")]
    private float rapidFireMultiplier = 1f;

    [Header("MultiShot ɢ������")]
    public float multiShotDamageFactor = 0f;   
    public float multiShotSpreadAngle = 15f; 

    [Header("ExplosiveAmmo Buff ����")]
    private bool useExplosiveAmmo = false;
    private float explosiveRadius = 0f;
    private float explosiveDamagePercent = 0f;

    [Header("HeatCapacity Buff ����")]
    private float heatCapacityMultiplier = 1f;  
    public float baseMaxHeat;                   

    [Header("Overclock Buff ����")]
    private float overclockMultiplier = 1f;

    [Header("Overdrive Buff ����")]
    private float overdriveMultiplier = 1f; 
    private float baseCoolInterval;          
    private float baseCoolAmount;

    [Header("Berserk Debuff ����")]
    private float berserkExtraDuration = 0f; 
    private bool berserkExtraActive = false;
    private float berserkExtraTimer = 0f;

    void Start()
    {
        player = FindObjectOfType<PlayerCharacter>();
        upgradeUI = FindObjectOfType<UpgradeUI>();
        baseMaxHeat = maxHeat;
        baseCoolInterval = coolInterval;
        baseCoolAmount = coolAmountPerTick;
    }
    void Update()
    {
        if (upgradeUI != null && upgradeUI.panel.activeSelf)
            return;
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
            float currentInterval = isOverheated
                ? 0.5f / overdriveMultiplier
                : baseCoolInterval / overdriveMultiplier;

            coolTimer += Time.deltaTime;
            if (coolTimer >= currentInterval)
            {
                currentHeat -= baseCoolAmount * overdriveMultiplier;
                currentHeat = Mathf.Clamp(currentHeat, 0, maxHeat);
                coolTimer = 0f;
            }
        }

        // ����ȴ�� 0 �ҵ�ǰ���ڹ���״̬
        if (isOverheated && currentHeat <= 0f)
        {
            // ��һ�δ��� extra �߼�
            if (!berserkExtraActive && berserkExtraDuration > 0f)
            {
                berserkExtraActive = true;
                berserkExtraTimer = berserkExtraDuration;
                Debug.Log($"[Weapon] �������ʧ�ؽ׶Σ����� {berserkExtraDuration} ��");
            }

            // ������� extra ʧ�ؽ׶Σ��򵹼�ʱ
            if (berserkExtraActive)
            {
                berserkExtraTimer -= Time.deltaTime;
                if (berserkExtraTimer <= 0f)
                {
                    // ����ʧ��
                    isOverheated = false;
                    berserkExtraActive = false;
                    berserkExtraDuration = 0f;
                    player.ExitOverheatMovement();
                    weaponRotator.ExitOverheat();
                    overheatBar.SetOverheatState(false);
                    Debug.Log("[Weapon] ����ʧ�ؽ������ָ�����");
                }
            }
            else
            {
                // û�� extra��ֱ�ӻָ�
                isOverheated = false;
                player.ExitOverheatMovement();
                weaponRotator.ExitOverheat();
                overheatBar.SetOverheatState(false);
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
        var b = bullet.GetComponent<Bullet>();
        b.SetDirection(dir);
        b.SetShooter(gameObject);
        float baseDmg = GetHeatBasedDamage();
        b.damage = baseDmg * overclockMultiplier;
        if (useExplosiveAmmo)
        {
            b.EnableExplosive(explosiveRadius, explosiveDamagePercent);
        } 
        weaponSpriteController.FlashOnFire();
    }

    private float GetHeatBasedDamage()
    {
        if (currentHeat < 20f) return 20f;
        else if (currentHeat < 40f) return 25f;
        else if (currentHeat < 60f) return 30f;
        else if (currentHeat < 80f) return 35f;
        else return 40f;
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

    public void ApplyExplosiveAmmoBuff(int level)
    {
        useExplosiveAmmo = true;
        explosiveRadius = new float[] { 1f, 1.5f, 2f }[level - 1];        // ����1 �� 1 ��, 2 �� 1.5 �� ...
        explosiveDamagePercent = new float[] { 0.5f, 0.75f, 1f }[level - 1];     // 50%��75%��100%
        Debug.Log($"[Weapon] ��Ӧ�� �����ӵ� Buff �� �ȼ� {level}, �뾶 {explosiveRadius}, �˺�{explosiveDamagePercent:P0}");
    }

    public void ApplyHeatCapacityBuff(int level)
    {
        // �ȼ� 1~3 ��Ӧ +15%/+30%/+50%
        float[] boosts = { 0.15f, 0.30f, 0.50f };
        heatCapacityMultiplier = 1f + boosts[level - 1];
        maxHeat = baseMaxHeat * heatCapacityMultiplier;

        currentHeat = Mathf.Min(currentHeat + baseMaxHeat * boosts[level - 1], maxHeat);

        Debug.Log($"[Weapon] ��Ӧ�� HeatCapacity Buff �� �ȼ� {level}�������� {maxHeat:F1}");
    }

    public void ApplyOverclockBuff(int level)
    {
        // ÿ������ 10%/20%/30%
        overclockMultiplier = 1f + 0.1f * level;
        Debug.Log($"[Weapon] ��Ӧ�� Overclock ���� �� �ȼ� {level}���˺����� {overclockMultiplier:P0}");
    }

    public void ApplyOverdriveBuff(int level)
    {
        // �ȼ� 1/2/3 ��Ӧ +20%/+40%/+60%
        overdriveMultiplier = 1f + 0.2f * level;

        Debug.Log($"[Weapon] ��Ӧ�� Overdrive ��Ƶ �� �ȼ� {level}��" +
                  $"�������� ��{overdriveMultiplier:F2}");
    }

    public void ApplyBerserkBuff(int level)
    {
        berserkExtraDuration = Mathf.Clamp(level, 1, 3);
        Debug.Log($"[Weapon] Ӧ�� Berserk Debuff �� ����ʧ�� {berserkExtraDuration} ��");
    }
}
