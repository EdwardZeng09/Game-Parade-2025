using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public Tilemap tilemap;
    [Header("怪物刷新配比")]
    public List<Enemies> enemies;
    [Range(0, 1f)]
    public float spawnChange = 1f;
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
        EnemyCount = enemies.Count;
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
                //刷新敌人
                SpawnEnemy(spawnPos);
            }

            spawnTimer = spawnDelay;
        }

    }


    //加权刷新敌人方法
    public void SpawnEnemy(Vector3 spawnPos)
    {
        if (Random.value > spawnChange)
            return;
        int totalWeight = 0;
        foreach (var enemy in enemies)
        {
            totalWeight += enemy.weight;
        }
        if (totalWeight == 0) return;//未给敌人添加权重

        int rand = Random.Range(0, totalWeight);
        int current = 0;
        foreach (var enemy in enemies)
        {
            current += enemy.weight;
            if (rand < current)
            {
                Instantiate(enemy.prefab, spawnPos, Quaternion.identity);
                break;
            }
        }
    }
    [System.Serializable]
    public class Enemies
    {
        public GameObject prefab;
        [Range(0, 100)]
        public int weight;
    }
}
