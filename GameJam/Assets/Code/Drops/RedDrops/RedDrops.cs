using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDrops : MonoBehaviour
{
    [SerializeField]public LayerMask mask;
    [Header("拾取设置")]
    [SerializeField]public float pickupRadius=2f;
    [SerializeField] public float moveSpeed=5f;
    [SerializeField] public float pickupDistance = 0.2f;
    [SerializeField] public float HP = 10f;
    private Transform targetPlayer;
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

            if (Vector3.Distance(transform.position, targetPlayer.position) < pickupDistance) 
            {
                OnPickup(targetPlayer.gameObject);
                Destroy(gameObject);
            }
        }
    }
    private void OnPickup(GameObject player) 
    {
    //Drops功能
    IDamageable healTarget = player.GetComponent<IDamageable>();
        if (healTarget!=null) 
        {
            healTarget.Heal(HP);
        }
    }
    //可视化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
