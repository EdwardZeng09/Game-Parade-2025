using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffButtonForDescription : MonoBehaviour
{
    [TextArea]
    public string description;
    public TextMeshProUGUI descriptionText;

    void Start()
    {
        // �Զ����ҳ�������Ϊ DescriptionText �� TextMeshProUGUI ���
        GameObject descObj = GameObject.Find("DescriptionText");
        if (descObj != null)
        {
            descriptionText = descObj.GetComponent<TextMeshProUGUI>();
        }
    }

    public void ShowDescription() 
    {
        if (descriptionText != null) 
        {
        descriptionText.text = description;
        
        }
    }
}
