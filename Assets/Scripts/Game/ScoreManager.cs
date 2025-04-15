using System;
using System.Collections;
using Scripts.Player;
using UnityEngine;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int score;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PlayerManager player;
        private bool canIncreaseScore;

        private void Awake()
        {
            GameManager.GameStarted += OnGameStarted;
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
        }

        private void OnDestroy()
        {
            GameManager.GameStarted -= OnGameStarted;
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
        }

        private void Update()
        {
            if (canIncreaseScore)
            {
                score = (int)player.transform.position.z / 10;
                uiManager.UpdateScore(score);
            }
        }

        private void OnPlayerCrashed()
        {
            canIncreaseScore = false;
            var oldScore = PlayerPrefs.GetInt("HighScore", 0);
            if (score > oldScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }

        private void OnGameStarted()
        {
            canIncreaseScore = true;
        }

        public int GetScore()
        {
            return score;
        }
    }
}