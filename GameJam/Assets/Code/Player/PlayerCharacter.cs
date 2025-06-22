using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour, IDamageable
{
    public Animator animator;
    private SpriteRenderer sr;
    [Header("Ѫ��UI")]
    public List<Image> healthImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    [Header("��������")]
    public float maxHealth = 5f;
    public float moveSpeed = 5f;

    private float currentHealth;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool IsDead => currentHealth <= 0;

    [Header("��������")]
    public float rollSpeed = 10f;
    public float rollDuration = 0.3f;
    public float rollCooldown = 1f;
    private bool isRolling = false;
    private bool canRoll = true;
    private float rollTimer = 0f;
    private Vector2 rollDirection;

    private Vector3 lastDirection = Vector3.right; 
    private const float minRotationThreshold = 1f;

    private bool isOverheated = false;
    
    [Header("BuffDebuff����")]
    private float speedDebuffMultiplier = 1f;
    private float baseMaxHealth;
    private float damageTakenBonus = 0f;
    private Camera mainCam;
    private bool camIsOrtho;
    private float baseViewValue;
    // Start is called before the first frame update
    void Start()
    {
        baseMaxHealth = maxHealth;
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        UpdateHealthUI();
        mainCam = Camera.main;
        camIsOrtho = mainCam.orthographic;
        baseViewValue = camIsOrtho
            ? mainCam.orthographicSize
            : mainCam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
        if (!isRolling)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
            animator.SetFloat("Speed", moveInput.magnitude);
            if (moveInput.x < 0) sr.flipX = true;
            else if (moveInput.x > 0) sr.flipX = false;
        }
        if (!isOverheated && Input.GetKeyDown(KeyCode.Space) && canRoll && moveInput != Vector2.zero)
        {
            StartRoll();
            animator.SetBool("roll", true);
        }

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                EndRoll();
                animator.SetBool("roll", false);
            }
        }
    }

    void FixedUpdate()
    {
        if (isRolling)
        {
            rb.velocity = rollDirection * rollSpeed;
        }
        else
        {
            rb.velocity = moveInput * moveSpeed * speedDebuffMultiplier;
        }
    }

    public void EnterOverheatMovement()
    {
        isOverheated = true;
    }

    public void ExitOverheatMovement()
    {
        isOverheated = false;
        moveInput = Vector2.zero; 
    }
    private void StartRoll()
    {
        isRolling = true;
        canRoll = false;
        rollTimer = rollDuration;
        rollDirection = moveInput;
        StartCoroutine(RollCooldownTimer());
    }

    private void EndRoll()
    {
        isRolling = false;
    }

    private IEnumerator RollCooldownTimer()
    {
        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        if (isRolling) return;
        float actualDamage = amount + damageTakenBonus;
        currentHealth -= actualDamage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        UpdateHealthUI();
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < maxHealth)
            {
                healthImages[i].enabled = true;
                healthImages[i].sprite = i < currentHealth ? heartFull : heartEmpty;
            }
            else
            {
                healthImages[i].enabled = false;
            }
        }
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        if (maxHealth > healthImages.Count) maxHealth = healthImages.Count; 
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }

    public void ApplyWeakenedDebuff(int level)
    {
        speedDebuffMultiplier = 1f - 0.1f * level;
        Debug.Log($"[Debuff] ��Ӧ�� ���� �� �ȼ� {level}, �ٶȱ��� {speedDebuffMultiplier:P0}");
    }
    public void ApplySacrificeDebuff(int level)
    {
        float[] reduces = { 1f, 2f, 3f };
        float pct = reduces[level - 1];
        maxHealth = baseMaxHealth - pct;

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        UpdateHealthUI();

        Debug.Log($"[Debuff] ��Ӧ�� ���� �� �ȼ� {level}��������� {maxHealth:F1}");
    }

    public void ApplyFragileDebuff(int level)
    {
        damageTakenBonus = level;
        Debug.Log($"[Debuff] ��Ӧ�� ���� �� �ȼ� {level}, �������� +{damageTakenBonus}");
    }

    public void ApplyBlindnessDebuff(int level)
    {
        if (mainCam == null) return;

        // �ȼ� �� ���ٱ���
        float[] reduces = { 0.2f, 0.4f, 0.6f };
        float pct = reduces[Mathf.Clamp(level - 1, 0, reduces.Length - 1)];
        float newValue = baseViewValue * (1f - pct);

        if (camIsOrtho)
            mainCam.orthographicSize = newValue;
        else
            mainCam.fieldOfView = newValue;

        Debug.Log($"[Debuff] ��Ӧ�� ��ä �� �ȼ� {level}, ��Ұ��С�� {newValue:F1} (���� {pct:P0})");
    }
}
