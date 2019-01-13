using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI deathsPlayerCounts = null;
	[SerializeField] private AudioClip deathNumberUpSound=null;
	private static UIManager instance;
	public static UIManager Instance => instance;
	private AudioSource myAudioSource;
	
	private void Awake()
	{
		instance = this;
		myAudioSource = GetComponent<AudioSource>();
	}
	
	public void UpdateUI()
	{
		deathsPlayerCounts.text = deathsPlayerCounts.text.Remove(deathsPlayerCounts.text.IndexOf(":") + 2);
		deathsPlayerCounts.text = deathsPlayerCounts.text.Insert(deathsPlayerCounts.text.Length,
			GameManager.Instance.DeathsPlayerCount.ToString());
		myAudioSource.clip = deathNumberUpSound;
		myAudioSource.loop = false;
		myAudioSource.Play();
	}
}