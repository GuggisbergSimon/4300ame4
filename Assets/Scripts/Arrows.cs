using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Arrows : MonoBehaviour
{
	[SerializeField] private float timeBeforeShot = 2.0f;
	[SerializeField] private float speedShot = 10.0f;

	private Rigidbody2D myRigidBody;
	private bool isShot = false;
	private float timer = 0.0f;
	private Collider2D mycollider;
	
	private void Start()
	{
		myRigidBody = GetComponent<Rigidbody2D>();
		mycollider = GetComponent<Collider2D>();
		Invoke("Shoot",timeBeforeShot);	
	}

	private void OnDrawGizmos()
	{
		//TODO for debug purposes, to remove later
		float distance = 30.0f;
		RaycastHit2D hit = Physics2D.Raycast(transform.position-Vector3.up*5.0f, Vector3.down, distance);
		Gizmos.DrawSphere(hit.point,1.0f);
		Gizmos.DrawSphere(transform.position-Vector3.up*5.0f,timeBeforeShot-timer);
	}

	private void Update()
	{
		if (!isShot)
		{
			timer += Time.deltaTime;
			//todo limit speed of arrows ?
			transform.position = Vector2.up*transform.position + Vector2.right * GameManager.Instance.Player.transform.position;
		}
	}

	private void Shoot()
	{
		isShot = true;
		myRigidBody.gravityScale = 1.0f;
		myRigidBody.velocity =
			(GameManager.Instance.Player.transform.position - transform.position).normalized * speedShot;
	}
}
