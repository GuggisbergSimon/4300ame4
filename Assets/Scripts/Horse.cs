using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
	[SerializeField] private AudioClip gallopSound = null;
	[SerializeField] private AudioClip[] neighSound = null;
	private Animator animator;
	private AudioSource[] myAudioSources;
	private float speed;

	public float Speed
	{
		get => speed;
		set => speed = value;
	}

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		animator.SetFloat("Speed", speed);
		myAudioSources = GetComponents<AudioSource>();
		PlaySound(0, gallopSound, true);
		PlaySound(1, neighSound[Random.Range(0, neighSound.Length)], false);
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}

	private void PlaySound(int index, AudioClip sound, bool loop)
	{
		myAudioSources[index].clip = sound;
		myAudioSources[index].loop = loop;
		myAudioSources[index].Play();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameManager.Instance.Player.Die();
		}
	}
}