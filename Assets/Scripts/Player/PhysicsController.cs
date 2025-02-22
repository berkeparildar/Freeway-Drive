using System;
using System.Collections;
using CollectibleSystem;
using Scripts.Player;
using UnityEngine;

namespace Player
{
    public class PhysicsController : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        
        private void Awake()
        {
            CollectibleGhostPowerUp.CollectedGhostPowerUp += OnCollectedGhostPowerUp;
        }

        private void OnDestroy()
        {
            CollectibleGhostPowerUp.CollectedGhostPowerUp -= OnCollectedGhostPowerUp;
        }

        private void OnCollectedGhostPowerUp(int time)
        {
            StartCoroutine(ColliderDisableRoutine());
            return;
            IEnumerator ColliderDisableRoutine()
            {
                boxCollider.enabled = false;
                yield return new WaitForSeconds(time);
                boxCollider.enabled = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TrafficCar"))
            {
                PlayerManager.PlayerCrashed?.Invoke();
            }
        }
    }
}