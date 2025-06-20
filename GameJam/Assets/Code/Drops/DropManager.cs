using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public static DropManager Instance { get; private set; }

    [Header("���������")]
    public List<DropItems> dropItems;
    [Range(0, 1f)]
    public float globalDropChange = 1f;//ȫ�ֵ�����

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);//��ֹ�����˶��DropManager
    }

    //���䷽������Ȩ���
    public void Drop(Vector3 posion)
    {
        if (Random.value > globalDropChange) return;//���ʲ�����

        int totalWeight = 0;
        foreach (var item in dropItems)
        {
            totalWeight += item.weight;
        }

        if (totalWeight == 0) return;//δ����Ȩ��

        int rand = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var item in dropItems)
        {
            current += item.weight;
            if (rand < current)
            {
                Instantiate(item.prefab, posion, Quaternion.identity);
                break;
            }
        }

    }
}


//�����ṹ�壬����Ȩ��
[System.Serializable]
public class DropItems
{
    public GameObject prefab;
    [Range(0, 100)]
    public int weight;
}