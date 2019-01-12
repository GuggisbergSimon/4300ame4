using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour
{
	[SerializeField] private float timeBeforeBurning=0.5f;
	[SerializeField] private float timeNoRebuild=4.0f;
	[SerializeField] private float timeRebuild=3.0f;
	private SpriteRenderer mySpriteRenderer;
	private Collider2D myCollider;
	private int initialNumberChildren;

	private void Start()
	{
		mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		myCollider = GetComponent<Collider2D>();
		initialNumberChildren = transform.childCount;
	}

	public IEnumerator Burn()
	{
		yield return new WaitForSeconds(timeBeforeBurning);
		myCollider.enabled = false;
		if (initialNumberChildren > 0)
		{
			for (int i = 0; i < initialNumberChildren; i++)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		for (int i = initialNumberChildren; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
		yield return new WaitForSeconds(timeNoRebuild);
		//todo make sprite blink while timetorebuild is used
		yield return new WaitForSeconds(timeRebuild);
		Build();
	}

	private void Build()
	{
		myCollider.enabled = true;
		mySpriteRenderer.enabled = true;
		if (initialNumberChildren > 0)
		{
			for (int i = 0; i < initialNumberChildren; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
	}
}
