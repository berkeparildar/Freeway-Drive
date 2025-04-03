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
        [SerializeField] private float speed;
        
        private static readonly int ShowScore = Animator.StringToHash("showScore");

        private int score;
        private int money;

        private void Awake()
        {
            speed = 20;
            GameManager.StartingGame += ShowStartCountDown;
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
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
            collectedMoney.text = money.ToString();
            currentMoney = PlayerPrefs.GetInt("Money", 0);
            totalMoney.text = currentMoney.ToString();
            currentScore.text = score.ToString();
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            allTimeScore.text = highScore.ToString();
            if (score >= highScore)
            {
                recordText.gameObject.SetActive(true);
            }
        }

        public void UpdateScore(int score)
        {
            this.score = score;
            scoreText.text = score.ToString();
        }

        public void UpdateSpeed()
        {
            speed += 0.5f;
            speedText.text = ((int)speed).ToString();
        }

        public void UpdateMoney(int money)
        {
            this.money += money;
            moneyText.text = this.money.ToString();
        }
    }
}