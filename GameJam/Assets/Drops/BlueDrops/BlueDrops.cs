using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDrops : MonoBehaviour
{
    [SerializeField] public LayerMask mask;
    [Header("拾取设置")]
    [SerializeField] public float pickupRadius = 2f;
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float pickupDistance = 0.2f;


    public AudioSource deathAudioSource;
    public AudioClip deathClip;

    private Transform targetPlayer;
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
            Vector3 dir = (targetPlayer.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPlayer.position) < pickupDistance&&i==0)
            {
                OnPickup(targetPlayer.gameObject);
                deathAudioSource.Play();
                StartCoroutine(Delay());
              
                i++;
            }
        }
    }
    private void OnPickup(GameObject player)
    {
        //Drops功能
        return;
      
    }
    //可视化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

    IEnumerator Delay() 
    { 
        yield return new WaitForSeconds(1f); 
        Destroy(gameObject);
    }
}
