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

	private SpriteRenderer spriteAimRenderer;
	private SpriteRenderer mySpriteRenderer;
	private Rigidbody2D myRigidBody;
	private bool isShot = false;
	private float timer = 0.0f;
	private Collider2D myCollider;

	private void Start()
	{
		myRigidBody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		spriteAimRenderer = aim.GetComponentInChildren<SpriteRenderer>();
		mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		Invoke("Shoot", timeBeforeShot);
	}

	private void OnDrawGizmos()
	{
		float distance = 30.0f;
		RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 5.0f, Vector3.down, distance);
		Gizmos.DrawSphere(transform.position - Vector3.up * 5.0f, timeBeforeShot - timer);
	}

	private void Update()
	{
		if (!isShot)
		{
			timer += Time.deltaTime;
			
			//handle the
			transform.position += Mathf.Sign((GameManager.Instance.Player.transform.position - transform.position).x) *
			                      Vector3.right * speedAim * Time.deltaTime;
			
			//handle the rotation when aiming
			Vector2 diff = transform.position - GameManager.Instance.Player.transform.position;
			Quaternion targetRot = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90);
			transform.rotation =
				Quaternion.RotateTowards(transform.rotation, targetRot, speedRotationAim * Time.deltaTime);
		}

		RaycastHit2D hit = Physics2D.Raycast(transform.position - transform.up.normalized * mySpriteRenderer.sprite.rect.height / 2,
			-transform.up);
		aim.transform.position = hit.point;
	}

	private void Shoot()
	{
		isShot = true;
		myRigidBody.gravityScale = 1.0f;
		myRigidBody.velocity = -transform.up * speedShot;
		//to use if we want to have arrows aiming correctly at player, which I found a bit unfair, unless we set a rotationspeed when aiming
		//Vector2 differenceVector = GameManager.Instance.Player.transform.position - transform.position;
		//myRigidBody.velocity = differenceVector.normalized * speedShot;
		//transform.up = -differenceVector;
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
		//myRigidBody.bodyType = RigidbodyType2D.Static;
		aim.SetActive(false);
		transform.SetParent(other.gameObject.transform);
	}
}