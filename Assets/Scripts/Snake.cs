using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments;

    public  Transform segmentPrefab;
    public PowerUp powerUpPrefabShield;
    public PowerUp powerUpPrefabScoreBoost;
    public PowerUp powerUpPrefabSpeedUp;
    public BoxCollider2D gridArea;
    public ScoreController score;
    public GameObject GamePausePanel;
    public GameObject GameOverPanel;
    public GameObject P1GameWon;
    public GameObject P2GameWon;
    private bool isGameOver = false;


    public float speed = 1.0f;

    private bool isShieldActive = false;
    private bool isScoreBoostActive = false;
    private bool isSpeedUpActive = false;
    private float powerUpDuration = 7.0f;
    private float powerUpCooldown = 3.0f;
    private float nextPowerUpTime;

    private void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);

        nextPowerUpTime = Time.time + Random.Range(5.0f, 10.0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            {
            Pause();
            GamePausePanel.SetActive(true);
        }

        // Check for power-up activation cooldown
        if (Time.time > nextPowerUpTime)
            {
            SpawnPowerUp();
            nextPowerUpTime = Time.time + Random.Range(10.0f, 20.0f);
            }
        if(tag == "Player") { 
              if (isGameOver == true)
              {
            return;
               }
        else { 

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
        }

        if (tag == "Player2")
        {
            if (isGameOver == true)
            {
                return;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && _direction != Vector2.down)
                {
                    _direction = Vector2.up;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up)
                {
                    _direction = Vector2.down;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right)
                {
                    _direction = Vector2.left;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && _direction != Vector2.left)
                {
                    _direction = Vector2.right;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckWrap();
    }

    private void Move()
    {
        float moveAmount = isSpeedUpActive ? speed * 2.0f : speed;

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x * moveAmount,
            Mathf.Round(this.transform.position.y) + _direction.y * moveAmount,
            0.0f
        );
    }
    private void CheckWrap()
    {
        Vector3 position = this.transform.position;

        float halfScreenWidth = 26.0f;
        float halfScreenHeight = 14.0f;

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
        if (position.y > halfScreenHeight)
        {
            position.y = -halfScreenHeight;
        }
        else if (position.y < -halfScreenHeight)
        {
            position.y = halfScreenHeight;
        }

        this.transform.position = position;
    }

    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    public void ReduceSize()
    {
        if (_segments.Count > 1) { 
            // Get the last segment and destroy it
        Transform lastSegment = _segments[_segments.Count - 1];
        Destroy(lastSegment.gameObject);

        // Remove the last segment from the list
        _segments.RemoveAt(_segments.Count - 1);
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void PlaySinglePlayer()
    {
       SceneManager.LoadScene(1);
    }
    public void PlayCoop()
    {
        SceneManager.LoadScene(2);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Level Restarted");
    }

    private void SpawnPowerUp()
    {
        // Randomly determine the type of power-up to spawn
        int powerUpIndex = Random.Range(0, 3);

        // Instantiate the corresponding power-up prefab
        PowerUp powerUpPrefab = null;
        switch (powerUpIndex)
        {
            case 0:
                powerUpPrefab = powerUpPrefabShield;
                Debug.Log("Spawning Shield Power-Up");
                break;
            case 1:
                powerUpPrefab = powerUpPrefabScoreBoost;
                Debug.Log("Spawning Score Boost Power-Up");
                break;
            case 2:
                powerUpPrefab = powerUpPrefabSpeedUp;
                Debug.Log("Spawning Speed Up Power-Up");
                break;
        }

        // Instantiate the power-up prefab
        PowerUp powerUp = Instantiate(powerUpPrefab, GetRandomSpawnPosition(), Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float halfScreenWidth = 26.0f;
        float halfScreenHeight = 14.0f;

        float randomX = Random.Range(-halfScreenWidth, halfScreenWidth);
        float randomY = Random.Range(-halfScreenHeight, halfScreenHeight);

        return new Vector3(randomX, randomY, 0.0f);
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

    public void ActivateScoreBoost()
    {
        StartCoroutine(DeactivateScoreBoost());
        isScoreBoostActive = true;
    }

    private IEnumerator DeactivateScoreBoost()
    {
        yield return new WaitForSeconds(powerUpDuration);
        isScoreBoostActive = false;
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SnakeBody")
        {
            if (!isShieldActive)
            {
                Debug.Log("Obstacle has been hit");
                GameOverPanel.SetActive(true);
                isGameOver = true;  
            }
        }
        else if (collision.tag == "Score")
        {
            Debug.Log("Score Obj has been hit");
            Destroy(collision.gameObject);
            ActivateScoreBoost();
        }

        else if (collision.tag == "Speed")
        {
            Debug.Log("Speed Obj has been hit");
            Destroy(collision.gameObject); ;
            ActivateSpeedUp();
        }
        else if (collision.tag == "Shield")
        {
            Debug.Log("Shield obj has been hit");
            Destroy(collision.gameObject);
            ActivateShield();
        }
        if (collision.tag == "Food")
        {
            if (!isScoreBoostActive)
            {
                score.IncreaseScore(100);
            }
            else
            {
                score.IncreaseScore(200);
            }
        }
            if(collision.tag == "ToxicFood")
            {
                score.DecreaseScore(50);
            }

        if (collision.tag == "Player")
        {
            GameOverPanel.SetActive(false);
            P1GameWon.SetActive(true);
        } 
        else if (collision.tag == "Player2")
        {
            GameOverPanel.SetActive(false);
            P2GameWon.SetActive(true);
        }
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    private void Pause()
    {
        Time.timeScale = 0f; // Set time scale to 0 to pause the game
        // Add any additional pause-related logic here
        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        Time.timeScale = 1f; // Set time scale back to 1 to resume the game
        // Add any additional resume-related logic here
        Debug.Log("Game Resumed");
        GamePausePanel.SetActive(false);
    }
}
