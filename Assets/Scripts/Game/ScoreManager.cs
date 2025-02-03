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
        
        private void OnPlayerCrashed()
        {
            StopCoroutine(IncreaseScore());
            var oldScore = PlayerPrefs.GetInt("HighScore", 0);
            if (score > oldScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }

        private void OnGameStarted()
        {
            StartCoroutine(IncreaseScore());
        }

        private IEnumerator IncreaseScore()
        {
            while (true)
            {
                score += 1;
                uiManager.UpdateScore(score);
                yield return new WaitForSeconds(0.2f);
            }
        }

        public int GetScore()
        {
            return score;
        }
    }
}