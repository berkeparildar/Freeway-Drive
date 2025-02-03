using Game;
using UnityEngine;

public class Road : MonoBehaviour
{
    private static readonly float[] SpawnPoints = new float[] { -2.5f, -5, -7.5f, -10 };
    [SerializeField] private int chanceToSpawn;
    [SerializeField] private TrafficCar car;
    [SerializeField] private PowerUp powerUp;

    public void Initialize()
    {
        chanceToSpawn = Random.Range(0, 6);
        switch (chanceToSpawn)
        {
            case < 2:
                InstantiateCar();
                break;
            case 4:
                InstantiatePowerUp();
                break;
        }
    }

    private void InstantiateCar()
    {
        car = PoolManager.GetTrafficCar();
        var xPos = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        var spawnPosition = new Vector3(xPos, 8, transform.position.z);
        car.transform.position = spawnPosition;
    }

    private void InstantiatePowerUp()
    {
        var powerUp = ObjectPoolOld.objectPoolOld.GetPooledPowerUp();
        if (powerUp != null)
        {
            var xPos = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            var yPos = 8.22f;
            var spawnPosition = new Vector3(xPos, yPos, transform.position.z);
            powerUp.transform.position = spawnPosition;
            powerUp.SetActive(true);
        }
    }

    public void ClearRoad()
    {
        if (car is not null && car.gameObject.activeSelf)
        {
            PoolManager.ReturnTrafficCar(car);
        }

        if (powerUp is not null && powerUp.gameObject.activeSelf)
        {
            
        }
    }
}