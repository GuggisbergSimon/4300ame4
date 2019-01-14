using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : MonoBehaviour
{
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float shakeTime;
    [SerializeField] private AudioClip stepSound = null;
    [SerializeField] private AudioClip[] trumpetSound = null;
    private AudioSource[] myAudioSources;

    private Animator animator;

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
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void PlaySound(int index, AudioClip sound)
    {
        myAudioSources[index].clip = sound;
        myAudioSources[index].loop = false;
        myAudioSources[index].Play();
    }

    public void Trumpet()
    {
        PlaySound(1,trumpetSound[Random.Range(0,trumpetSound.Length)]);
    }

    public IEnumerator Shake()
    {
        PlaySound(0, stepSound);
        CameraManager.Instance.Noise(shakeAmplitude, shakeFrequency);
        yield return new WaitForSeconds(shakeTime);
        CameraManager.Instance.Noise(0, 0);

    }

}
