using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{

    public BoxCollider2D gridArea;
    public bool isSpeedUpActive;
    private float powerUpDuration = 7f;
    public Snake snake;

    private void Start()
    {
        RandomizePosition();
    }

    public void ActivateSpeedUp()
    {
        StartCoroutine(DeactivateSpeedUp());
        isSpeedUpActive = true;
    }

    private IEnumerator DeactivateSpeedUp()
    {
        yield return new WaitForSeconds(powerUpDuration);
        isSpeedUpActive = false;
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
            ActivateSpeedUp();
        }
    }

}
