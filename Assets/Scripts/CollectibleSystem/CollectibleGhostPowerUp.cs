using System;
using Game;
using UnityEngine;
using UnityEngine.Events;

namespace CollectibleSystem
{
    public class CollectibleGhostPowerUp : Collectible
    {
        public static UnityAction<int> CollectedGhostPowerUp;
        [SerializeField] private int ghostDuration;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TrafficCar")) return;
            CollectedGhostPowerUp?.Invoke(ghostDuration);
            PoolManager.ReturnCollectible(this);
        }
    }
}