using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWater : MonoBehaviour
{
	[SerializeField] private float width = 2.0f;
	[SerializeField] private float maxHeight = 2.0f;
	private Collider2D myCollider;

	private void Start()
	{
		myCollider = GetComponent<Collider2D>();
	}

	public void RaiseHeight(float height, float time)
	{
	}

	private bool Contains(Bounds bounds)
	{
		Bounds container = myCollider.bounds;
		return container.min.x <= bounds.min.x && container.min.y <= bounds.min.y &&
		       container.max.x >= bounds.max.x && container.max.y >= bounds.max.y;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player") && Contains(other.bounds))
		{
			StartCoroutine(GameManager.Instance.Player.Die());
		}
	}
}