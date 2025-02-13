using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    // structure that contains enemyPrefab, amount of enemies, cooldown between each enemy instantiation and cooldown between the next unit spawn
    [SerializeField] private Vector2 _firstTilePosition;

    [SerializeField] private EnemyManager _enemyManager;

    [SerializeField] private GridManager _gridManager; // todo remove put in enemyManager

    [SerializeField] private GameObject _tankPrefab;
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

    SpawnGroup[] testArray;


    void Start()
    {
        testArray = new SpawnGroup[]
            {
            new SpawnGroup(_tankPrefab, 5, 2.0f, 5.0f),
            new SpawnGroup(_tankPrefab, 3, 2.0f, 6.0f),
            new SpawnGroup(_tankPrefab, 4, 1.5f, 4.0f)
            };

        Debug.LogWarning("testArray " + testArray[0].amount);
        // SpawnWave(0);
        StartCoroutine(SpawnWaveCoroutine(0));
    }


    // generated code 
    private int currentWave = 0;
    private int currentGroup = 0;
    private int currentEnemy = 0;
    private Timer groupTimer;
    private Timer spawnTimer;

    void Update()
    {
        if (currentWave < testArray.Length)
        {
            if (groupTimer == null || groupTimer.Ready())
            {
                if (currentEnemy < testArray[currentGroup].amount)
                {
                    if (spawnTimer == null || spawnTimer.Ready())
                    {
                        SpawnEnemy(currentGroup, currentEnemy);
                        currentEnemy++;
                        spawnTimer = new Timer(testArray[currentGroup].cooldownBetweenUnit);
                        spawnTimer.Start();
                    }
                    else
                    {
                        spawnTimer.Update();
                    }
                }
                else
                {
                    currentGroup++;
                    currentEnemy = 0;
                    if (currentGroup < testArray.Length)
                    {
                        groupTimer = new Timer(testArray[currentGroup].cooldownBetweenSpawn);
                        groupTimer.Start();
                    }
                    else
                    {
                        currentWave++;
                        currentGroup = 0;
                    }
                }
            }
            else
            {
                groupTimer.Update();
            }
        }
    }

    private void SpawnEnemy(int groupIndex, int enemyIndex)
    {
        GameObject obj = Instantiate(testArray[groupIndex].enemyPrefab, transform.position, Quaternion.identity);
        Enemy spawnedEnemy = obj.GetComponent<Tank>();

        spawnedEnemy.name = $"Enemy {groupIndex} {enemyIndex}";
        spawnedEnemy.transform.SetParent(_enemyManager.transform);
        spawnedEnemy.Init(_firstTilePosition, _gridManager);
        Debug.LogWarning("spawned enemy first position " + spawnedEnemy.startPosition);
    }

    // public void SpawnWave(int wave)
    // {
    //     for (int i = 0; i < 1; i++) // testArray.Length
    //     {
    //         // iterate through each spawn group in the wave, and after spawning the i-th group, wait for cooldownBetweenSpawn seconds.
    //         Timer groupTimer = new Timer(testArray[i].cooldownBetweenUnit);
    //         groupTimer.Start();
    //         Debug.LogWarning("timer " + groupTimer.Ready());

    //         for (int j = 0; j < testArray[i].amount; j++)
    //         {
    //             while (!groupTimer.Ready())
    //             {
    //                 groupTimer.Update(); // is this called on this own, do i have to call it manually?
    //             }

    //             // INSTANTIATE ENEMY, SET IT'S SPAWN POSITION AND TARGET, AND ADD IT TO THE ENEMYMANAGER AS ITS CHILD

    //             GameObject obj = Instantiate(testArray[i].enemyPrefab, transform.position, Quaternion.identity); // would dis work? var?
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
    //     for (int i = 0; i < testArray.Length; i++)
    //     {
    //         for (int j = 0; j < testArray[i].amount; j++)
    //         {
    //             GameObject obj = Instantiate(testArray[i].enemyPrefab, transform.position, Quaternion.identity);
    //             Enemy spawnedEnemy = obj.GetComponent<Tank>();

    //             spawnedEnemy.name = $"Enemy {i} {j}";
    //             spawnedEnemy.transform.SetParent(_enemyManager.transform);
    //             spawnedEnemy.Init(_firstTilePosition, _gridManager);
    //             Debug.LogWarning("spawned enemy first position " + spawnedEnemy.startPosition);

    //             yield return new WaitForSeconds(testArray[i].cooldownBetweenUnit);
    //         }
    //         yield return new WaitForSeconds(testArray[i].cooldownBetweenSpawn);
    //     }
    // }
}

// }