using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public GameObject spikes;
    public GameObject coins;

    private float spawnRangeX = 4.0f;
    private float spawnRangeY = 0f;
    private float spawnCoinsRangeY = 0.5f;
    private float spawnRangeZ = 4.0f;

    void Start()
    {
        int itemCount = 6;
        while (itemCount > 0)
        {
            SpawnSpikes();
            SpawnCoins();
            itemCount--;
        }
    }

    void SpawnSpikes()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);

        Vector3 spawnPosition = new Vector3(randomX, spawnRangeY, randomZ);

        Instantiate(spikes, spawnPosition, spikes.gameObject.transform.rotation);
    }

    void SpawnCoins()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);

        Vector3 spawnPosition = new Vector3(randomX, spawnCoinsRangeY, randomZ);

        Instantiate(coins, spawnPosition, coins.gameObject.transform.rotation);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
