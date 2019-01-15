using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Elephant : MonoBehaviour
{
	[SerializeField] private float shakeAmplitude;
	[SerializeField] private float shakeFrequency;
	[SerializeField] private float shakeTime;
	[SerializeField] private AudioClip stepSound = null;
	[SerializeField] private AudioClip[] trumpetSound = null;
	[SerializeField] private float pitchThresholdRandom = 0.1f;
	private AudioSource[] myAudioSources;

	private Animator animator;
	private List<float> originalPitches;
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
		originalPitches=new List<float>();
		foreach (var myAudioSource in myAudioSources)
		{
			originalPitches.Add(myAudioSource.pitch);
		}
		PlaySound(1, trumpetSound[Random.Range(0, trumpetSound.Length)], 0.0f);
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}

	private void PlaySound(int index, AudioClip sound, float pitch)
	{
		myAudioSources[index].pitch = originalPitches[index] + Random.Range(-pitch, pitch);
		myAudioSources[index].clip = sound;
		myAudioSources[index].loop = false;
		myAudioSources[index].Play();
	}

	public void Trumpet()
	{
		PlaySound(1, trumpetSound[Random.Range(0, trumpetSound.Length)], 0);
	}

	public IEnumerator Shake()
	{
		PlaySound(0, stepSound, pitchThresholdRandom);
		CameraManager.Instance.Noise(shakeAmplitude, shakeFrequency);
		yield return new WaitForSeconds(shakeTime);
		CameraManager.Instance.Noise(0, 0);
	}
}