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

    }

    // LIST OF POSSIBLE MOVES, DESCENDING PRIORITY
    private List<Vector2> moves = new List<Vector2> {
        new Vector2(1, 0), // right
        new Vector2(0, 1), // up
        new Vector2(0, -1), // down
        new Vector2(-1, 0) // left 
    };

    void Move()
    {
        if (currentState == TankState.Idle)
        {
            bool selected = false;

            Debug.Log(start_x + " " + start_y);
            foreach (Vector2 move in moves)
            {
                Vector2 targetPosition = new Vector2(start_x + move.x, start_y + move.y);
                Debug.Log("Target position " + targetPosition);
                Tile tile = _tileManager.GetTileAtPosition(targetPosition);
                if (tile != null && tile is EnemyTile)
                {
                    movingTowardsTile = tile;
                    start_x = (int)targetPosition.x;
                    start_y = (int)targetPosition.y;
                    currentState = TankState.Moving;
                    selected = true;
                    break;
                }
            }

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
