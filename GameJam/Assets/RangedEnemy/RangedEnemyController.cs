using System.Collections;
using System.Collections.Generic;
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

    public GameObject projectile;//弹幕组件
    public Transform shotPoint;

    public Transform Player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isAttack = true;
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
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            StartCoroutine(AttackCoroutine(rotation));
        }
    }

    IEnumerator AttackCoroutine(Quaternion rotation)
    {

        //animator.SetTrigger("isAttack");//或者bool类型触发动画
        Instantiate(projectile, shotPoint.position, rotation);
        //GameObject bulletObj = Instantiate(projectile, shotPoint.position, rotation);
        //Bullet bullet = bulletObj.GetComponent<Bullet>();
        //bullet.SetDirection((Player.position - transform.position).normalized);
        yield return new WaitForSeconds(attackCoolDuration);
        isAttack = true;
    }
    public void EnemyHurt()
    {
        animator.SetTrigger("isHurt");
    }
    public void Dead()
    {
        isDead = true;
    }

    public void SetAnimation()
    {
        animator.SetBool("isRun", MovementInput.magnitude > 0);
        animator.SetBool("isDead", isDead);
    }
}
