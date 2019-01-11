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
	[SerializeField] private float timeBeforeRespawn = 1.0f;
	[SerializeField] private float timeInvincibility = 1.5f;
	[SerializeField] private GameObject playerPrefab = null;
	private bool hasPressedJump;
	private bool isAirborne;
	private Rigidbody2D myRigidbody2D;
	private Collider2D myCollider;
	private float horizontalInput;
	private float verticalInput;
	private Vector2 respawnPosition;
	private int initialNumberChildren=1;

	public enum PlayerState
	{
		Idle,
		ClimbingLadder,
		Walking,
		Jumping,
		Falling,
		Dying,
		Invincibility
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

	private void Awake()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		respawnPosition = transform.position;
		initialNumberChildren = transform.childCount;
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

			case PlayerState.Dying:
			{
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

			case PlayerState.Dying:
			{
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
		if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Shelter"))
		{
			isAirborne = false;
		}
	}

	public void Setup(int initialNumberChildren)
	{
		myRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
		myCollider.enabled = true;
		myState = PlayerState.Invincibility;
		GameManager.Instance.Player = this;
		this.initialNumberChildren = initialNumberChildren;
		for (int i = initialNumberChildren; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	public IEnumerator Die()
	{
		if (myState != PlayerState.Dying && myState != PlayerState.Invincibility)
		{
			//todo add way to see ingame that player is ded
			myState = PlayerState.Dying;
			myRigidbody2D.velocity = Vector2.zero;
			myRigidbody2D.bodyType = RigidbodyType2D.Static;
			myCollider.enabled = false;
			yield return new WaitForSeconds(timeBeforeRespawn);

			//todo make sprite blink while timeInvincibility is used (in update or fixedupdate probably would be best)
			this.tag = "Untagged";
			GameObject newObject = Instantiate(playerPrefab, respawnPosition, transform.rotation, transform.parent);
			PlayerController newPlayer = newObject.GetComponent<PlayerController>();
			newPlayer.Setup(initialNumberChildren);
			yield return new WaitForSeconds(timeInvincibility);
			newPlayer.myState = PlayerState.Idle;
		}
	}
}