using UnityEngine;

public class GameManager : MonoBehaviour
{
    // fetches list of child spawners on start 

    // each spawner puts the enemies it spawns as its children -> and they communicate to their parent when they're destroyed.?


    // Wave parameters
    int waveNumber = 0; // levels will range from 0 to X and automatically update after each wave
    private EnemySpawner[] enemySpawners;


    // Player parameters
    int playerHealth = 100;
    int playerCoins = 0;

    void Start()
    {
        enemySpawners = GetComponentsInChildren<EnemySpawner>();
    }


    public void StartWave()
    {
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.startWave(waveNumber);
        }
    }
}

