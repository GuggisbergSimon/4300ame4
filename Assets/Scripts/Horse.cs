using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {

    }
    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(GameManager.Instance.Player.Die());
        }
    }
}
