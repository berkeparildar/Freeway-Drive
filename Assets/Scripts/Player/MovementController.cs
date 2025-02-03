using System;
using System.Collections;
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

        private void Awake()
        {
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
        }
        
        private void OnEnable()
        {
            StartCoroutine(SpeedIncreaseRoutine());
        }
        
        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
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
                speed += 0.2f;
                uiManager.UpdateSpeed((int)speed);
                GameManager.PlayerSpeed = speed;
                yield return new WaitForSeconds(1);
            }
        }
    }
}

