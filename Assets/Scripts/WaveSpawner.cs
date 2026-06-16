using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [Header("Префабы мобов от слабых к сильным")]
    public GameObject[] enemyPrefabs;  

    [Header("Точка появления")]
    public Transform spawnPoint;

    [Header("Сложность")]
    public int baseEnemies = 5;              
    public float enemiesPerWave = 1.5f;      
    public float healthScalePerWave = 0.15f; 
    public float goldScalePerWave = 0.07f;    
    public float spawnInterval = 0.7f;       
    public float timeBetweenWaves = 5f;      

    [Header("UI")]
    public Text waveText;

    public static int enemiesAlive = 0;
    public static int CurrentWave = 0;   

    private int waveNumber = 0;

    void Start()
    {
        enemiesAlive = 0;
        CurrentWave = 0;
        UpdateWaveUI();
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        yield return new WaitForSeconds(2f);

        while (true)                 
        {
            if (GameManager.Instance.IsGameOver) yield break;

            waveNumber++;
            CurrentWave = waveNumber;
            UpdateWaveUI();

            yield return StartCoroutine(SpawnWave(waveNumber));

            
            while (enemiesAlive > 0)
            {
                if (GameManager.Instance.IsGameOver) yield break;
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnWave(int wave)
    {
        int count = baseEnemies + Mathf.RoundToInt((wave - 1) * enemiesPerWave);
        float healthMult = 1f + (wave - 1) * healthScalePerWave;
        float goldMult = 1f + (wave - 1) * goldScalePerWave;

        int maxType = Mathf.Min(enemyPrefabs.Length - 1, (wave - 1) / 2);

        for (int i = 0; i < count; i++)
        {
            int typeIndex = Random.Range(0, maxType + 1);
            SpawnEnemy(enemyPrefabs[typeIndex], healthMult, goldMult);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(GameObject prefab, float healthMult, float goldMult)
    {
        if (prefab == null || spawnPoint == null) return;

        GameObject go = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Enemy e = go.GetComponent<Enemy>();
        if (e != null) e.Scale(healthMult, goldMult);

        enemiesAlive++;
    }

    void UpdateWaveUI()
    {
        if (waveText) waveText.text = "" + Mathf.Max(waveNumber, 1);
    }
}