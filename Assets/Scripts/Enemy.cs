using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    // Objects
    BoxCollider2D collider;

    // Parameters - Enemy

    [SerializeField] private int health; // = 100;
    [SerializeField] private float speed; //= 5.0f;
    [SerializeField] private int damageToObject; // = 10;
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
        Destroyed
    }

    private EnemyState currentState = EnemyState.MovingStart;

    // Constructors

    public Enemy(int speed, int width, int height)
    {
        this.speed = speed;
        this.width = width;
        this.height = height;
        this.damageToObject = 5;
        this.health = 100;
    }

    public Enemy(int speed, int width, int height, int damage, int health)
    {
        this.speed = speed;
        this.width = width;
        this.height = height;
        this.damageToObject = damage;
        this.health = health;
    }

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // TODO check if null ???
        collider = GetComponent<BoxCollider2D>();

        collider.size = new Vector2(width, height);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        sprite.size = new Vector2(width, height);

        // SET STARTING TILE !!

        // Tile getTile = _tileManager.GetTileAtPosition(start_x, start_y);
        // if (getTile == null)
        // {
        //     Debug.Log("Starting tile is null");
        // }
        // if (getTile is not EnemyTile)
        // {
        //     Debug.Log("Starting tile is not an EnemyTile");
        // }

        // currentTile = (EnemyTile)getTile;
    }

    void Move()
    {
        if (currentState == EnemyState.MovingStart)
        {

            transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            Debug.LogWarning("Current position: " + transform.position.x + " " + transform.position.y);

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


        if (currentState == EnemyState.Idle)
        {
            // EnemyTile currentTile = (EnemyTile)_tileManager.GetTileAtPosition(start_x, start_y);
            // if (currentTile == null)
            // {
            //     Debug.Log("Current tile is null");
            //     return;
            // }

            Debug.LogWarning(start_x + " " + (int)currentTile.getMoveTo().x + " " + start_y + " " + (int)currentTile.getMoveTo().y);
            Tile nextTile = _tileManager.GetTileAtPosition(start_x + (int)currentTile.getMoveTo().x, start_y + (int)currentTile.getMoveTo().y);
            bool selected = false;

            Debug.Log(start_x + " " + start_y);
            if (nextTile != null && nextTile is EnemyTile)
            {
                // currentTile = (EnemyTile)nextTile;
                movingTowardsTile = nextTile;
                start_x = (int)nextTile.transform.position.x;
                start_y = (int)nextTile.transform.position.y;
                currentState = EnemyState.Moving;
                selected = true;
            }

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
    // Update is called once per frame
    void Update()
    {
        Move();

        // SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        // sprite.size = new Vector2(width, height);

        // switch (currentState)
        // {
        //     case EnemyState.Idle:
        //         Debug.Log("Tank State: IDLE");
        //         break;
        //     case EnemyState.Moving:
        //         Debug.Log("Tank State: MOVING");
        //         break;
        //     case EnemyState.Attacking:
        //         Debug.Log("Tank State: ATTACKING");
        //         break;
        //     case EnemyState.Destroyed:
        //         Debug.Log("Tank State: DESTROYED");
        //         break;
        // }

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