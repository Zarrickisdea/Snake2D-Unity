using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType
    {
        Speed,
        Phase,
        Multiply
    }

    public PowerupType powerupType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Snake snake = collision.GetComponent<Snake>();
        int powerupIndex;
        if (snake != null)
        {
            switch (powerupType)
            {
                case PowerupType.Speed:
                    powerupIndex = 0;
                    snake.SpeedPower(powerupIndex);
                    break;
                case PowerupType.Phase:
                    powerupIndex = 1;
                    snake.PhasePower(powerupIndex);
                    break;
                case PowerupType.Multiply:
                    powerupIndex = 2;
                    snake.MultiplyPower(powerupIndex);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
