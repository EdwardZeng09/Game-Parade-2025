using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("µ¯Ä»ÊôÐÔ")]
    [SerializeField] public float speed = 10f;
    [SerializeField] public float damage = 10f;
    [SerializeField] public float lifeTime = 2f;


    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        //transform.Translate(direction * speed * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
             IDamageable target = other.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
       
    }

}