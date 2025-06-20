using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    [Header("属性")]
    [SerializeField] private float currentSpeed = 0;

    public Vector2 MovementInput { get; set; }
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    [Header("攻击")]
    //[SerializeField] private bool isAttack = true;
    //[SerializeField] private float attackCoolDuration = 1;
    [SerializeField] public float damage = 10f;

    private bool isDead;
    [SerializeField] public Transform Player;

    [SerializeField] public float knockbackForce = 5f;
   
    public bool canMove=true;

    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Canvas worldCanves;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        if (Player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                Player = playerObj.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead&&canMove)
        Move();
        SetAnimation();
    }
    public void Move()
    {
        if (MovementInput.magnitude > 0.1f && currentSpeed >= 0)
        {
            //敌人移动
            rb.velocity = MovementInput * currentSpeed;

            //敌人朝向翻转
            if (MovementInput.x < 0)
                sr.flipX = true;
            if (MovementInput.x > 0)
                sr.flipX = false;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    


  
    public void EnemyHurt()
    {
        Knockback(Player.position);
        animator.SetTrigger("isHurt");
        
    }
    public void Dead()

    {
       
        DropManager.Instance.Drop(transform.position);
        isDead = true;
        FindObjectOfType<EnemySpawner>().OnEnemyKilled(); 
        Destroy(gameObject);
    }

    public void SetAnimation()
    {
        //animator.SetBool("isWalk", MovementInput.magnitude > 0);
        //animator.SetBool("isDead", isDead);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
           
        }
    }


    public void Knockback(Vector2 attackerPosition) 
    {   
    Vector2 direction=((Vector2)transform.position-attackerPosition).normalized;
    rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(StopMove());
    }


    IEnumerator StopMove() 
    {canMove = false;
        yield return new WaitForSeconds(0.2f);
    canMove = true; 
    }


    public void ShowDamageText(float damage) 
    {
    Vector3 spawnPos= transform.position+new Vector3(0,1f,0);
    GameObject obj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity, worldCanves.transform);
        obj.GetComponent<DamageText>().SetText(damage.ToString("F0"));
    }
}
