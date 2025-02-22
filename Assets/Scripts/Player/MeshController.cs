using System;
using System.Collections;
using CollectibleSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class MeshController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material[] colors;
        [SerializeField] private Material[] ghostedMaterials;

        private void Awake()
        {
            CollectibleGhostPowerUp.CollectedGhostPowerUp += OnCollectedGhostPowerUp;
            SetColor();
        }

        private void OnDestroy()
        {
            CollectibleGhostPowerUp.CollectedGhostPowerUp -= OnCollectedGhostPowerUp;
        }

        private void OnCollectedGhostPowerUp(int time)
        {
            StartCoroutine(GhostMaterialRoutine());
            return;
            IEnumerator GhostMaterialRoutine()
            {
                var currentMaterials = meshRenderer.materials;
                meshRenderer.materials = ghostedMaterials;
                yield return new WaitForSeconds(time);
                meshRenderer.materials = currentMaterials;
            }
        }

        private void SetColor()
        {
            var currentColor = PlayerPrefs.GetInt("Color", 0);
            var currentMaterials = meshRenderer.materials;
            currentMaterials[0] = colors[currentColor];
            meshRenderer.materials = currentMaterials;
        }
    }
}