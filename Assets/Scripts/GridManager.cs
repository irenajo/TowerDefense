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

    void Start()
    {
        GenerateGrid();
        Debug.Log(_tiles);
    }

    void GenerateGrid()
    {

        // for each character in each line, depending on the character, generate the appropriate tile type 

        // Open test matrix directory, in the "Other" folder
        // string path = "Assets/Other/test_matrix.txt";
        string path = "Assets/Other/matrix_path.txt";
        string[] lines = System.IO.File.ReadAllLines(path);

        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            // 
            for (int y = 0; y < _height; y++)
            {
                // For each character in each line, depending on the character, generate the appropriate tile type
                char tileType = lines[y][x];
                Tile tilePrefabToUse;

                switch (tileType)
                {
                    case '0':
                        tilePrefabToUse = _turretTilePrefab;
                        break;
                    case '>':
                        tilePrefabToUse = _enemyTilePrefab;
                        break;
                    // case '.':
                    default:
                        tilePrefabToUse = _tilePrefab;
                        break;
                }


                // You can add more conditions here to choose different tile prefabs based on tileType

                var spawnedTile = Instantiate(tilePrefabToUse, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                spawnedTile.transform.SetParent(_gridManager.transform);

                _tiles[new Vector2(x, y)] = spawnedTile;

                Debug.Log(tilePrefabToUse);
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        Debug.Log(pos);
        Debug.Log("Tile looking for: ", _tiles[pos]);
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}