using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    public float lifeTime = 2f;
    private Vector2 direction;
    private GameObject shooter;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
    public void SetShooter(GameObject shooterObj)
    {
        shooter = shooterObj;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == shooter) return;
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
