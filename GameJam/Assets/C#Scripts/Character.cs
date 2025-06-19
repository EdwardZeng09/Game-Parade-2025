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


    [SerializeField] public GameObject damageTextPrefab;
    [SerializeField] public string canvasName = "PlayerUI";
    public Canvas worldCanvas;

    protected virtual void Awake()
    {
        GameObject foundCanvasObj = GameObject.Find(canvasName);
        if (foundCanvasObj != null) 
        {
        worldCanvas = foundCanvasObj.GetComponent<Canvas>();
            Debug.Log("1");
        }
            
        else
            Debug.LogError("δ�ҵ���Ϊ " + canvasName + " �Ļ�����");
    }

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
            Debug.Log("2");
            ShowDamageText(amount);
            
            StartCoroutine(nameof(InvulnerableCoroutine));//�����޵�ʱ��Э��
           
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


    public void ShowDamageText(float damage)
    {
        try
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnPos);

            GameObject obj = Instantiate(damageTextPrefab, worldCanvas.transform); // ��Ҫ��λ��
            obj.transform.position = screenPos; // ʹ����Ļ��������λ��

            var dt = obj.GetComponent<DamageText>();
            if (dt == null)
            {
                Debug.LogError(" DamageText �ű�û����Ԥ�����ϣ�");
                return;
            }

            dt.SetText(damage.ToString("F0"));
            Debug.Log(" �ɹ����� SetText");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("ShowDamageText ִ�г���" + ex.Message);
        }
    }
}
