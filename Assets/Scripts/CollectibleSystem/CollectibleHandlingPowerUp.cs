using System;
using Game;
using UnityEngine;
using UnityEngine.Events;

namespace CollectibleSystem
{
    public class CollectibleHandlingPowerUp : Collectible
    {
        public static UnityAction<float> CollectedHandlingPowerUp;
        [SerializeField] private float handlingIncreaseAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TrafficCar")) return;
            CollectedHandlingPowerUp?.Invoke(handlingIncreaseAmount);
            PoolManager.ReturnCollectible(this);
        }
    }
}