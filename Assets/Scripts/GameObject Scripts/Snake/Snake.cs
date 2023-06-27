using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
	[SerializeField] private int snakeSize;
	[SerializeField] Transform snakeSegment;
	[SerializeField] private float moveSpeed;
	private Vector2 currentDirection = Vector2.zero;
	private Vector2 lastDirection;
	private List<Transform> segments = new List<Transform>();
	private TextMeshProUGUI scoreText;
	private TextMeshProUGUI powerText;
	private int score;
	private int currentPower;
	private float powerTimer = 5.0f;
	private Rigidbody2D rb;

	private PlayerInput playerInput;
	private bool isPaused = true;
	private bool isPowered = false;
	private int scoreMultiplier = 1;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		rb = GetComponent<Rigidbody2D>();
		rb.simulated = false;
	}

	private void Start()
	{
        SetSnake();
        powerText.enabled = false;
    }

	// new Input System assignments
	private void OnEnable()
	{
		playerInput.onActionTriggered += OnActionTriggered;
	}

	private void OnDisable()
	{
		playerInput.onActionTriggered -= OnActionTriggered;
	}

	public void OnActionTriggered(InputAction.CallbackContext context)
	{
		if (context.action.name == "Move")
		{
			if (isPaused)
			{
				Vector2 moveVector = context.ReadValue<Vector2>();
				if (moveVector.magnitude > 0.1f)
				{
					currentDirection = moveVector.normalized;
					isPaused = false;
					rb.simulated = true; // to remove collision with the snake segments
				}
			}
			else
			{
				Vector2 moveVector = context.ReadValue<Vector2>();
				if (moveVector.magnitude > 0.1f)
				{
					if (Mathf.Approximately(moveVector.x, -currentDirection.x) || Mathf.Approximately(moveVector.y, -currentDirection.y))
					{
						// Invalid move, ignore it
						return;
					}
					currentDirection = moveVector.normalized;
				}
			}
		}
	}

	public void Pause(InputAction.CallbackContext value)
	{
		isPaused = !isPaused;
		if (isPaused)
		{
			rb.simulated = false;
		}
		else
		{
			rb.simulated = true;
		}
	}

	private void Update()
	{
		scoreText.text = "Score:\n" + score.ToString();

        if (isPowered)
		{
			powerText.enabled = true;
			powerText.text = powerupTexts[currentPower] + " " + powerTimer.ToString("F1");
		}
		else
		{
			powerText.enabled = false;
		}
	}

	private void FixedUpdate()
	{
		if (isPaused)
		{
			return;
		}

		lastDirection = currentDirection;
		Vector2 movement = currentDirection * moveSpeed;

		for (int i = segments.Count - 1; i > 0; i--)
		{
			segments[i].position = segments[i - 1].position;
		}

		float x = Mathf.Round(transform.position.x) + movement.x;
		float y = Mathf.Round(transform.position.y) + movement.y;

		if (x < GameBounds.Left)
			x = GameBounds.Right;
		else if (x > GameBounds.Right)
			x = GameBounds.Left;

		if (y < GameBounds.Bottom)
			y = GameBounds.Top;
		else if (y > GameBounds.Top)
			y = GameBounds.Bottom;

		transform.position = new Vector2(x, y);
	}

	private void SetSnake()
	{
		for (int i = 1; i < segments.Count; i++)
		{
			Destroy(segments[i].gameObject);
		}

		segments.Clear();
		segments.Add(transform);

		for (int i = 0; i < snakeSize - 1; i++)
		{
			Grow();
		}

		score = 0;
	}

	public void Grow()
	{
		Transform snakePart = Instantiate(snakeSegment);
		snakePart.position = segments[segments.Count - 1].position;
		segments.Add(snakePart);
		
		score = score + (1 * scoreMultiplier); // multipler is always one unless the correct powerup is picked up
		AudioManager.Instance.PlayEffect(AudioManager.Effects.Grow);
	}

	public void Reduce()
	{
		Transform snakePart = segments[segments.Count - 1].transform;
		segments.RemoveAt(segments.Count - 1);
		Destroy(snakePart.gameObject);

		if (score > 0)
		{
			score -= 1;
		}
		else
		{
			score = 0;
		}

        AudioManager.Instance.PlayEffect(AudioManager.Effects.Reduce);
    }

	public void BurnSnake()
	{
		foreach (Transform segment in segments)
		{
			if (segment != null)
			{
				Destroy(segment.gameObject);
			}
		}

		segments.Clear();
		Destroy(gameObject);

		AudioManager.Instance.PlayEffect(AudioManager.Effects.Burn);
		SceneManager.LoadScene("GameOver");
	}

	public void SetTextObject(TextMeshProUGUI text1obj, TextMeshProUGUI text2obj)
	{
		scoreText = text1obj;
		powerText = text2obj;
	}

	public void PhasePower(int powerIndex)
	{
		isPowered = true;
		currentPower = powerIndex;
        AudioManager.Instance.PlayEffect(AudioManager.Effects.Phase);
        StartCoroutine(ActivatePhasePowerCoroutine());
	}

	public void SpeedPower(int powerIndex)
	{
		isPowered = true;
        currentPower = powerIndex;
        AudioManager.Instance.PlayEffect(AudioManager.Effects.Speed);
        StartCoroutine(ActivateSpeedPowerCoroutine());
	}

	public void MultiplyPower(int powerIndex)
	{
		isPowered = true;
        currentPower = powerIndex;
        AudioManager.Instance.PlayEffect(AudioManager.Effects.Double);
        StartCoroutine(ActivateMultiplyPowerCoroutine());
	}

    private IEnumerator RunDurationTimer(float duration)
    {
        float timer = duration;

        while (timer > 0f)
        {
            powerTimer = timer; // Update powerTimer value
            yield return new WaitForSeconds(0.1f); // Adjust the interval for updating the timer
            timer -= 0.1f; // Decrease the timer by the interval
        }

        powerTimer = 0f; // Reset the powerTimer value
        isPowered = false;
    }

    private IEnumerator ActivatePhasePowerCoroutine()
    {
        rb.simulated = false;
        float duration = 5.0f;

        yield return StartCoroutine(RunDurationTimer(duration));

        rb.simulated = true;
    }

    private IEnumerator ActivateSpeedPowerCoroutine()
    {
        float currentSpeed = moveSpeed;
        moveSpeed += 2.0f;
        float duration = 5.0f;

        yield return StartCoroutine(RunDurationTimer(duration));

        moveSpeed = currentSpeed;
    }

    private IEnumerator ActivateMultiplyPowerCoroutine()
    {
        scoreMultiplier = 2;
        float duration = 5.0f;

        yield return StartCoroutine(RunDurationTimer(duration));

        scoreMultiplier = 1;
    }

	// different UI prompt for each powerup
    private string[] powerupTexts = new string[]
	{
		"Snake is now faster for",
		"Snake will pass through objects for",
		"Every pizza is double points for",
	};
}
