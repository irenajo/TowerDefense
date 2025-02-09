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

    [SerializeField] private TileManager _tileManager;

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

    private List<Vector2> moves = new List<Vector2> {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0)
    };

    void Move()
    {
        if (currentState == TankState.Idle)
        {
            bool selected = false;

            foreach (Vector2 move in moves)
            {
                Vector2 targetPosition = new Vector2(start_x + move.x, start_y + move.y);
                Tile tile = _tileManager.GetTileAtPosition(targetPosition);
                if (tile != null && tile._enemyTile)
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
                Debug.Log("Tank is in fact not moving towards: " + movingTowardsTile.transform.position);
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

        switch (currentState)
        {
            case TankState.Idle:
                Debug.Log("Tank State: IDLE");
                break;
            case TankState.Moving:
                Debug.Log("Tank State: MOVING");
                break;
            case TankState.Attacking:
                Debug.Log("Tank State: ATTACKING");
                break;
            case TankState.Destroyed:
                Debug.Log("Tank State: DESTROYED");
                break;
        }

    }


}
