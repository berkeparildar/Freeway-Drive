using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Road[] roads;
    [SerializeField] private int roadIndex;
    [SerializeField] private AudioSource backgroundMusic;
    public static readonly Dictionary<string, float> SpawnPointsIndex = new();
    private int currentRoadLocation;
    private int playerTargetPos;
    public static float PlayerSpeed;
    
    public static UnityAction StartingGame;
    public static UnityAction GameStarted;
    public static UnityAction<int> MoneyCollected;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        playerTargetPos = 15;
        currentRoadLocation = 150;
        SpawnPointsIndex.Add("Bus", 8.32f);
        SpawnPointsIndex.Add("Mini", 7.922f);
        SpawnPointsIndex.Add("Golf", 7.895f);
        SpawnPointsIndex.Add("Pickup", 7.995f);
        SpawnPointsIndex.Add("Doblo", 8.139f);
        SpawnPointsIndex.Add("Sport", 7.938f);
        SpawnPointsIndex.Add("Truck", 8.5f);
        SpawnPointsIndex.Add("Tanker", 8.384f);
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
        playerTargetPos += 15;
    }

    private void GenerateRoad()
    {
        var lastRoad = roads[roadIndex];
        lastRoad.transform.position = new Vector3(0, 0, currentRoadLocation);
        lastRoad.Initialize();
        roadIndex++;
        roadIndex %= roads.Length;
        currentRoadLocation += 15;
    }

    public void PlayAgain()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        DOTween.KillAll();
        SceneManager.LoadScene(currentSceneName);
        SpawnPointsIndex.Clear();
    }

    private IEnumerator StartGame()
    {
        StartingGame?.Invoke();
        yield return new WaitForSeconds(3);
        GameStarted?.Invoke();
    }
    
    public void GoToMenu()
    {
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        backgroundMusic.Stop();
        DOTween.KillAll();
        OldPlayer.hasCrashed = false;
        SpawnPointsIndex.Clear();
        OldPlayer.hasCrashed = false;
        SceneManager.LoadScene(0);
    }
}