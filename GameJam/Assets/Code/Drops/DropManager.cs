using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public static DropManager Instance { get; private set; }

    [Header("公共掉落表")]
    public List<DropItems> dropItems;
    [Range(0, 1f)]
    public float globalDropChange = 1f;//全局掉落率

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);//防止挂用了多个DropManager
    }

    //掉落方法，加权随机
    public void Drop(Vector3 posion)
    {
        if (Random.value > globalDropChange) return;//概率不掉落

        int totalWeight = 0;
        foreach (var item in dropItems)
        {
            totalWeight += item.weight;
        }

        if (totalWeight == 0) return;//未分配权重

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


//构建结构体，设置权重
[System.Serializable]
public class DropItems
{
    public GameObject prefab;
    [Range(0, 100)]
    public int weight;
}