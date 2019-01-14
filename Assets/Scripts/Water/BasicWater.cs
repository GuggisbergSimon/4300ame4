using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWater : MonoBehaviour
{
	[SerializeField] private float width = 2.0f;
	[SerializeField] private float maxHeight = 2.0f;
	[SerializeField] private GameObject sprite = null;
	[SerializeField] private AudioClip raisingSound = null;
	[SerializeField] private AudioClip[] splashSound = null;
	private BoxCollider2D myCollider;
	private bool isRaising = false;
	private float finalHeight;
	private float finalTime;
	private AudioSource[] myAudioSources;
	
	private void Start()
	{
		myCollider = GetComponentInChildren<BoxCollider2D>();
		SetHeight(0.0f);
		SetWidth(width);
		myAudioSources = GetComponents<AudioSource>();
	}

	private void PlaySound(int index, AudioClip sound, bool loop)
	{
		myAudioSources[index].clip = sound;
		myAudioSources[index].loop = loop;
		myAudioSources[index].Play();
	}
	
	public IEnumerator RaiseHeight(float height, float time)
	{
		if (!isRaising)
		{
			PlaySound(0, raisingSound, true);
			if (height > maxHeight)
			{
				height = maxHeight;
			}

			float timer = 0.0f;
			while (timer < time)
			{
				yield return new WaitForEndOfFrame();
				timer += Time.deltaTime;
				SetHeight(timer * height / time);
			}

			SetHeight(height);
			myAudioSources[0].Stop();
		}
		else
		{
			yield return null;
		}
	}

	private void SetHeight(float height)
	{
		
		sprite.transform.localScale = Vector2.right * sprite.transform.localScale + Vector2.up * height;
		sprite.transform.localPosition = Vector2.right * sprite.transform.localPosition + Vector2.up * height / 2;
		myCollider.size = Vector2.right * myCollider.size + Vector2.up * height;
		myCollider.gameObject.transform.localPosition = Vector2.right * myCollider.transform.localPosition + Vector2.up * height / 2;
	}

	private void SetWidth(float width)
	{
		sprite.transform.localScale = Vector2.right * width + Vector2.up * sprite.transform.localScale;
		myCollider.size = Vector2.right * width + Vector2.up * myCollider.size;
	}

	private bool Contains(Bounds bounds)
	{
		Bounds container = myCollider.bounds;
		return container.min.x <= bounds.min.x && container.min.y <= bounds.min.y &&
		       container.max.x >= bounds.max.x && container.max.y >= bounds.max.y;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		PlaySound(1, splashSound[Random.Range(0,splashSound.Length)], false);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player") && Contains(other.bounds))
		{
			GameManager.Instance.Player.Die();
		}
	}
}