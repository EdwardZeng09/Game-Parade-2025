using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("µ¯Ä»ÊôÐÔ")]
    [SerializeField] public float speed = 10f;
    [SerializeField] public float damage = 10f;
    [SerializeField] public float lifeTime = 2f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir) 
    {
    direction = dir.normalized;
    }
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        lifeTime-=Time.deltaTime;
        if (lifeTime <= 0f) 
        {
        Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
