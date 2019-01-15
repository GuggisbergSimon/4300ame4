using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private GameObject arrowSlidingPrefab;
    [SerializeField] private GameObject arrowRotatePrefab;
    [SerializeField] private GameObject elephantPrefab;
    [SerializeField] private GameObject horsePrefab;
    [SerializeField] private BasicWater waterGameObject;

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

    public void SpawnArrowSliding()
    {
        GameObject newGameObject = Instantiate(arrowSlidingPrefab);
    }

    public void SpawnArrowRotate()
    {
        GameObject newGameObject = Instantiate(arrowRotatePrefab);
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

    public void SpawnWater(float speed)
    {
        waterGameObject.RaiseHeightFunction(true, speed);
    }

    public void DespawnWater(float speed)
    {
        waterGameObject.RaiseHeightFunction(false, speed);
    }

    public void WavesCount(int wavesCounts)
    {
        GameManager.Instance.WavesCount = wavesCounts;
        UIManager.Instance.UpdateUI();
        UIManager.Instance.PlaySoundByEnum(0,UIManager.enumSound.newWaveSound);
    }

    public void End()
    {
        GameManager.Instance.End();
    }


}
