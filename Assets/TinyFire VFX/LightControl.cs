using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
	private Light myLight;
	float nRand = 0;

	private void Start()
	{
		myLight = GetComponent<Light>();
	}

	void Update()
	{
		nRand = Random.Range(4f, 5f);
		myLight.intensity = nRand;
	}
}