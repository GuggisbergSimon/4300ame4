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
	[SerializeField] private int layerPlayer = 0;
	[SerializeField] private int layerPlayerDead = 0;
	[SerializeField] private Color deathColor = Color.red;
	[SerializeField] private Color inactiveColor = Color.red;
	[SerializeField] private Color invincibilityColor = Color.gray;
	[SerializeField] private AudioClip[] deathSounds = null;
	[SerializeField] private AudioClip jumpSound = null;

	[SerializeField] private AudioClip[] landSounds = null;

	//[SerializeField] private AudioClip walkSound = null;
	private bool hasPressedJump;
	private bool isAirborne;
	private Rigidbody2D myRigidbody2D;
	private SpriteRenderer mySpriteRenderer;
	private Animator myAnimator;
	private AudioSource myAudioSource;
	private float horizontalInput;
	private float verticalInput;
	private Vector2 respawnPosition;
	private int initialNumberChildren = 1;

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

	private PlayerState myState = PlayerState.Dying;

	private PlayerState MyState
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
		mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		myAudioSource = GetComponent<AudioSource>();
		myAnimator = GetComponentInChildren<Animator>();
		respawnPosition = transform.position;
		initialNumberChildren = transform.childCount;
		SetActive(false);
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
					PlaySound(jumpSound);
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
				myAnimator.SetFloat("Speed", 0);
				break;
			}

			default:
			{
				//updates horizontal input
				horizontalInput = Input.GetAxis("Horizontal");
				myAnimator.SetFloat("Speed", Mathf.Abs(myRigidbody2D.velocity.x));
				//flips the animator gameobject depending on direction
				if (horizontalInput > 0)
				{
					mySpriteRenderer.flipX = true;
				}
				else if (horizontalInput < 0)
				{
					mySpriteRenderer.flipX = false;
				}

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

				if (myState == PlayerState.Invincibility)
				{
					for (int i = initialNumberChildren; i < transform.childCount; i++)
					{
						Destroy(transform.GetChild(i).gameObject);
					}
				}


				break;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Shelter"))
		{
			PlaySound(landSounds[Random.Range(0, landSounds.Length)]);
			isAirborne = false;
		}
	}

	public void SetActive(bool value)
	{
		if (value)
		{
			myState = PlayerState.Idle;
			myRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		else
		{
			myState = PlayerState.Dying;
			myRigidbody2D.bodyType = RigidbodyType2D.Static;
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	private void Setup(int initialNumberChildren)
	{
		SetActive(true);
		myState = PlayerState.Invincibility;
		mySpriteRenderer.color = invincibilityColor;
		myAnimator.SetBool("Death", false);
		GameManager.Instance.Player = this;
		this.tag = "Player";
		this.gameObject.layer = layerPlayer;
		this.initialNumberChildren = initialNumberChildren;
		for (int i = initialNumberChildren; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	public void Die()
	{
		StartCoroutine(Dying());
	}

	private void PlaySound(AudioClip sound)
	{
		myAudioSource.clip = sound;
		myAudioSource.loop = false;
		myAudioSource.Play();
	}

	public IEnumerator Dying()
	{
		if (myState != PlayerState.Dying && myState != PlayerState.Invincibility)
		{
			PlaySound(deathSounds[Random.Range(0, deathSounds.Length)]);
			myState = PlayerState.Dying;
			mySpriteRenderer.color = deathColor;
			myAnimator.SetBool("Death", true);
			yield return new WaitForSeconds(timeBeforeRespawn);

			GameManager.Instance.DeathsPlayerCount++;
			UIManager.Instance.UpdateUI();
			UIManager.Instance.PlaySound(0, UIManager.enumSound.respawnSound);
			this.tag = "Untagged";
			this.gameObject.layer = layerPlayerDead;
			GameObject newObject = Instantiate(playerPrefab, respawnPosition, transform.rotation, transform.parent);
			PlayerController newPlayer = newObject.GetComponent<PlayerController>();
			newPlayer.Setup(initialNumberChildren);
			mySpriteRenderer.color = inactiveColor;
			yield return new WaitForSeconds(timeInvincibility);
			newPlayer.GetComponentInChildren<SpriteRenderer>().color = Color.white;
			newPlayer.myState = PlayerState.Idle;
		}
	}
}