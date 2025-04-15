using System.Collections;
using Cinemachine;
using DG.Tweening;
using InputSystem;
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
        [SerializeField] private InputManager inputManager;
        [SerializeField] private CinemachineVirtualCamera startCamera;
        private int currentRoadLocation;
        private int playerTargetPos;
        public static float PlayerSpeed;
    
        public static UnityAction StartingGame;
        public static UnityAction GameStarted;

        private void Awake()
        {
            Debug.Log(startCamera.Priority);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            playerTargetPos = -35;
            currentRoadLocation = 105;
            StartCoroutine(StartGame());
            StartCoroutine(StartCamera());
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
            inputManager.gameObject.SetActive(true);
        }

        private IEnumerator StartCamera()
        {
            yield return new WaitForSeconds(0.1f);
            startCamera.Priority = 1;
        }
    
        public void GoToMenu()
        {
            DOTween.KillAll();
            SceneManager.LoadScene(0);
        }
    }
}