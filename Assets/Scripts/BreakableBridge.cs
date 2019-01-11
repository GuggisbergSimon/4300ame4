using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class BreakableBridge : MonoBehaviour
{
	[SerializeField] private Sprite[] spritesDestroyed = null;
	[SerializeField] private GameObject destroyedPartPrefab = null;
	private Collider2D myCollider;
	private SpriteRenderer mySpriteRenderer;
	private bool isBroken = false;

	private void Start()
	{
		myCollider = GetComponent<Collider2D>();
		mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

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
		myCollider.enabled = false;
		mySpriteRenderer.enabled = false;
		foreach (var sprite in spritesDestroyed)
		{
			Vector4 cropping = DataUtility.GetPadding(sprite);
			float widthCropped = sprite.rect.width - cropping.x - cropping.z;
			float heightCropped = sprite.rect.height - cropping.w - cropping.y;
			//todo reformulate those lines. they are pretty hideous
			Vector2 centerCropped = (cropping.x > cropping.z ? Vector2.left : Vector2.right) *
			                        (sprite.rect.width/2 -
			                         ((cropping.x > cropping.z ? cropping.x : cropping.z) + widthCropped / 2)) +
			                        (cropping.y > cropping.w ? Vector2.down : Vector2.up) *
			                        (sprite.rect.height/2 -
			                         ((cropping.y > cropping.w ? cropping.y : cropping.w) + heightCropped / 2));
			Vector2 initPos = (Vector2) transform.position + centerCropped / sprite.pixelsPerUnit;
			GameObject destroyedPart = Instantiate(destroyedPartPrefab, initPos, transform.rotation, transform);
			BoxCollider2D colliderDestroyer = destroyedPart.GetComponent<BoxCollider2D>();
			colliderDestroyer.size = Vector2.right * widthCropped / sprite.pixelsPerUnit +
			                         Vector2.up * heightCropped / sprite.pixelsPerUnit;
			SpriteRenderer spriteDestroyedRenderer = destroyedPart.GetComponentInChildren<SpriteRenderer>();
			spriteDestroyedRenderer.sprite = sprite;
			spriteDestroyedRenderer.gameObject.transform.position = transform.position;
			Debug.Log(spriteDestroyedRenderer.gameObject.transform.localPosition);
			//todo add velocity random to destroyedpart ?
		}
	}
}