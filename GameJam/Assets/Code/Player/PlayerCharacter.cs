using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour, IDamageable
{
    [Header("血量UI")]
    public List<Image> healthImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    [Header("基础属性")]
    public float maxHealth = 5f;
    public float moveSpeed = 5f;

    private float currentHealth;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool IsDead => currentHealth <= 0;

    [Header("翻滚设置")]
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
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        UpdateHealthUI();
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
        }
        if (!isOverheated && Input.GetKeyDown(KeyCode.Space) && canRoll && moveInput != Vector2.zero)
        {
            StartRoll();
        }

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                EndRoll();
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
            rb.velocity = moveInput * moveSpeed;
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
        currentHealth -= amount;
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
}
