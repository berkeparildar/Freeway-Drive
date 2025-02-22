using Game;
using UnityEngine;

namespace TrafficSystem
{
    public class Road : MonoBehaviour
    {
        private static readonly float[] SpawnPoints = { -10f, -7.5f, -5f, -2.5f };
        [SerializeField] private int chanceToSpawn;

        public void Initialize()
        {
            chanceToSpawn = Random.Range(0, 6);
            switch (chanceToSpawn)
            {
                case < 2:
                    InstantiateCar();
                    break;
                case > 3:
                    InstantiateCollectible();
                    break;
            }
        }

        private void InstantiateCar()
        {
            var car = PoolManager.GetTrafficCar();
            var laneIndex = Random.Range(0, SpawnPoints.Length);
            var xPos = SpawnPoints[laneIndex];
            var spawnPosition = new Vector3(xPos, 8, transform.position.z);
            car.transform.position = spawnPosition;
            car.SetLane(laneIndex);
        }

        private void InstantiateCollectible()
        {
            var collectible = PoolManager.GetCollectible();
            var xPos = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            var spawnPosition = new Vector3(xPos, 8, transform.position.z);
            collectible.transform.position = spawnPosition;
        }
    }
}