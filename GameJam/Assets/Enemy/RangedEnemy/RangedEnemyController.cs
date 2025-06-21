using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float currentSpeed = 0;

    public Vector2 MovementInput { get; set; }
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    [Header("����")]
    [SerializeField] private bool isAttack = true;
    [SerializeField] private float attackCoolDuration = 1;

    private bool isDead;

    [SerializeField] public float knockbackForce = 5f;

    public bool canMove = true;

    public GameObject projectile;//��Ļ���
    public Transform shotPoint;

    public Transform Player;

    public AudioSource HurtAudioSource;

    //[Header("Fade����")]
    //[SerializeField] public Material enemyMaterial;
    //[SerializeField] public float fadeDuration = 2f;
    //public float currentFadeValue = 1f;
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
        //GetComponent<SpriteRenderer>().material = enemyMaterial;
       
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

        animator.SetTrigger("isAttack");//����bool���ʹ�������
        yield return new WaitForSeconds(0.5f);
        Instantiate(projectile, shotPoint.position, rotation);
        yield return new WaitForSeconds(attackCoolDuration);
        isAttack = true;
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
        //StartCoroutine(FadeOutAndDestroy());
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


    //IEnumerator FadeOutAndDestroy()
    //{
    //    float startFade = currentFadeValue;
    //    float endFade = 0f;
    //    float timeElapsed = 0f;
    //    canMove = false;

    //    // ȷ��ÿ���޸ĺ�ǿ��Ӧ�ò���
    //    if (GetComponent<SpriteRenderer>().material != enemyMaterial)
    //    {
    //        GetComponent<SpriteRenderer>().material = enemyMaterial;
    //    }

    //    while (timeElapsed < fadeDuration)
    //    {
    //        currentFadeValue = Mathf.Lerp(startFade, endFade, timeElapsed / fadeDuration);

    //        // ȷ������͸���Ȳ�ǿ��ˢ�²���
    //        enemyMaterial.SetFloat("Fade", currentFadeValue);
    //        GetComponent<SpriteRenderer>().material = enemyMaterial;  // ǿ�Ƹ��²���

    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    currentFadeValue = endFade;
    //    enemyMaterial.SetFloat("Fade", currentFadeValue);

    //    // ��������
    //    Debug.Log("Fade finished, destroying object.");
    //    Destroy(gameObject);
    //}
}
