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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isDead)
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
            if (MovementInput.x > 0)
                sr.flipX = true;
            if (MovementInput.x < 0)
                sr.flipX = false;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    //public void Attack()
    //{
    //    if (isAttack)
    //    {
    //        isAttack = false;
    //        StartCoroutine(nameof(AttackCoroutine));
    //    }
    //}

    //IEnumerator AttackCoroutine()
    //{
    //    animator.SetTrigger("isAttack");//或者bool类型触发动画
    //    yield return new WaitForSeconds(attackCoolDuration);
    //    //isAttack = true;
    //}
    public void EnemyHurt()
    {
        animator.SetTrigger("isHurt");
    }
    public void Dead()
    {
        DropManager.Instance.Drop(transform.position);
        isDead = true;
    }

    public void SetAnimation()
    {
        //animator.SetBool("isWalk", MovementInput.magnitude > 0);
        animator.SetBool("isDead", isDead);
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
}
