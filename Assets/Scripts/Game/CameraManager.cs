using System;
using Cinemachine;
using Scripts.Player;
using UnityEngine;

namespace Game
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private CinemachineVirtualCamera deathCamera;

        private void Awake()
        {
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
        }

        private void OnDestroy()
        {
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
        }

        private void OnPlayerCrashed()
        {
            followCamera.Priority -= 1;
            deathCamera.Priority += 1;
        }
    }
}