using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoomerAttribute : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput;

    public UnityEvent OnAttack;

    [SerializeField] public Transform Player;

    [SerializeField] public float attackDistance = 0.8f;
    void Update()
    {
        if (Player == null)
            return;
        float distance = Vector2.Distance(Player.position, transform.position);
        if (distance <= attackDistance)
        {
            //�������
            OnMovementInput?.Invoke(Vector2.zero);//ͣ��ԭ��
            OnAttack?.Invoke();//boom.....
        }
        else
        {
            //����׷��
            Vector2 direction = Player.position - transform.position;
            OnMovementInput?.Invoke(direction.normalized);
        }
    }
   
}
