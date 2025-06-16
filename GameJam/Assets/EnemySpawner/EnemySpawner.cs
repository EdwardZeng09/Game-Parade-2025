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


    [SerializeField] public float spawnDelay = 1f;
    [SerializeField] public float spawnTimer = 0f;

    [SerializeField] public int Num;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cell))
                {
                    Vector3 worldPos = tilemap.GetCellCenterWorld(cell);
                    TileWorldPos.Add(worldPos);
                }
            }
        }

        //初始化数组


        TileCount = TileWorldPos.Count;
        EnemyCount = Enemies.Count;
    }

    // Update is called once per frame
    void Update()
    {
        //每隔一段时间
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0f)
        {
            int i = 1;
            for (; i <= Num; i++)
            {
                //随机选择一个地点
                int aRandomTile = Random.Range(0, TileCount);
                Vector3 spawnPos = TileWorldPos[aRandomTile];

                //随机选择一个敌人
                int aRandomEnemy = Random.Range(0, EnemyCount);
                GameObject spawnRes = Enemies[aRandomEnemy];

                //生成敌人
                Instantiate(spawnRes, spawnPos, Quaternion.identity);
                spawnTimer = spawnDelay;
            }
            i = 1;

        }


    }
}
