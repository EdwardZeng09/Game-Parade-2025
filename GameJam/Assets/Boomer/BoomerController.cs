using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoomerController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float currentSpeed = 0;

    public Vector2 MovementInput { get; set; }
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    [Header("����")]
    [SerializeField] public float damage = 50f;
    [SerializeField] public float explosionRadius=2f;
    [SerializeField] public float chaseRadius = 1f;

    public LayerMask damageLayer;
    private bool isDead;

    public float delay = 1f;
    public Coroutine explosionCoroutine;
    public bool isPlayerInside = false;
    public bool canMove=true;
   
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isDead&&canMove)
            Move();
        SetAnimation();
    }
    private void Update()
    {
     Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, chaseRadius, damageLayer);
        if (player.Length>0)
        {
            
            if (!isPlayerInside)
            {
               
                isPlayerInside = true;
                canMove=false;//ֹͣ�ƶ�
                rb.velocity = Vector2.zero;
                explosionCoroutine = StartCoroutine(DelayedExplosion());
            }
        }
        else 
        {
           
            if (isPlayerInside) 
            {
                
               
                isPlayerInside = false;
                canMove = true;
            StopCoroutine(explosionCoroutine);
                explosionCoroutine = null;
            }
        }
    }
    public void Move()
    {
        if (MovementInput.magnitude > 0.1f && currentSpeed >= 0)
        {
            //�����ƶ�
            rb.velocity = MovementInput * currentSpeed;

            //���˳���ת
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
       
    }

    //��ը�˺��¼�
    public void ExplodeDamage() 
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);
        foreach (Collider2D col in targets) 
        {
        IDamageable target=col.GetComponent<IDamageable>();
            if (target != null) 
            {
                target.TakeDamage(damage);
            }
        }
    }
    
    public void EnemyHurt()
    {
        animator.SetTrigger("isHurt");
    }
    public void Dead()
    {
        isDead = true;
        Destroy(this.gameObject);
    }
   

    public void SetAnimation()
    {
        //animator.SetBool("isRun", MovementInput.magnitude > 0);
        //animator.SetBool("isDead", isDead);
    }

    //��ը��Χ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }


    private IEnumerator DelayedExplosion() 
    {
    //animator.SetTrigger("waitForBoom");
    yield return new WaitForSeconds(delay);
        animator.SetTrigger("isBoom");
    }
}
