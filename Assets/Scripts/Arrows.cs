using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Arrows : MonoBehaviour
{
	[SerializeField] private float timeBeforeShot = 2.0f;
	[SerializeField] private float speedAim = 2.0f;
	[SerializeField] private float speedShot = 10.0f;

	private Rigidbody2D myRigidBody;
	private bool isShot = false;
	private float timer = 0.0f;
	private Collider2D myCollider;

	private void Start()
	{
		myRigidBody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		Invoke("Shoot", timeBeforeShot);
	}

	private void OnDrawGizmos()
	{
		//TODO for debug purposes, to remove later
		float distance = 30.0f;
		RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 5.0f, Vector3.down, distance);
		Gizmos.DrawSphere(hit.point, 1.0f);
		Gizmos.DrawSphere(transform.position - Vector3.up * 5.0f, timeBeforeShot - timer);
	}

	private void Update()
	{
		if (!isShot)
		{
			timer += Time.deltaTime;
			transform.position += Mathf.Sign((GameManager.Instance.Player.transform.position - transform.position).x) *
								  Vector3.right * speedAim * Time.deltaTime;
		}
	}

	private void Shoot()
	{
		Vector2 differenceVector = GameManager.Instance.Player.transform.position - transform.position;
		isShot = true;
		myRigidBody.gravityScale = 1.0f;
		myRigidBody.velocity = differenceVector.normalized * speedShot;
		transform.up = -differenceVector;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameManager.Instance.Player.Die();
			Destroy(this.gameObject);
		}
		else if (other.CompareTag("Ground"))
		{
			myCollider.enabled = false;
			myRigidBody.bodyType = RigidbodyType2D.Static;
		}
		else if (other.CompareTag("Shelter"))
		{
			StartCoroutine(other.GetComponent<Shelter>().Burn());
			myCollider.enabled = false;
			myRigidBody.bodyType = RigidbodyType2D.Static;
			transform.SetParent(other.gameObject.transform);
		}
	}
}