using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] public int enemiesPerWave = 5;
    [SerializeField] public int Num;

    [Header("波次设定")]
    public int maxWave = 3;
    private int currentWave = 0;
    private bool isSpawning = false;
    private int aliveEnemies = 0;
    private bool isWaveFinished = false;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public GameObject upgradePanel;
    private bool waitingForUpgrade = false;
    private bool hasSelectedBuff = false;
    private bool hasSelectedDebuff = false;

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
        StartNextWave();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartNextWave()
    {
        currentWave++;
        if (currentWave > maxWave) return;

        waveText.text = $"Wave {currentWave} / {maxWave}";
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        isSpawning = true;
        isWaveFinished = false; // 重置波次完成标志

        for (int i = 0; i < enemiesPerWave; i++)
        {
            int randomTile = Random.Range(0, TileWorldPos.Count);
            Vector3 spawnPos = TileWorldPos[randomTile];
            SpawnEnemy(spawnPos); 
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
        isWaveFinished = true; 
    }

    void ShowUpgradePanel()
    {
        upgradePanel.SetActive(true);
        waitingForUpgrade = true;
        hasSelectedBuff = false;
        hasSelectedDebuff = false;
    }

    public void OnBuffSelected()
    {
        hasSelectedBuff = true;
        TryContinueWave();
    }

    public void OnDebuffSelected()
    {
        hasSelectedDebuff = true;
        TryContinueWave();
    }

    private void TryContinueWave()
    {
        if (hasSelectedBuff && hasSelectedDebuff)
        {
            upgradePanel.SetActive(false);
            waitingForUpgrade = false;
            StartNextWave();
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
                aliveEnemies++;
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

    public void OnEnemyKilled()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0 && isWaveFinished && currentWave < maxWave)
        {
            ShowUpgradePanel();
        }
    }
}
