using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    private static CameraManager instance;
    public static CameraManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }


    private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
