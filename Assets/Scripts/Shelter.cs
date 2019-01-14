using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour
{
	[SerializeField] private float timeBeforeBurning = 0.5f;
	[SerializeField] private float timeBurning = 1.0f;
	[SerializeField] private float timeBetweenNewFlame = 0.1f;
	[SerializeField] private float timeNoRebuild = 4.0f;
	[SerializeField] private float timeRebuild = 3.0f;
	[SerializeField] private GameObject firePrefab = null;
	[SerializeField] private int minNumberFire = 1;
	[SerializeField] private int maxNumberFire = 5;
	[SerializeField] private float yFireOffset = -0.4f;
	[SerializeField] private Color RebuildingColor = Color.gray;
	private SpriteRenderer mySpriteRenderer;
	private Collider2D myCollider;
	private int initialNumberChildren;

	private void Start()
	{
		mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		myCollider = GetComponent<Collider2D>();
		initialNumberChildren = transform.childCount;
	}

	public void Burn()
	{
		StartCoroutine(Burning());
	}

	private IEnumerator Burning()
	{
		yield return new WaitForSeconds(timeBeforeBurning);
		int randomFire = Random.Range(minNumberFire, maxNumberFire);
		for (int i = 0; i < randomFire; ++i)
		{
			Bounds myBounds = myCollider.bounds;
			Instantiate(firePrefab,
				new Vector2(Random.Range(myBounds.min.x, myBounds.max.x), Random.Range(myBounds.min.y, myBounds.max.y)+yFireOffset),
				transform.rotation, transform);
			yield return new WaitForSeconds(timeBetweenNewFlame);
		}

		yield return new WaitForSeconds(timeBurning);
		myCollider.enabled = false;

		for (int i = 0; i < initialNumberChildren; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}

		for (int i = initialNumberChildren; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}

		yield return new WaitForSeconds(timeNoRebuild);
		mySpriteRenderer.gameObject.SetActive(true);
		mySpriteRenderer.color = RebuildingColor;
		yield return new WaitForSeconds(timeRebuild);
		Build();
	}

	private void Build()
	{
		myCollider.enabled = true;
		mySpriteRenderer.enabled = true;
		mySpriteRenderer.color = Color.white;
		for (int i = 0; i < initialNumberChildren; i++)
		{
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}
}