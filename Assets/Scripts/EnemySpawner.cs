using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private Vector2 _firstTilePosition;

    // [SerializeField] private EnemyManager _enemyManager;

    [SerializeField] private GridManager _gridManager;

    [SerializeField] private GameObject _tankPrefab;

    [ContextMenu("Set Wave ID to 0")]
    public void SetWaveManually()
    {
        waveState.setWaveId(0);
    }

    [ContextMenu("Start Wave")]
    public void StartWaveManually()
    {
        waveState.startWave();
    }


    /// <summary>
    /// A structure that contains enemyPrefab, amount of enemies, cooldown between each enemy instantiation and cooldown between the next unit spawn
    /// </summary>
    struct SpawnGroup
    {
        public GameObject enemyPrefab;
        public int amount;
        public float cooldownBetweenUnit;
        public float cooldownBetweenSpawn;

        public SpawnGroup(GameObject enemyPrefab, int amount, float cooldownBetweenUnit, float cooldownBetweenSpawn)
        {
            this.enemyPrefab = enemyPrefab;
            this.amount = amount;
            this.cooldownBetweenUnit = cooldownBetweenUnit;
            this.cooldownBetweenSpawn = cooldownBetweenSpawn;
        }
    }

    /// <summary>
    /// A structure that explains the current state of the wave.
    /// </summary>
    public class WaveState
    {
        public int waveId;
        public bool waveRunning;
        public int currGroup;
        public int currAmount;
        public Timer groupTimer;
        public Timer spawnTimer;

        public bool waitForGroup;

        public WaveState()
        {
            this.waveId = -1;
            this.waveRunning = false;
            this.currGroup = 0;
            this.currAmount = 0;
            this.groupTimer = new Timer(0);
            this.spawnTimer = new Timer(0);
            this.waitForGroup = true;
        }

        public void startWave()
        {
            this.waveRunning = true;
        }
        public void setWaveId(int waveId)
        {
            this.waveId = waveId;
        }
        public void endWave()
        {
            Debug.Log("Wave " + this.waveId + " ended.");
            this.waveRunning = false;
            this.currGroup = 0;
            this.currAmount = 0;
            this.groupTimer.setCooldownAndRestart(0);
            this.waitForGroup = true;
            this.waveId = -1;
        }
    }

    public WaveState waveState;
    SpawnGroup[][] waves;

    /// <summary>
    /// Loads each of the waves with the appropriate spawn groups. Waves are 0-indexed.
    /// </summary>
    void Start()
    {
        waveState = new WaveState();
        waves = new SpawnGroup[][]
        {
            new SpawnGroup[]
            {
                new SpawnGroup(_tankPrefab, 3, 1.0f, 3.0f),
                new SpawnGroup(_tankPrefab, 2, 0.5f, 6.0f),
                new SpawnGroup(_tankPrefab, 10, 0.1f, 3.0f)
            },
            new SpawnGroup[]
            {
                new SpawnGroup(_tankPrefab, 5, 2.0f, 5.0f),
                new SpawnGroup(_tankPrefab, 3, 2.0f, 6.0f),
                new SpawnGroup(_tankPrefab, 4, 1.5f, 4.0f)
            }
        };

        Debug.LogWarning("waves " + waves[0][0].amount);

    }


    // generated code 


    void Update()
    {
        if (waveState.waveRunning)
        {
            UpdateWave();
        }
        // else
        // {
        //     Debug.Log("No wave is running.");
        // }
    }

    /// <summary>
    /// Starts the wave with given id which has to be set before calling this function.
    /// </summary>
    /// <param name="waveId"></param>
    public void startWave(int waveId)
    {
        if (waveState.waveRunning)
        {
            Debug.LogError("Wave " + waveState.waveId + " is already running.");
            return;
        }
        waveState.startWave();
        waveState.setWaveId(waveId);
    }


    /// <summary>
    /// Spawns an enemy and adds it as a child to the enemyManager.
    /// </summary>
    /// <param name="groupIndex"></param>
    /// <param name="enemyIndex"></param>
    private void SpawnEnemy(int groupIndex, int enemyIndex)
    {
        GameObject obj = Instantiate(waves[waveState.waveId][groupIndex].enemyPrefab, transform.position, Quaternion.identity);
        Enemy spawnedEnemy = obj.GetComponent<Tank>();

        spawnedEnemy.name = $"Enemy {groupIndex} {enemyIndex}";
        spawnedEnemy.transform.SetParent(this.transform);
        spawnedEnemy.Init(_firstTilePosition, _gridManager);
        // _enemyManager.AddEnemy(spawnedEnemy); MOZDA ZA STATISTIKU?
    }

    /// <summary>
    /// If wave is running, this function regulates the waveState object and spawns enemies.
    /// </summary>
    public void UpdateWave()
    {
        //not sure where to put this check
        if (waveState.waveId < 0 || waveState.waveId > waves.Length - 1)
        {
            Debug.LogError("Wave " + waveState.waveId + " is running but does not exist.");
        }

        // Debug.Log("Wave " + waveState.waveId + " is running.");

        var waveGroups = waves[waveState.waveId];


        if (waveState.currGroup < waveGroups.Length)
        {
            if (waveState.waitForGroup)
            {
                if (waveState.groupTimer.Ready())
                {
                    waveState.waitForGroup = false;
                    waveState.spawnTimer.setCooldownAndRestart(waveGroups[waveState.currGroup].cooldownBetweenUnit);
                }
                else
                {
                    waveState.groupTimer.Update();
                }
                return;
            }

            if (waveState.currAmount < waveGroups[waveState.currGroup].amount)
            {
                if (waveState.spawnTimer.Ready())
                {
                    SpawnEnemy(waveState.currGroup, waveState.currAmount);
                    waveState.currAmount++;
                }
                else
                {
                    waveState.spawnTimer.Update();
                    // if (waveState.spawnTimer != null) Debug.Log("spawnTimer " + waveState.spawnTimer.Read());  
                }
            }
            else
            {
                float waitNext = waveGroups[waveState.currGroup].cooldownBetweenSpawn;
                waveState.currGroup++;
                waveState.currAmount = 0;
                waveState.waitForGroup = true;
                if (waveState.currGroup < waveGroups.Length)
                {
                    waveState.groupTimer.setCooldownAndRestart(waitNext);
                    // waveState.groupTimer.Start();
                    // waveState.spawnTimer.setCooldownAndRestart(waveGroups[waveState.currGroup].cooldownBetweenUnit);
                }
            }

        }
        else
        {
            waveState.endWave();
        }
    }

    // public void SpawnWave(int wave)
    // {
    //     for (int i = 0; i < 1; i++) // waveGroups.Length
    //     {
    //         // iterate through each spawn group in the wave, and after spawning the i-th group, wait for cooldownBetweenSpawn seconds.
    //         Timer groupTimer = new Timer(waveGroups[i].cooldownBetweenUnit);
    //         groupTimer.Start();
    //         Debug.LogWarning("timer " + groupTimer.Ready());

    //         for (int j = 0; j < waveGroups[i].amount; j++)
    //         {
    //             while (!groupTimer.Ready())
    //             {
    //                 groupTimer.Update(); // is this called on this own, do i have to call it manually?
    //             }

    //             // INSTANTIATE ENEMY, SET IT'S SPAWN POSITION AND TARGET, AND ADD IT TO THE ENEMYMANAGER AS ITS CHILD

    //             GameObject obj = Instantiate(waveGroups[i].enemyPrefab, transform.position, Quaternion.identity); // would dis work? var?
    //             Enemy spawnedEnemy = obj.GetComponent<Tank>();

    //             // set parameters here! (target position!)
    //             spawnedEnemy.name = $"Enemy {i} {j}";


    //             // IMPORTANT: the EnemyManager's position HAS TO BE (0,0) so that it doesn't mess around with the enemy's position
    //             spawnedEnemy.transform.SetParent(_enemyManager.transform);

    //             spawnedEnemy.Init(_firstTilePosition, _gridManager);
    //             Debug.LogWarning("spawned enemy first position " + spawnedEnemy.startPosition);

    //         }
    //     }
    // }

    // private IEnumerator SpawnWaveCoroutine(int wave)
    // {
    //     for (int i = 0; i < waveGroups.Length; i++)
    //     {
    //         for (int j = 0; j < waveGroups[i].amount; j++)
    //         {
    //             GameObject obj = Instantiate(waveGroups[i].enemyPrefab, transform.position, Quaternion.identity);
    //             Enemy spawnedEnemy = obj.GetComponent<Tank>();

    //             spawnedEnemy.name = $"Enemy {i} {j}";
    //             spawnedEnemy.transform.SetParent(_enemyManager.transform);
    //             spawnedEnemy.Init(_firstTilePosition, _gridManager);
    //             Debug.LogWarning("spawned enemy first position " + spawnedEnemy.startPosition);

    //             yield return new WaitForSeconds(waveGroups[i].cooldownBetweenUnit);
    //         }
    //         yield return new WaitForSeconds(waveGroups[i].cooldownBetweenSpawn);
    //     }
    // }
}

// }