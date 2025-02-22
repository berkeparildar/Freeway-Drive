using System;
using CollectibleSystem;
using Extensions;
using TrafficSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class PoolManager : MonoBehaviour
    {
        private static ObjectPool<TrafficCar> trafficCarPool;
        [SerializeField] private TrafficCar[] trafficCarPrefabs;
        [SerializeField] private Transform trafficCarContainer;
        
        private static ObjectPool<Collectible> moneyPool;
        private static ObjectPool<Collectible> ghostPowerUpPool;
        private static ObjectPool<Collectible> handlingPowerUpPool;
        [SerializeField] private Collectible[] moneyPrefab;
        [SerializeField] private Collectible[] ghostPowerUpPrefab;
        [SerializeField] private Collectible[] handlingPowerUpPrefab;
        
        [SerializeField] private Transform powerUpContainer;
        
        private void Awake()
        {
            trafficCarPool = new ObjectPool<TrafficCar>(trafficCarPrefabs, 20, trafficCarContainer);
            moneyPool = new ObjectPool<Collectible>(moneyPrefab, 10, powerUpContainer);
            ghostPowerUpPool = new ObjectPool<Collectible>(ghostPowerUpPrefab, 10, powerUpContainer);
            handlingPowerUpPool = new ObjectPool<Collectible>(handlingPowerUpPrefab, 10, powerUpContainer);
        }
        
        public static TrafficCar GetTrafficCar()
        {
            var car = trafficCarPool.GetFromPool();
            return car;
        }

        public static void ReturnTrafficCar(TrafficCar trafficCar)
        {
            trafficCarPool.ReturnToPool(trafficCar);
        }

        public static Collectible GetCollectible()
        {
            var randomInt = Random.Range(0, 3);
            return randomInt switch
            {
                0 => moneyPool.GetFromPool(),
                1 => ghostPowerUpPool.GetFromPool(),
                2 => handlingPowerUpPool.GetFromPool(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static void ReturnCollectible(Collectible collectible)
        {
            switch (collectible)
            {
                case CollectibleMoney:
                    moneyPool.ReturnToPool(collectible);
                    break;
                case CollectibleGhostPowerUp:
                    ghostPowerUpPool.ReturnToPool(collectible);
                    break;
                case CollectibleHandlingPowerUp:
                    handlingPowerUpPool.ReturnToPool(collectible);
                    break;
            }
        }
    }
}