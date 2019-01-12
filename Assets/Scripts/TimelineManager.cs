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
        Instantiate(arrowPrefab);
    }

    public void SpawnElephant()
    {
        Instantiate(elephantPrefab);
    }

    public void SpawnHorse()
    {
        Instantiate(horsePrefab);
    }


}
