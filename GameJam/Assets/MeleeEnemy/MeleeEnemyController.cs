using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float currentSpeed = 0;

    public Vector2 MovementInput { get; set; }
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    [Header("����")]
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
            //�����ƶ�
            rb.velocity = MovementInput * currentSpeed;

            //���˳���ת
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
    //    animator.SetTrigger("isAttack");//����bool���ʹ�������
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
