using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public List<GameObject> Enemies = new List<GameObject>();

    private List<Vector3> TileWorldPos = new List<Vector3>();
    private int TileCount;
    private int EnemyCount;


    [SerializeField]public float spawnDelay=1f;
    [SerializeField]public float spawnTimer=0f;

    [SerializeField] public int Num;
    void Start()
    {
    tilemap= GetComponent<Tilemap>();
    Vector3Int tmOrg = tilemap.origin;
    Vector3Int tmSz= tilemap.size;

        //��ʼ������
        for (int x = tmOrg.x; x < tmSz.x; x++) 
        {
            for (int y = tmOrg.y; y < tmSz.y; y++)
            
            {
                Vector3 cellToWorldPos = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));//ת��Ϊ��������
                TileWorldPos.Add(cellToWorldPos);

            }
        }

        TileCount=TileWorldPos.Count;
        EnemyCount=Enemies.Count;
    }

    // Update is called once per frame
    void Update()
    {
        //ÿ��һ��ʱ��
        spawnTimer-=Time.deltaTime;
        if (spawnTimer < 0f) 
        {
            int i = 1;
            for (; i <=Num; i++) 
            {
             //���ѡ��һ���ص�
        int aRandomTile=Random.Range(0,TileCount);
        Vector3 spawnPos = TileWorldPos[aRandomTile];

        //���ѡ��һ������
        int aRandomEnemy=Random.Range(0,EnemyCount);
        GameObject spawnRes=Enemies[aRandomEnemy];

        //���ɵ���
        Instantiate(spawnRes,spawnPos,Quaternion.identity);
        spawnTimer=spawnDelay;
            }
            i = 1;
       
        }
       
     
    }
}
