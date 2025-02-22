using System.Collections;
using DG.Tweening;
using TrafficSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private GameObject player;
        [SerializeField] private Road[] roads;
        [SerializeField] private int roadIndex;
        private int currentRoadLocation;
        private int playerTargetPos;
        public static float PlayerSpeed;
    
        public static UnityAction StartingGame;
        public static UnityAction GameStarted;

        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            playerTargetPos = 35;
            currentRoadLocation = 175;
            StartCoroutine(StartGame());
        }

        private void Update()
        {
            CheckPlayerPosition();
        }

        private void CheckPlayerPosition()
        {
            if (!(player.transform.position.z >= playerTargetPos)) return;
            GenerateRoad();
            playerTargetPos += 35;
        }

        private void GenerateRoad()
        {
            var lastRoad = roads[roadIndex];
            lastRoad.transform.position = new Vector3(0, 0, currentRoadLocation);
            lastRoad.Initialize();
            roadIndex++;
            roadIndex %= roads.Length;
            currentRoadLocation += 35;
        }

        public void PlayAgain()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            DOTween.KillAll();
            SceneManager.LoadScene(currentSceneName);
        }

        private IEnumerator StartGame()
        {
            StartingGame?.Invoke();
            yield return new WaitForSeconds(3);
            GameStarted?.Invoke();
        }
    
        public void GoToMenu()
        {
            DOTween.KillAll();
            SceneManager.LoadScene(0);
        }
    }
}