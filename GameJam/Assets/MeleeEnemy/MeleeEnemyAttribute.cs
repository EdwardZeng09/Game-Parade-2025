using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeleeEnemyAttribute : Character
{
    public UnityEvent<Vector2> OnMovementInput;

    [SerializeField] public Transform Player;

    [SerializeField] public float damage;
 

    void Update()
    {
        if (Player == null)
            return;
        float distance = Vector2.Distance(Player.position, transform.position);
            Vector2 direction = Player.position - transform.position;
            OnMovementInput?.Invoke(direction.normalized);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character>().TakeDamage(damage);
            //调用玩家受击函数
        }
    }
}
