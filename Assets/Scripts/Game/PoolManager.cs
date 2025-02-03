using System;
using Extensions;
using Scripts.Player;
using UnityEngine;

namespace Game
{
    public class PoolManager : MonoBehaviour
    {
        private static ObjectPool<TrafficCar> trafficCarPool;
        [SerializeField] private TrafficCar[] trafficCarPrefabs;
        [SerializeField] private Transform trafficCarContainer;
        
        private void Awake()
        {
            trafficCarPool = new ObjectPool<TrafficCar>(trafficCarPrefabs, 20, trafficCarContainer);
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
    }
}