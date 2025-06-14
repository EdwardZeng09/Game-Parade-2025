using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,IDamageable
{
    [Header("����")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;//protected��ζ��ֻ�е�ǰ��ͼ̳����������Ե���

    [Header("�޵�")]
    public bool invulnerable;
    public float invulnerableDuration;//�޵�ʱ��
    public UnityEvent OnHurt;
    public UnityEvent OnDeath;

    //public bool isDead=false;
    public bool IsDead => currentHealth <= 0;
    protected virtual void OnEnable()//virtual�����ʾ������ԶԸ÷���������д 
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        if (invulnerable)
            return;
        if (currentHealth - amount > 0f)
        {
            currentHealth -= amount;
            StartCoroutine(nameof(InvulnerableCoroutine));//�����޵�ʱ��Э��
            //ִ�н�ɫ���˶���
            OnHurt?.Invoke();//�ж�OnHurt�¼��Ƿ�Ϊ�գ�����Ϊ����ִ��

        }
        else
        {
            //isDead = true;
            OnDeath?.Invoke();
            Die();
            //��ɫ����
        }
    }
    public void Heal(float amount) 
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    public virtual void Die()
    {
        currentHealth = 0f;
        //ִ�н�ɫ����������ݻٶ���
        
    }


    //�����޵�ʱ���Э��
    protected virtual IEnumerator InvulnerableCoroutine()
    {
        invulnerable = true;
        //�ȴ��޵�ʱ��
        yield return new WaitForSeconds(invulnerableDuration);
        invulnerable = false;
    }
}
