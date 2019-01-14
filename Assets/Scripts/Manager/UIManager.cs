using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI deathsPlayerCounts = null;
	[SerializeField] private TextMeshProUGUI wavesCounts = null;
	[SerializeField] private AudioClip respawnSound = null;
	[SerializeField] private AudioClip newWaveSound = null;
	[SerializeField] private AudioClip startSound = null;
	[SerializeField] private AudioClip selectSound = null;
	private static UIManager instance;
	public static UIManager Instance => instance;
	private AudioSource myAudioSource;

	public enum enumSound
	{
		respawnSound,
		newWaveSound,
		startSound,
		selectSound,
	}

	private Dictionary<enumSound, AudioClip> enumSoundToAudioClip = new Dictionary<enumSound, AudioClip>();


	private void Awake()
	{
		//todo find a way more efficient to write this
		enumSoundToAudioClip.Add(enumSound.respawnSound, respawnSound);
		enumSoundToAudioClip.Add(enumSound.newWaveSound, newWaveSound);
		enumSoundToAudioClip.Add(enumSound.startSound, startSound);
		enumSoundToAudioClip.Add(enumSound.selectSound, selectSound);
		instance = this;
		myAudioSource = GetComponent<AudioSource>();
	}

	public void PlaySound(enumSound sound)
	{
		myAudioSource.clip = enumSoundToAudioClip[sound];
		myAudioSource.loop = false;
		myAudioSource.Play();
	}

	public void PlaySound(string sound)
	{
		Enum.TryParse(sound, out enumSound mySound);
		PlaySound(mySound);
	}

	public void ResetSelectedPosition(GameObject button)
	{
		EventSystem.current.SetSelectedGameObject(button);
	}

	public void UpdateUI()
	{
		deathsPlayerCounts.text = deathsPlayerCounts.text.Remove(deathsPlayerCounts.text.IndexOf(":") + 2);
		deathsPlayerCounts.text = deathsPlayerCounts.text.Insert(deathsPlayerCounts.text.Length,
			GameManager.Instance.DeathsPlayerCount.ToString());

		wavesCounts.text = wavesCounts.text.Remove(wavesCounts.text.IndexOf(":") + 2);
		wavesCounts.text = wavesCounts.text.Insert(wavesCounts.text.Length,
			GameManager.Instance.WavesCount.ToString());
	}
}