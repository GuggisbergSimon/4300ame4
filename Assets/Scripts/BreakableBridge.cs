using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBridge : MonoBehaviour
{
	[SerializeField] private Sprite[] spritesDestroyed=null;
	[SerializeField] private GameObject destroyedPartPrefab=null;
	private Collider2D myCollider;
	private bool isBroken = false;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!isBroken && !other.gameObject.CompareTag("Elephant"))
		{
			Break();
		}
	}

	private void Break()
	{
		isBroken = true;
		Destroy(this.gameObject);
		foreach (var sprite in spritesDestroyed)
		{
			GameObject destroyedPart = Instantiate(destroyedPartPrefab);
			destroyedPart.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
			//destroyedPart.GetComponent<Rigidbody2D>().velocity = Random;
			//todo change collider of destroyedPart to its correct form and correct place for destroyedPart
		}
	}
}