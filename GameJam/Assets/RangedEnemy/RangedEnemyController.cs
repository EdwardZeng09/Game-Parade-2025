using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    [Header("属性")]
    [SerializeField] private float currentSpeed = 0;

    public Vector2 MovementInput { get; set; }
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    [Header("攻击")]
    [SerializeField] private bool isAttack = true;
    [SerializeField] private float attackCoolDuration = 1;

    private bool isDead;

    [SerializeField] public float knockbackForce = 5f;

    public bool canMove = true;

    public GameObject projectile;//弹幕组件
    public Transform shotPoint;

    public Transform Player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isAttack = true;
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

    public void Attack()
    {
        if (isAttack)
        { 
            isAttack = false;
            Vector2 direction = (Player.position - transform.position).normalized;

            if (direction.x < 0)
                sr.flipX=true;
            if (direction.x > 0)
                sr.flipX = false;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            StartCoroutine(AttackCoroutine(rotation));
        }
    }


    IEnumerator AttackCoroutine(Quaternion rotation)
    {

        animator.SetTrigger("isAttack");//或者bool类型触发动画
        yield return new WaitForSeconds(0.5f);
        Instantiate(projectile, shotPoint.position, rotation);
        yield return new WaitForSeconds(attackCoolDuration);
        isAttack = true;
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
        animator.SetBool("isWalk", MovementInput.magnitude > 0);
        //animator.SetBool("isDead", isDead);
    }
    public void Knockback(Vector2 attackerPosition)
    {
        Vector2 direction = ((Vector2)transform.position - attackerPosition).normalized;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(StopMove());
    }


    IEnumerator StopMove()
    {
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
