using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI deathsPlayerCounts = null;
	private static UIManager instance;
	public static UIManager Instance => instance;
	
	private void Awake()
	{
		instance = this;
	}
	
	public void UpdateUI()
	{
		deathsPlayerCounts.text = deathsPlayerCounts.text.Remove(deathsPlayerCounts.text.IndexOf(":") + 2);
		deathsPlayerCounts.text = deathsPlayerCounts.text.Insert(deathsPlayerCounts.text.Length,
			GameManager.Instance.DeathsPlayerCount.ToString());
	}
}