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

    //public GameObject projectile;//��Ļ���
    //public Transform shotPoint;

    void Start()
    {

    }

    void Update()
    {
        if (Player == null)
            return;
        float distance = Vector2.Distance(Player.position, transform.position);
        if (distance <= attackDistance)
        {
            //�������
            OnMovementInput?.Invoke(Vector2.zero);
            OnAttack?.Invoke();
        }
        else
        {
            //����׷��
            Vector2 direction = Player.position - transform.position;
            OnMovementInput?.Invoke(direction.normalized);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character>().TakeDamage(damage);
            //��������ܻ�����
        }
    }
}
