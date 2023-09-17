using UnityEngine;

public class Road : MonoBehaviour
{
    private static readonly float[] SpawnPoints = new float[] {-2.5f, -5, -7.5f, -10};
    [SerializeField] private int _chanceToSpawn;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject carContainer;

    private void Start()
    {
        carContainer = GameObject.Find("CarContainer");
        _chanceToSpawn = Random.Range(0, 6);
        if (_chanceToSpawn is 2 or 4)
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
        var xPos = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        var yPos = GameManager.SpawnPointsIndex[car.tag];
        var spawnPosition = new Vector3(xPos, yPos, transform.position.z);
        var initCar = Instantiate(car, spawnPosition, Quaternion.Euler(0, -90, 0));
        initCar.transform.SetParent(carContainer.transform);
    }

    private void InstantiatePowerUp()
    {
        var powerUp = powerUps[Random.Range(0, powerUps.Length)];
        var xPos = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        var yPos = 8.22f;
        var spawnPosition = new Vector3(xPos, yPos, transform.position.z);
        Instantiate(powerUp, spawnPosition, Quaternion.identity);
    }
}
