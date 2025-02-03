using System;
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
        }

        private void OnDestroy()
        {
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
        }

        public void TurnLeft()
        {
            animator.SetTrigger(Left);    
        }

        public void TurnRight()
        {
            animator.SetTrigger(Right);
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
