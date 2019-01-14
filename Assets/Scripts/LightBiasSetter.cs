using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBiasSetter : MonoBehaviour
{
	[SerializeField] private float bias = -0.05f;
	[SerializeField] private bool enableUpdate = false;
	private Light myLight;

	private void OnGUI()
	{
		myLight = GetComponent<Light>();
		myLight.shadowBias = bias;
	}

	private void Start()
	{
		myLight = GetComponent<Light>();
	}

	private void Update()
	{
		if (enableUpdate)
		{
			myLight.shadowBias = bias;
		}
	}
}