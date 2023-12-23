using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments;

    public  Transform segmentPrefab;

    private void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckWrap();
    }


    private void Move()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }


    private void CheckWrap()
    {
        Vector3 position = this.transform.position;


        // Get half of the screen's width
        float halfScreenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        // Wrap on the X-axis
        if (position.x > halfScreenWidth)
        {
            position.x = -halfScreenWidth;
        }
        else if (position.x < -halfScreenWidth)
        {
            position.x = halfScreenWidth;
        }


        // Wrap on the Y-axis
        if (position.y > Camera.main.orthographicSize)
        {
            position.y = -Camera.main.orthographicSize;
        }
        else if (position.y < -Camera.main.orthographicSize)
        {
            position.y = Camera.main.orthographicSize;
        }

        this.transform.position = position;
    }



    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    private void ResetState()
    {
         for(int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Grow();
        }else if(collision.tag == "Obstacle")
        {
            ResetState();
        }
    }
}