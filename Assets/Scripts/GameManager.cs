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

        EventBus.Instance.OnEnemyKilled += UpdateCoins;
        EventBus.Instance.OnPlayerDamaged += UpdateHealth;

    }

    private void OnDisable()
    {
        EventBus.Instance.OnEnemyKilled -= UpdateCoins;
        EventBus.Instance.OnPlayerDamaged -= UpdateHealth;
    }


    public void StartWave()
    {
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.startWave(waveNumber);
        }
        EventBus.Instance.waveStart();
        // waveNumber++;
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public int GetCoins()
    {
        return playerCoins;
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    void UpdateCoins(int coins)
    {
        if (playerCoins + coins < 0)
        {
            // insufficient funds, dont make purchase!
            return;
        }
        playerCoins += coins;
        // coinsLabel.text = "Coins: " + player.GetCoins();
        EventBus.Instance.UpdateCoinsUI();

    }

    void UpdateHealth(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            // Game Over
        }
        // healthLabel.text = "Health: " + player.GetHealth();
        EventBus.Instance.UpdateHealthUI();

    }
}

