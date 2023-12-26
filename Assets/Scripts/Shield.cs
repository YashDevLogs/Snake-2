using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public bool isShieldActive;
    private float powerUpDuration = 7f;
    public Snake snake;
    public Snake snake2;
    private void Start()
    {
        RandomizePosition();
    }

    public void ActivateShield()
    {
        isShieldActive = true;
        Debug.Log("Shield is active");
        StartCoroutine(DeactivateShield());
    }

    private IEnumerator DeactivateShield()
    {
        yield return new WaitForSeconds(powerUpDuration);
        isShieldActive = false;
        Debug.Log("Shield is inactive");
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
            ActivateShield();
        }
        if (collision.tag == "Player2")
        {
            RandomizePosition();
            ActivateShield();
        }
    }
}
