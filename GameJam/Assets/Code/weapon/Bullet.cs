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
    public GameObject explosionPrefab;

    private bool isExplosive = false;
    private float explosionRadius = 0f;
    private float explosionDamagePercent = 0f;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

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
            if (isExplosive)
            {
                // ① 主目标也吃爆炸伤害
                float aoeDmg = damage * explosionDamagePercent;
                target.TakeDamage(aoeDmg);

                // ② 找周围的所有 IDamageable （按 LayerMask 过滤掉墙/地面）
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
                foreach (var c in hits)
                {
                    if (c.gameObject == shooter) continue;
                    var aoeTarget = c.GetComponent<IDamageable>();
                    if (aoeTarget != null && c.gameObject != other.gameObject)
                        aoeTarget.TakeDamage(aoeDmg);
                }
            }
            else
            {
                // 普通子弹
                target.TakeDamage(damage);
            }
            Instantiate(explosionPrefab,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void EnableExplosive(float radius, float damagePercent)
    {
        isExplosive = true;
        explosionRadius = radius;
        explosionDamagePercent = damagePercent;
    }
}
