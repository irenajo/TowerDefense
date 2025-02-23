using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    // Objects
    BoxCollider2D enemyCollider;

    // Parameters - Enemy

    [SerializeField] private int health; // = 100;
    [SerializeField] private float speed; //= 5.0f;
    [SerializeField] private int damageToPlayer; // = 10;

    [SerializeField] private int coinsWorth; // = 10;
    [SerializeField] private int width; //= 100;
    [SerializeField] private int height; // = 100;

    // Parameters - Moving
    [SerializeField] private GridManager _tileManager;

    public Vector2 startPosition = Vector2.zero;
    private Tile movingTowardsTile; // Postavlja se u Idle ako se nadje sledeci target tile. Proverava se u Moving radi kretanja
    private EnemyTile currentTile;

    private int start_x = 0;
    private int start_y = 0;

    private enum EnemyState
    {
        MovingStart,
        Idle,
        Moving,
        AtTarget,
        Destroyed
    }

    private EnemyState currentState = EnemyState.MovingStart;

    // Constructors

    public Enemy(int speed, int width, int height)
    {
        this.speed = speed;
        this.width = width;
        this.height = height;
        this.damageToPlayer = 5;
        this.health = 100;
        this.coinsWorth = 2;
    }

    public Enemy(int speed, int width, int height, int damage, int health, int coinsWorth)
    {
        this.speed = speed;
        this.width = width;
        this.height = height;
        this.damageToPlayer = damage;
        this.health = health;
        this.coinsWorth = coinsWorth;
    }

    /// <summary>
    /// Must be called after instantiating an enemy object. This sets the first file of the enemy path this enemy should follow.
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="_tileManager"></param>
    public void Init(Vector2 startPosition, GridManager _tileManager)
    {
        this._tileManager = _tileManager;
        this.startPosition = startPosition;

        Debug.LogWarning("Initialized tank");

        if (_tileManager == null)
        {
            Debug.Log("TileManager is null");
        }
        if (_tileManager.GetTileAtPosition((int)startPosition.x, (int)startPosition.y) == null)
        {
            Debug.Log("Starting tile Error");
        }


    }

    private void Update()
    {
        Move();
    }

    /// <summary>
    /// TO IMPLEMENT: If a Weapon hits the enemy, the enemy takes damage.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Weapon weapon = other.GetComponent<Weapon>();
        // if (weapon != null)  // If the object is a weapon that deals damage to enemy
        // {
        //     Debug.Log("A Weapon/Bullet object entered and dealt 5 damage!");
        //     this.TakeDamage(5);
        // }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currentState = EnemyState.Destroyed;
        }
    }

    void Start()
    {
        enemyCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        enemyCollider.size = new Vector2(width, height);
        sprite.size = new Vector2(width, height);
    }

    //todo -> friend or something? this should be private.
    /// <summary>
    /// If enemy is idle, locates the next tile to move to and sets the state to Moving. When enemy reaches movingTowardsTile, sets state to Idle.
    /// </summary>
    public void Move()
    {
        if (currentState == EnemyState.MovingStart)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            // Debug.LogWarning("Current position: " + transform.position.x + " " + transform.position.y);

            if (transform.position.x == startPosition.x && transform.position.y == startPosition.y)
            {
                currentState = EnemyState.Idle;
                start_x = (int)transform.position.x;
                start_y = (int)transform.position.y;
                var getTile = _tileManager.GetTileAtPosition(start_x, start_y);
                if (getTile == null)
                {
                    Debug.Log("getTile is null");
                }
                else if (getTile is not EnemyTile)
                {
                    Debug.Log("getTile is not Enemy tile");
                }
                else
                {
                    currentTile = (EnemyTile)getTile;
                }
            }
        }

        if (currentState == EnemyState.AtTarget)
        {
            EventBus.Instance.PlayerDamaged(damageToPlayer); //random coins for now, TODO move to each enemy!
            Destroy(gameObject);
            Debug.Log("Enemy reached target tile");
            return;
        }

        if (currentState == EnemyState.Destroyed)
        {
            EventBus.Instance.EnemyKilled(coinsWorth); //todo change to enemy value
            Destroy(gameObject);
            Debug.Log("Enemy destroyed");
            return;
        }


        if (currentState == EnemyState.Idle)
        {
            // Debug.LogWarning(start_x + " " + (int)currentTile.getMoveTo().x + " " + start_y + " " + (int)currentTile.getMoveTo().y);
            Tile nextTile = _tileManager.GetTileAtPosition(start_x + (int)currentTile.getMoveTo().x, start_y + (int)currentTile.getMoveTo().y);
            bool selected = false;

            if (nextTile == null)
            {
                Debug.Log("Didn't find a tile.");
                return;
            }

            if (nextTile is EnemyTile)
            {
                // currentTile = (EnemyTile)nextTile;
                movingTowardsTile = nextTile;
                start_x = (int)nextTile.transform.position.x;
                start_y = (int)nextTile.transform.position.y;
                currentState = EnemyState.Moving;
                selected = true;
            }
            else if (nextTile is TargetTile)
            {
                Debug.Log("Enemy reached target tile");
                currentState = EnemyState.AtTarget;
                selected = true;
            }

            // debug
            if (!selected)
            {
                if (!movingTowardsTile)
                {
                    Debug.Log("Enemy is not moving towards anything");
                }
                else
                {
                    Debug.Log("Enemy is in fact not moving towards: " + movingTowardsTile.transform.position);
                }
            }

        }

        if (currentState == EnemyState.Moving)
        {
            if (movingTowardsTile != null)
            {
                Vector2 targetPosition = movingTowardsTile.GetCenterPosition();
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
                {
                    currentState = EnemyState.Idle;
                    start_x = (int)transform.position.x;
                    start_y = (int)transform.position.y;
                    var getTile = _tileManager.GetTileAtPosition(start_x, start_y);
                    if (getTile == null)
                    {
                        Debug.Log("getTile is null");
                    }
                    else if (getTile is not EnemyTile)
                    {
                        Debug.Log("getTile is not Enemy tile");
                    }
                    else
                    {
                        currentTile = (EnemyTile)getTile;
                    }
                }
            }
        }
    }

}


// IDEA 1: explained in the Move() function
// private List<Vector2> path = new List<Vector2>
// {
//     new Vector2(0, 0),
//     new Vector2(1, 0),
//     new Vector2(2, 0)
// };


// IDEA 1: FOLLOW A PREDETERMINED PATH OF VECTOR2. IF YOU REACH THE END OF THE PATH, DESTROY THE TANK AND DEAL DAMAGE TO THE TARGET TILE/PLAYER
// if currTile < len(path){
//     Vector2 targetPosition = path[currTile+1];
//     Vector2 currentPosition = transform.position;

//     if currentPosition.x - targetPosition.x != 0 or currentPosition.y - targetPosition.y != 0:
//         // move 
//         transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
//     else:
//         currTile += 1;
// }
// else{
//     // destroy tank and deal damage to target tile/player
// }