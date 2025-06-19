using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,IDamageable
{
    [Header("属性")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;//protected意味着只有当前类和继承其的子类可以调用

    [Header("无敌")]
    public bool invulnerable;
    public float invulnerableDuration;//无敌时间
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
            Debug.LogError("未找到名为 " + canvasName + " 的画布！");
    }

    //public bool isDead=false;
    public bool IsDead => currentHealth <= 0;
    protected virtual void OnEnable()//virtual这里表示子类可以对该方法进行重写 
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
            
            StartCoroutine(nameof(InvulnerableCoroutine));//启动无敌时间协程
           
            OnHurt?.Invoke();//判断OnHurt事件是否为空，若不为空则执行

        }
        else
        {
            //isDead = true;
            OnDeath?.Invoke();
            Die();
            //角色死亡
        }
    }
    public void Heal(float amount) 
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    public virtual void Die()
    {
        currentHealth = 0f;
        //执行角色死亡动画后摧毁对象
        
    }


    //关于无敌时间的协程
    protected virtual IEnumerator InvulnerableCoroutine()
    {
        invulnerable = true;
        //等待无敌时间
        yield return new WaitForSeconds(invulnerableDuration);
        invulnerable = false;
    }


    public void ShowDamageText(float damage)
    {
        try
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnPos);

            GameObject obj = Instantiate(damageTextPrefab, worldCanvas.transform); // 不要带位置
            obj.transform.position = screenPos; // 使用屏幕坐标设置位置

            var dt = obj.GetComponent<DamageText>();
            if (dt == null)
            {
                Debug.LogError(" DamageText 脚本没挂在预制体上！");
                return;
            }

            dt.SetText(damage.ToString("F0"));
            Debug.Log(" 成功调用 SetText");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("ShowDamageText 执行出错：" + ex.Message);
        }
    }
}
