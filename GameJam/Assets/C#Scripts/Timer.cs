using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] public float timeDelay = 60f;
    [SerializeField] public float remainingTime = 0f;

    [SerializeField] private TextMeshProUGUI timerText;
    private void Start()
    {
        remainingTime = timeDelay;
    }
    private void Update()
    {
        remainingTime-=Time.deltaTime;
        if (remainingTime < 0f) 
        {
            //������й���
           ClearAllEnemies();
        //��ת������ҳ��


        remainingTime = timeDelay;
        }
        timerText.text = "Time Left: "+Mathf.CeilToInt(remainingTime)+"s";
    }


    public void ClearAllEnemies() 
    {
     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies) 
            {
            Destroy(enemy);
            }
    }
}
