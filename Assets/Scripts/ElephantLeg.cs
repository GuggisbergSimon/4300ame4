﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantLeg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Meur putain");

            StartCoroutine(GameManager.Instance.Player.Die());
        }
    }
}
