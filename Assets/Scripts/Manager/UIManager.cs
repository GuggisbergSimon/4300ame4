using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI deathsPlayerCounts = null;
	[SerializeField] private TextMeshProUGUI wavesCounts = null;
    [SerializeField] private AudioClip respawnSound = null;
    [SerializeField] private AudioClip enableSound = null;
    [SerializeField] private AudioClip disableSound = null;
    [SerializeField] private AudioClip startSound = null;
    [SerializeField] private AudioClip selectSound = null;
    [SerializeField] private AudioClip backSound = null;
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject panelEnd;
    [SerializeField] private TextMeshProUGUI scoreFinal;
    private static UIManager instance;
	public static UIManager Instance => instance;
	private AudioSource myAudioSource;

	public enum enumSound
	{
		respawnSound,
		enableSound,
		disableSound,
		startSound,
		selectSound,
		backSound
	}

	private Dictionary<enumSound, AudioClip> enumSoundToAudioClip = new Dictionary<enumSound, AudioClip>();
	
	
	private void Awake()
	{
		//todo find a way more efficient to write this
		enumSoundToAudioClip.Add(enumSound.respawnSound,respawnSound);
		enumSoundToAudioClip.Add(enumSound.enableSound,enableSound);
		enumSoundToAudioClip.Add(enumSound.disableSound,disableSound);
		enumSoundToAudioClip.Add(enumSound.startSound,startSound);
		enumSoundToAudioClip.Add(enumSound.selectSound,selectSound);
		enumSoundToAudioClip.Add(enumSound.backSound,backSound);
		instance = this;
		myAudioSource = GetComponent<AudioSource>();
	}
	
	public void PlaySound(enumSound sound)
	{
		myAudioSource.clip = enumSoundToAudioClip[sound];
		myAudioSource.loop = false;
		myAudioSource.Play();
	}
	
	public void UpdateUI()
	{
		deathsPlayerCounts.text = deathsPlayerCounts.text.Remove(deathsPlayerCounts.text.IndexOf(":") + 2);
		deathsPlayerCounts.text = deathsPlayerCounts.text.Insert(deathsPlayerCounts.text.Length,
		GameManager.Instance.DeathsPlayerCount.ToString());
		myAudioSource.clip = respawnSound;
		myAudioSource.loop = false;
		myAudioSource.Play();

	    wavesCounts.text = wavesCounts.text.Remove(wavesCounts.text.IndexOf(":") + 2);
	    wavesCounts.text = wavesCounts.text.Insert(wavesCounts.text.Length,
	    GameManager.Instance.WavesCount.ToString());
    }

    public void End()
    {
        panelHUD.SetActive(false);
        panelEnd.SetActive(true);
        scoreFinal.text = "Score : " + GameManager.Instance.DeathsPlayerCount.ToString();
    }
}