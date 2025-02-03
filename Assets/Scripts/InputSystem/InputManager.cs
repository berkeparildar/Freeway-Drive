using Scripts.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInputController inputController;

        // Minimum distance (in screen pixels) to consider it a valid swipe
        [SerializeField] private float minimumSwipeDistance = 192;

        public static UnityAction<SwipeDirection> PlayerSwiped;

        // We’ll store the touch start position here
        private Vector2 startPosition;

        private void Awake()
        {
            inputController = new PlayerInputController();
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
            Debug.LogWarning("OnTouchStarted");
            startPosition = inputController.Player.PrimaryPosition.ReadValue<Vector2>();
        }

        private void OnTouchEnded(InputAction.CallbackContext context)
        {
            Debug.LogWarning("OnTouchEnded");
            Vector2 endPosition = inputController.Player.PrimaryPosition.ReadValue<Vector2>();

            Vector2 swipeVector = endPosition - startPosition;

            if (swipeVector.magnitude < minimumSwipeDistance)
            {
                return;
            }
            Debug.LogWarning(swipeVector);
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