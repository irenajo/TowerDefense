using UnityEngine;

public class EnemyTile : Tile
{

    private Vector2 MoveTo;

    public void setMoveTo(Vector2 moveTo)
    {
        MoveTo = moveTo;
    }

    public Vector2 getMoveTo()
    {
        return MoveTo;
    }

    // EnemyTile(Vector2 position, Vector2 moveTo) : base(position)
    // {
    //     MoveTo = moveTo;
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
