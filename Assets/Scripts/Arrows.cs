using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour
{
	[SerializeField] private float timeBeforeShot = 2.0f;
	[SerializeField] private float speedAim = 2.0f;
	[SerializeField] private float speedRotationAim = 0.0f;
	[SerializeField] private float speedShot = 10.0f;
	[SerializeField] private GameObject aim = null;
	[SerializeField] private GameObject fire = null;
	[SerializeField] private AudioClip releaseSound = null;
	[SerializeField] private AudioClip aimingSound = null;
	[SerializeField] private AudioClip[] shootingSounds = null;
	private SpriteRenderer mySpriteRenderer;
	private Rigidbody2D myRigidBody;
	private bool isShot = false;
	private Collider2D myCollider;
	private AudioSource myAudioSource;

	private void Start()
	{
		myRigidBody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		myAudioSource = GetComponent<AudioSource>();
		Invoke("Shoot", timeBeforeShot);
		PlaySound(releaseSound);
	}

	private void Update()
	{
		if (!isShot)
		{
			transform.position += Mathf.Sign((GameManager.Instance.Player.transform.position - transform.position).x) *
			                      Vector3.right * speedAim * Time.deltaTime;
			//handle the rotation when aiming
			Vector2 diff = transform.position - GameManager.Instance.Player.transform.position;
			Quaternion targetRot = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90);
			transform.rotation =
				Quaternion.RotateTowards(transform.rotation, targetRot, speedRotationAim * Time.deltaTime);
		}

		RaycastHit2D hit = Physics2D.Raycast(
			transform.position - transform.up.normalized * mySpriteRenderer.transform.localScale.x / 2,
			-transform.up);
		aim.transform.position = hit.point;
		fire.transform.rotation = Quaternion.Euler(Vector3.zero);
	}

	private void PlaySound(AudioClip sound)
	{
		myAudioSource.clip = sound;
		myAudioSource.loop = false;
		myAudioSource.Play();
	}
	
	private void Shoot()
	{
		PlaySound(shootingSounds[Random.Range(0,shootingSounds.Length)]);
		isShot = true;
		myRigidBody.gravityScale = 1.0f;
		myRigidBody.velocity = -transform.up * speedShot;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameManager.Instance.Player.Die();
			CollisionSolidObject(other);
		}
		else if (other.CompareTag("Arrow"))
		{
			//do nothing, as expected
		}
		else if (other.CompareTag("Shelter"))
		{
			CollisionSolidObject(other);
			other.GetComponent<Shelter>().Burn();
		}
		else
		{
			CollisionSolidObject(other);
		}
	}

	private void CollisionSolidObject(Collider2D other)
	{
		myCollider.enabled = false;
		Destroy(myRigidBody);
		aim.SetActive(false);
		transform.SetParent(other.gameObject.transform);
	}
}