using System;
using CollectibleSystem;
using UnityEngine;

namespace Scripts.Player
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int Crash = Animator.StringToHash("crash");
        private static readonly int Left = Animator.StringToHash("left");
        private static readonly int Right = Animator.StringToHash("right");
        private static readonly int Gear = Animator.StringToHash("gear");

        private void Awake()
        {
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
            CollectibleHandlingPowerUp.CollectedHandlingPowerUp += OnCollectedHandlingPowerUp;
        }

        private void OnDestroy()
        {
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
            CollectibleHandlingPowerUp.CollectedHandlingPowerUp -= OnCollectedHandlingPowerUp;
        }

        public void TurnLeft()
        {
            animator.SetTrigger(Left);    
        }

        public void TurnRight()
        {
            animator.SetTrigger(Right);
        }
        
        private void OnCollectedHandlingPowerUp(float speedMultiplier)
        {
            Debug.Log("Collected handling amount of  " + speedMultiplier);
            animator.speed /= speedMultiplier;
            Debug.Log(animator.speed);
        }

        public void IncreaseGear()
        {
            animator.SetTrigger(Gear);
        }

        private void OnPlayerCrashed()
        {
            animator.SetTrigger(Crash);
        }
    }
}
