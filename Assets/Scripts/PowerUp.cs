using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Shield,
        ScoreBoost,
        SpeedUp
    }

    public PowerUpType powerUpType;

    public void ActivatePowerUp(Snake snake)
    {
        switch (powerUpType)
        {
            case PowerUpType.Shield:
                snake.ActivateShield();
                break;
            case PowerUpType.ScoreBoost:
                snake.ActivateScoreBoost();
                break;
            case PowerUpType.SpeedUp:
                snake.ActivateSpeedUp();
                break;
        }

        // Deactivate the power-up object
        gameObject.SetActive(false);
    }
}