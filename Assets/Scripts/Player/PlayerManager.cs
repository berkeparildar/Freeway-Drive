using InputSystem;
using Player;
using Scripts.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private MovementController movementController;
        [SerializeField] private AnimationController animationController;
        [SerializeField] private SoundController soundController;

        public static UnityAction PlayerCrashed;

        private void Awake()
        {
            GameManager.GameStarted += OnGameStarted;
            InputManager.PlayerSwiped += OnPlayerSwiped;
        }

        private void OnGameStarted()
        {
            movementController.enabled = true;
            animationController.enabled = true;
            soundController.enabled = true;
        }

        private void OnDestroy()
        {
            GameManager.GameStarted -= OnGameStarted;
            InputManager.PlayerSwiped -= OnPlayerSwiped;
        }

        private void OnPlayerSwiped(SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.Left:
                     if (movementController.TurnLeft()) animationController.TurnLeft();
                    break;
                case SwipeDirection.Right:
                    if (movementController.TurnRight()) animationController.TurnRight();
                    break;
            }
        }
    }
}
