using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : MonoBehaviour
{
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float shakeTime;

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
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public IEnumerator Shake()
    {
        CameraManager.Instance.Noise(shakeAmplitude, shakeFrequency);
        yield return new WaitForSeconds(shakeTime);
        CameraManager.Instance.Noise(0, 0);

    }

}
