using UnityEngine;

public class Road : MonoBehaviour
{
    private static readonly float[] SpawnPoints = new float[] {-2.5f, -5, -7.5f, -10};
    [SerializeField] private int chanceToSpawn;
    
    public void Initialize()
    {
        chanceToSpawn = Random.Range(0, 6);
        if (chanceToSpawn is 2 or 4)
        {
            InstantiateCar();
        }
        else if (chanceToSpawn == 3)
        {
            InstantiatePowerUp();
        }
    }

    private void InstantiateCar()
    {
        var car = ObjectPool.objectPool.GetPooledCars(); 
        if (car != null) {
            var xPos = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            var yPos = GameManager.SpawnPointsIndex[car.tag];
            var spawnPosition = new Vector3(xPos, yPos, transform.position.z);
            car.transform.position = spawnPosition;
            car.transform.rotation = Quaternion.Euler(0, -90, 0);
            car.SetActive(true);
        }
    }

    private void InstantiatePowerUp()
    {
        var powerUp = ObjectPool.objectPool.GetPooledPowerUp(); 
        if (powerUp != null) {
            var xPos = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            var yPos = 8.22f;
            var spawnPosition = new Vector3(xPos, yPos, transform.position.z);
            powerUp.transform.position = spawnPosition;
            powerUp.SetActive(true);
        }
    }
}
