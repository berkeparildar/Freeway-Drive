using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool objectPool;
    public List<GameObject> pooledCars;
    public GameObject[] carPrefabs;
    public int carPoolCount;
    public GameObject carContainer;
    public List<GameObject> pooledPowerUps;
    public GameObject powerUpContainer;
    public int powerPoolCount;

    private void Awake()
    {
        objectPool = this;
        powerPoolCount = powerUpContainer.transform.childCount;
    }

    private void Start()
    {
        pooledCars = new List<GameObject>();
        for (var i = 0; i < carPoolCount; i++)
        {
            var tempCar = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length - 1)], carContainer.transform);
            tempCar.SetActive(false);
            pooledCars.Add(tempCar);
        }
        pooledPowerUps = new List<GameObject>();
        for (var i = 0; i < powerPoolCount; i++)
        {
            pooledPowerUps.Add(powerUpContainer.transform.GetChild(i).gameObject);
        }
    }
    
    public GameObject GetPooledCars()
    {
        var i = Random.Range(0, pooledCars.Count - 1);
        while (pooledCars[i].activeInHierarchy)
        {
            i = Random.Range(0, pooledCars.Count - 1);
        }
        return pooledCars[i];
    }
    
    public GameObject GetPooledPowerUp()
    {
        var i = Random.Range(0, powerPoolCount - 1);
        while (pooledPowerUps[i].activeInHierarchy)
        {
            i = Random.Range(0, powerPoolCount - 1);
        }
        return pooledPowerUps[i];
    }
}
