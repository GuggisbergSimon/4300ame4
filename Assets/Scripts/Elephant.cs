using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : MonoBehaviour
{
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float shakeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Shake()
    {
        CameraManager.Instance.Noise(shakeAmplitude, shakeFrequency);
        yield return new WaitForSeconds(shakeTime);
        CameraManager.Instance.Noise(0, 0);

    }

}
