using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ENEMY TILES:
// 1 4
// 2 4
// 3 4
// 4 4
// 4 5
public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    [SerializeField] public bool _enemyTile;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    // todo
    // what is center position if size is 1x1 ?
    public Vector2 GetCenterPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

}