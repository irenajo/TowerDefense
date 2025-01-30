using UnityEngine;

public class Tank : MonoBehaviour
{

    // Objects
    BoxCollider2D collider;

    // Parameters
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // TODO check if null ???
        collider = GetComponent<BoxCollider2D>();


        collider.size = new Vector2(width, height);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
