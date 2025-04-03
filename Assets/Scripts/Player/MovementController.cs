using System;
using System.Collections;
using CollectibleSystem;
using DG.Tweening;
using Game;
using Scripts.Player;
using UnityEngine;

namespace Player
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float turnSpeed;
        [SerializeField] private UIManager uiManager;

        private Tween turnRightTween;
        private Tween turnLeftTween;
        private void OnEnable()
        {
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
            CollectibleHandlingPowerUp.CollectedHandlingPowerUp += OnCollectedHandlingPowerUp;
            StartCoroutine(SpeedIncreaseRoutine());
        }

        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
            CollectibleHandlingPowerUp.CollectedHandlingPowerUp -= OnCollectedHandlingPowerUp;
        }
        
        private void OnCollectedHandlingPowerUp(float speedMultiplier)
        {
            turnSpeed *= speedMultiplier;
        }

        private void OnPlayerCrashed()
        {
            this.enabled = false;
        }
        
        public bool TurnLeft()
        {
            if (transform.position.x == -10) return false;
            if (IsTurning()) return false;
            turnLeftTween = transform.DOMoveX(-2.5f, turnSpeed).SetRelative();
            return true;
        }

        public bool TurnRight()
        {
            if (transform.position.x == -2.5f) return false;
            if (IsTurning()) return false;
            turnLeftTween = transform.DOMoveX(2.5f, turnSpeed).SetRelative();
            return true;
        }

        private bool IsTurning()
        {
            if (turnLeftTween.IsActive())
                return true;

            if (turnRightTween.IsActive())
                return true;

            return false;
        }

        private IEnumerator SpeedIncreaseRoutine()
        {
            while (true)
            {
                uiManager.UpdateSpeed();
                speed += 0.2f;
                GameManager.PlayerSpeed = speed;
                yield return new WaitForSeconds(1);
            }
        }
    }
}

