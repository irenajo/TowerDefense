using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    public event Action<int> OnEnemyKilled;
    public event Action<int> OnPlayerDamaged;
    public event Action OnUpdateHealthUI;
    public event Action OnUpdateCoinsUI;
    public event Action OnWaveStart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void EnemyKilled(int coins) => OnEnemyKilled?.Invoke(coins);
    public void PlayerDamaged(int damage) => OnPlayerDamaged?.Invoke(damage);

    public void UpdateHealthUI() => OnUpdateHealthUI?.Invoke();
    public void UpdateCoinsUI() => OnUpdateCoinsUI?.Invoke();

    public void waveStart() => OnWaveStart?.Invoke();
}
