using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public Snake snake;
    public Snake snake2;


    private void Start()
    {
        RandomizePosition();

    }

    private void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RandomizePosition();
            snake.Grow();


        }
        if (collision.tag == "Player2")
        {
            RandomizePosition();
            snake2.Grow();

        }
    }
}
