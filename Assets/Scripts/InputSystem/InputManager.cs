using System;
using Scripts.InputSystem;
using Scripts.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInputController inputController;
        [SerializeField] private float minimumSwipeDistance = 192;
        public static UnityAction<SwipeDirection> PlayerSwiped;
        public static UnityAction PlayerSwipedForFirstTime;
        private Vector2 startPosition;
        private bool firstSwipe = true;

        private void Awake()
        {
            inputController = new PlayerInputController();
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
        }

        private void OnDestroy()
        {
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
        }

        private void OnPlayerCrashed()
        {
            this.enabled = false;
        }

        private void OnEnable()
        {
            inputController.Enable();

            inputController.Player.PrimaryContact.performed += OnTouchStarted;
            inputController.Player.PrimaryContact.canceled += OnTouchEnded;
        }

        private void OnDisable()
        {
            inputController.Player.PrimaryContact.performed -= OnTouchStarted;
            inputController.Player.PrimaryContact.canceled -= OnTouchEnded;

            inputController.Disable();
        }

        private void OnTouchStarted(InputAction.CallbackContext context)
        {
            startPosition = inputController.Player.PrimaryPosition.ReadValue<Vector2>();
        }

        private void OnTouchEnded(InputAction.CallbackContext context)
        {
            Vector2 endPosition = inputController.Player.PrimaryPosition.ReadValue<Vector2>();

            Vector2 swipeVector = endPosition - startPosition;

            if (swipeVector.magnitude < minimumSwipeDistance)
            {
                return;
            }

            if (firstSwipe)
            {
                firstSwipe = false;
                PlayerSwipedForFirstTime?.Invoke();
            }
            if (swipeVector.x > 0)
            {
                PlayerSwiped.Invoke(SwipeDirection.Right);
            }
            else
            {
                PlayerSwiped.Invoke(SwipeDirection.Left);
            }
        }
    }
}