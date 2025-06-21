using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoomerController : MonoBehaviour
{
    [Header("ÊôÐÔ")]
    [SerializeField] private float currentSpeed = 0;

    public Vector2 MovementInput { get; set; }
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    [Header("¹¥»÷")]
    [SerializeField] public float damage = 50f;
    [SerializeField] public float explosionRadius=2f;
    [SerializeField] public float chaseRadius = 1f;

    public LayerMask damageLayer;
    private bool isDead;

    public float delay = 1f;
    public Coroutine explosionCoroutine;
    public bool isPlayerInside = false;
    public bool canMove=true;

    [SerializeField] public Transform Player;

    [SerializeField] public float knockbackForce = 5f;

    public AudioSource HurtAudioSource;
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
    private void Update()
    {
     Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, chaseRadius, damageLayer);
        if (player.Length>0)
        {
            
            if (!isPlayerInside)
            {
               
                isPlayerInside = true;
                canMove=false;//Í£Ö¹ÒÆ¶¯
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
            //µÐÈËÒÆ¶¯
            rb.velocity = MovementInput * currentSpeed;

            //µÐÈË³¯Ïò·­×ª
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

    public void Attack()
    {
       
    }

    //±¬Õ¨ÉËº¦ÊÂ¼þ
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
        Knockback(Player.position);
        animator.SetTrigger("isHurt");
        HurtAudioSource.Play();
    }
    public void Dead()
    {
        DropManager.Instance.Drop(transform.position);
        isDead = true;
        FindObjectOfType<EnemySpawner>().OnEnemyKilled();
        Destroy(this.gameObject);
    }
   

    public void SetAnimation()
    {
        //animator.SetBool("isRun", MovementInput.magnitude > 0);
        //animator.SetBool("isDead", isDead);
    }

    //±¬Õ¨·¶Î§
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
