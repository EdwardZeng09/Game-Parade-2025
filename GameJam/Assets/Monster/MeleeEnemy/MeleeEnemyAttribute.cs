using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeleeEnemyAttribute : Character
{
    public UnityEvent<Vector2> OnMovementInput;

    [SerializeField] public Transform Player;

    [SerializeField] public float damage;


    [SerializeField] private float separationRadius = 1f;
    [SerializeField] private float separationForceScale = 1f;


    private void Awake()
    {
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
            Vector2 direction = Player.position - transform.position;
        direction += GetSeparationForce();
            OnMovementInput?.Invoke(direction.normalized);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Character player = collision.GetComponent<Character>();
            if (player != null) 
            {
                player.TakeDamage(damage);
            }
            //collision.GetComponent<Character>().TakeDamage(damage);
            ////调用玩家受击函数
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
            float distance= diff.magnitude;
            if (distance > 0f) 
            {
                force += diff.normalized / distance;
            }
        }
        return force*separationForceScale;
    }

}
