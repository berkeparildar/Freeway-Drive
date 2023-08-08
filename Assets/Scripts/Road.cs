using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private readonly float[] _spawnPoints = new float[] {-2.5f, -5, -7.5f, -10};
    private int _chanceToSpawn;
    public GameObject[] cars;
    public GameObject[] powerUps;
    private GameObject _carContainer;
    void Start()
    {
        _carContainer = GameObject.Find("CarContainer");
        _chanceToSpawn = Random.Range(0, 6);
        if (_chanceToSpawn == 2)
        {
            InstantiateCar();
        }
        else if (_chanceToSpawn == 3)
        {
            InstantiatePowerUp();
        }
    }

    private void InstantiateCar()
    {
        var car = cars[Random.Range(0, cars.Length)];
        var xPos = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        var yPos = GameManager._spawnPointsIndex[car.tag];
        var spawnPosition = new Vector3(xPos, yPos, transform.position.z);
        var initCar = Instantiate(car, spawnPosition, Quaternion.Euler(0, -90, 0));
        initCar.transform.SetParent(_carContainer.transform);
    }

    private void InstantiatePowerUp()
    {
        var powerUp = powerUps[Random.Range(0, powerUps.Length)];
        var xPos = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        var yPos = 8.22f;
        var spawnPosition = new Vector3(xPos, yPos, transform.position.z);
        Instantiate(powerUp, spawnPosition, Quaternion.identity);
    }
}
