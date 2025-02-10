using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tank : MonoBehaviour
{

    // Objects
    BoxCollider2D collider;

    // Parameters
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;

    [SerializeField] private GridManager _tileManager;

    private EnemyTile currentTile;

    // IDEA 1: explained in the Move() function
    // private List<Vector2> path = new List<Vector2>
    // {
    //     new Vector2(0, 0),
    //     new Vector2(1, 0),
    //     new Vector2(2, 0)
    // };

    [SerializeField] private Vector2 targetPosition = new Vector2(0, 0);

    private int start_x = 0;
    private int start_y = 0;

    private Tile movingTowardsTile = null;

    private enum TankState
    {
        Idle,
        Moving,
        Attacking,
        Destroyed
    }

    private TankState currentState = TankState.Idle;

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

    // LIST OF POSSIBLE MOVES, DESCENDING PRIORITY
    // private List<Vector2> moves = new List<Vector2> {
    //     new Vector2(1, 0), // right
    //     new Vector2(0, 1), // up
    //     new Vector2(0, -1), // down
    //     new Vector2(-1, 0) // left 
    // };

    void Move()
    {

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


        if (currentState == TankState.Idle)
        {
            EnemyTile currentTile = (EnemyTile)_tileManager.GetTileAtPosition(start_x, start_y);
            if (currentTile == null)
            {
                Debug.Log("Current tile is null");
                return;
            }

            Tile nextTile = _tileManager.GetTileAtPosition(start_x + (int)currentTile.getMoveTo().x, start_y + (int)currentTile.getMoveTo().y);
            bool selected = false;

            Debug.Log(start_x + " " + start_y);
            if (nextTile != null && nextTile is EnemyTile)
            {
                currentTile = (EnemyTile)nextTile;

                movingTowardsTile = nextTile;
                start_x = (int)nextTile.transform.position.x;
                start_y = (int)nextTile.transform.position.y;
                currentState = TankState.Moving;
                selected = true;
            }

            // foreach (Vector2 move in moves)
            // {
            //     Vector2 targetPosition = new Vector2(start_x + move.x, start_y + move.y);
            //     Debug.Log("Target position " + targetPosition);
            //     Tile tile = _tileManager.GetTileAtPosition(targetPosition);
            //     if (tile != null && tile is EnemyTile)
            //     {
            //         movingTowardsTile = tile;
            //         start_x = (int)targetPosition.x;
            //         start_y = (int)targetPosition.y;
            //         currentState = TankState.Moving;
            //         selected = true;
            //         break;
            //     }
            // }

            if (!selected)
            {
                if (!movingTowardsTile)
                {
                    Debug.Log("Tank is not moving towards anything");
                }
                else
                {
                    Debug.Log("Tank is in fact not moving towards: " + movingTowardsTile.transform.position);
                }
            }

            // special case ako ne nadjes prema kome ce da se pomeara[]
        }

        if (currentState == TankState.Moving)
        {
            if (movingTowardsTile != null)
            {
                Vector2 tileCenter = movingTowardsTile.GetCenterPosition();
                targetPosition = tileCenter;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
                {
                    currentState = TankState.Idle;
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
        //     case TankState.Idle:
        //         Debug.Log("Tank State: IDLE");
        //         break;
        //     case TankState.Moving:
        //         Debug.Log("Tank State: MOVING");
        //         break;
        //     case TankState.Attacking:
        //         Debug.Log("Tank State: ATTACKING");
        //         break;
        //     case TankState.Destroyed:
        //         Debug.Log("Tank State: DESTROYED");
        //         break;
        // }

    }


}
