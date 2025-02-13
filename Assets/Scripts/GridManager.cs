using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private TurretTile _turretTilePrefab;

    [SerializeField] private EnemyTile _enemyTilePrefab;

    [SerializeField] private Transform _cam;

    [SerializeField] private UnityEngine.GameObject _gridManager;



    private Dictionary<Vector2, Tile> _tiles;

    // void Start()
    // {
    //     GenerateGrid();
    //     Debug.Log(_tiles);
    // }

    void Awake()
    {
        GenerateGrid();
    }

    void Start()
    {
        Debug.Log(_tiles);
    }


    Tile generateTile(int x, int y)
    {
        return Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
    }

    Tile generateEnemyTile(int x, int y, Vector2 moveTo)
    {
        EnemyTile tile = Instantiate(_enemyTilePrefab, new Vector3(x, y), Quaternion.identity);
        tile.setMoveTo(moveTo);
        return tile;
    }

    Tile generateTurretTile(int x, int y)
    {
        return Instantiate(_turretTilePrefab, new Vector3(x, y), Quaternion.identity);
    }


    void GenerateGrid()
    {

        // for each character in each line, depending on the character, generate the appropriate tile type 

        // Open test matrix directory, in the "Other" folder
        string path = "Assets/Other/spiral.txt";
        string[] lines = System.IO.File.ReadAllLines(path);

        Dictionary<char, Vector2> moveTo = new Dictionary<char, Vector2>{
            { '>', new Vector2(1, 0) },
            { '<', new Vector2(-1, 0) },
            { '^', new Vector2(0, 1) },
            { 'v', new Vector2(0, -1) }
        }; //

        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            // 
            for (int y = 0; y < _height; y++)
            {
                // For each character in each line, depending on the character, generate the appropriate tile type
                char tileType = lines[_height - 1 - y][x];
                Tile spawnedTile;

                // Tile generation based on type
                switch (tileType)
                {
                    case '0':
                        spawnedTile = generateTurretTile(x, y);
                        break;
                    case '>':
                    case '^':
                    case '<':
                    case 'v':
                        spawnedTile = generateEnemyTile(x, y, moveTo[tileType]);
                        break;
                    default:
                        spawnedTile = generateTile(x, y);
                        break;
                }

                spawnedTile.name = $"Tile {x} {y}";
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                spawnedTile.transform.SetParent(_gridManager.transform);

                _tiles[new Vector2(x, y)] = spawnedTile;

                // Debug.Log(tilePrefabToUse);
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        Debug.Log(pos);
        Debug.Log(_tiles);
        // Debug.Log("Tile looking for: ", _tiles[pos]);
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        Debug.Log("Tile not found");
        return null;
    }
}
