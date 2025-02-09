using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{

    private Dictionary<Vector2, Tile> _tiles = new Dictionary<Vector2, Tile>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Tile "))
            {
                string[] parts = child.name.Split(' ');
                if (parts.Length == 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
                {
                    Vector2 pos = new Vector2(x, y);
                    Tile tile = child.GetComponent<Tile>();
                    if (tile != null)
                    {
                        _tiles[pos] = tile;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}
