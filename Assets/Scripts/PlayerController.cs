using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float fallMultiplier = 2.5f;
	[SerializeField] private float lowJumpMultiplier = 2.0f;
	[SerializeField, Range(1, 10)] private float jumpSpeed = 5.0f;
	[SerializeField, Range(1, 10)] private float speed = 5.0f;
	[SerializeField, Range(1, 10)] private float climbingSpeed = 2.0f;
	private bool hasPressedJump;
	private bool isAirborne;
	private Rigidbody2D myRigidbody2D;
	private float horizontalInput;
	private float verticalInput;

	public enum PlayerState
	{
		Idle,
		ClimbingLadder,
		Walking,
		Jumping,
		Falling
	}

	private PlayerState myState;

	public PlayerState MyState
	{
		get => myState;
		set
		{
			myState = value;
			switch (myState)
			{
				case PlayerState.ClimbingLadder:
				{
					myRigidbody2D.gravityScale = 0.0f;
					break;
				}

				default:
				{
					myRigidbody2D.gravityScale = 1.0f;
					break;
				}
			}
		}
	}

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		switch (MyState)
		{
			case PlayerState.ClimbingLadder:
			{
				myRigidbody2D.velocity = Vector2.up * verticalInput * climbingSpeed;
				break;
			}

			default:
			{
				//launch the jump
				if (hasPressedJump)
				{
					myRigidbody2D.velocity = Vector2.up * jumpSpeed;
					hasPressedJump = false;
				}

				//code from "better jumping with 4 lines of code"
				if (myRigidbody2D.velocity.y < 0)
				{
					myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
				}
				else if (myRigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
				{
					myRigidbody2D.velocity +=
						Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
				}

				//adjust horizontal velocity
				myRigidbody2D.velocity = Vector2.right * speed * horizontalInput + myRigidbody2D.velocity * Vector2.up;
				break;
			}
		}
	}

	private void Update()
	{
		switch (MyState)
		{
			case PlayerState.ClimbingLadder:
			{
				//updates horizontal and vertical input
				horizontalInput = Input.GetAxis("Horizontal");
				verticalInput = Input.GetAxis("Vertical");
				if (horizontalInput.CompareTo(0) != 0)
				{
					myState = PlayerState.Idle;
				}

				break;
			}

			default:
			{
				//updates horizontal input
				horizontalInput = Input.GetAxis("Horizontal");
				//flips the animator gameobject depending on direction


				//code for checking jump input
				if (Input.GetButtonDown("Jump") && !isAirborne)
				{
					hasPressedJump = true;
					isAirborne = true;
				}
				else if (Input.GetButtonUp("Jump"))
				{
					hasPressedJump = false;
				}

				break;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		//TODO remove the ability to wall jump
		isAirborne = false;
	}

	public void Die()
	{
		
	}
}