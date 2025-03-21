using System.Collections;
using Scripts.Player;
using TMPro;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI collectedMoney;
        [SerializeField] private TextMeshProUGUI totalMoney;
        [SerializeField] private TextMeshProUGUI currentScore;
        [SerializeField] private TextMeshProUGUI allTimeScore;
        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private GameObject recordText;
        
        [SerializeField] private GameObject gameUI;
        [SerializeField] private GameObject menuUI;
        
        [SerializeField] private int highScore;
        [SerializeField] private int currentMoney;
        
        private static readonly int ShowScore = Animator.StringToHash("showScore");

        private int score;
        private int money;

        private void Awake()
        {
            GameManager.StartingGame += ShowStartCountDown;
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
            
            currentMoney = PlayerPrefs.GetInt("Money", 0);
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void OnDestroy()
        {
            GameManager.StartingGame -= ShowStartCountDown;
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
        }

        private void ShowStartCountDown()
        {
            StartCoroutine(CountDownRoutine());
            return;
            IEnumerator CountDownRoutine()
            {
                countDownText.text = "3";
                yield return new WaitForSeconds(1);
                countDownText.text = "2";
                yield return new WaitForSeconds(1);
                countDownText.text = "1";
                yield return new WaitForSeconds(1);
                countDownText.gameObject.SetActive(false);
            }
        }
        
        private void OnPlayerCrashed()
        {
            gameUI.SetActive(false);
            menuUI.SetActive(true);
        }

        public void UpdateScore(int score)
        {
            this.score = score;
            scoreText.text = score.ToString();
        }

        public void UpdateSpeed(int speed)
        {
            speedText.text = speed.ToString();
        }

        public void UpdateMoney(int money)
        {
            this.money += money;
            moneyText.text = this.money.ToString();
        }
    }
}