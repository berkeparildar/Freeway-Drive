using Game;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CollectibleSystem
{
    public class CollectibleMoney : Collectible
    {
        public static UnityAction<int> CollectedMoney;
        [SerializeField] private int maxMoneyAmount;
        [SerializeField] private int minMoneyAmount;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TrafficCar")) return;
            CollectedMoney?.Invoke(Random.Range(minMoneyAmount, maxMoneyAmount));
            PoolManager.ReturnCollectible(this);
        }
    }
}