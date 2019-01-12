using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject elephantPrefab;
    [SerializeField] private GameObject horsePrefab;

    [SerializeField] private GameObject spawnerArrow;
    [SerializeField] private GameObject spawnerElephant;
    [SerializeField] private GameObject spawnerHorse;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnArrow()
    {
        GameObject newGameObject = Instantiate(arrowPrefab);
    }

    public void SpawnElephant(float speed)
    {
        GameObject newGameObject = Instantiate(elephantPrefab);
        newGameObject.GetComponent<Elephant>().Speed = speed;
    }

    public void SpawnHorse(float speed)
    {
        GameObject newGameObject = Instantiate(horsePrefab);
        newGameObject.GetComponent<Horse>().Speed = speed;
    }


}
