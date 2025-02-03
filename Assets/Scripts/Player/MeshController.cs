using System;
using UnityEngine;

namespace Player
{
    public class MeshController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer renderer;
        [SerializeField] private Material[] colors;

        private void Awake()
        {
            SetColor();
        }

        private void SetColor()
        {
            var currentColor = PlayerPrefs.GetInt("Color", 0);
            var currentMaterials = renderer.materials;
            currentMaterials[0] = colors[currentColor];
            renderer.materials = currentMaterials;
        }
    }
}