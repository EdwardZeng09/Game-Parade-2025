using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class RedDrops : MonoBehaviour
{
    [SerializeField]public LayerMask mask;
    [Header("ʰȡ����")]
    [SerializeField]public float pickupRadius=2f;
    [SerializeField] public float moveSpeed=5f;
    [SerializeField] public float pickupDistance = 0.2f;
    [SerializeField] public float HP = 10f;
    private Transform targetPlayer;


    public AudioSource source;
    public int i = 0;
    private void Update()
    {
        if (targetPlayer == null)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, pickupRadius, mask);
            if (targets.Length > 0)
            {
                targetPlayer = targets[0].transform;
            }
        }
        else 
        {
        Vector3 dir=(targetPlayer.position-transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPlayer.position) < pickupDistance&&i==0) 
            {
                OnPickup(targetPlayer.gameObject);
                StartCoroutine(Delay());
                i++;
            }
        }
    }
    private void OnPickup(GameObject player) 
    {
    //Drops����
    IDamageable healTarget = player.GetComponent<IDamageable>();
        if (healTarget!=null) 
        {
            healTarget.Heal(HP);
        }
    }
    //���ӻ�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

    IEnumerator Delay() 
    {
    source.Play();
    yield  return new WaitForSeconds(0.5f);
    Destroy(gameObject);
    }
}
