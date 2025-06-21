using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedEnemyAttribute :Character
{
    public UnityEvent<Vector2> OnMovementInput;

    public UnityEvent OnAttack;

    [SerializeField] public Transform Player;

    [SerializeField] public float attackDistance = 0.8f;

    [SerializeField] public float damage;


    [SerializeField] private float separationRadius = 1f;
    [SerializeField] private float separationForceScale = 1f;

    //public GameObject projectile;//µ¯Ä»×é¼þ
    //public Transform shotPoint;
    protected override void Awake()
    {
        base.Awake();
        if (Player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                Player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (Player == null)
            return;
        float distance = Vector2.Distance(Player.position, transform.position);
        if (distance <= attackDistance)
        {
            //¹¥»÷Íæ¼Ò
            OnMovementInput?.Invoke(Vector2.zero);
            OnAttack?.Invoke();
        }
        else
        {
            //¼ÌÐø×·»÷
            Vector2 direction = Player.position - transform.position;
            direction += GetSeparationForce();
            OnMovementInput?.Invoke(direction.normalized);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Character player=collision.GetComponent<Character>();

        if (player != null) 
        {
            player.TakeDamage(damage);
        }
    }

    private Vector2 GetSeparationForce()
    {
        Vector2 force = Vector2.zero;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;
            if (!hit.CompareTag("Enemy")) continue;

            Vector2 diff = (Vector2)(transform.position - hit.transform.position);
            float distance = diff.magnitude;
            if (distance > 0f)
            {
                force += diff.normalized / distance;
            }
        }
        return force * separationForceScale;
    }
}
