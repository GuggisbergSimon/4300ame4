using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    [SerializeField] private AudioClip stepSound = null;
    [SerializeField] private AudioClip neighSound = null;
    private Animator animator;
    private AudioSource myAudioSource;
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
        myAudioSource = GetComponent<AudioSource>();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    
    private void PlaySound(AudioClip sound)
    {
        myAudioSource.clip = sound;
        myAudioSource.loop = false;
        myAudioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Player.Die();
        }
    }
}
