using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject roadContainer;
    [SerializeField] private GameObject carContainer;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject speedIndicator;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject recordText;
    [SerializeField] private TextMeshProUGUI reviveCountDown;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI collectedMoney;
    [SerializeField] private TextMeshProUGUI totalMoney;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI allTimeScore;
    [SerializeField] private Animator carAnimator;
    [SerializeField] private Animator uiAnimator;
    [SerializeField] private MeshRenderer carRenderer;
    [SerializeField] private bool crashInit;
    [SerializeField] private GameObject player;
    [SerializeField] private Material[] colors;
    [SerializeField] private RewardedAdsButton rewardedAdsButton;
    [SerializeField] private AudioSource crash;
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private CinemachineVirtualCamera deathCamera;
    public static readonly Dictionary<string, float> SpawnPointsIndex = new();
    public static bool playedAd;
    private static int _currentRoadLocation;
    private static int _playerTargetPos;
    private static int _money;
    private static int _score; 
    private static readonly int ShowScore = Animator.StringToHash("showScore");
    private static readonly int Revive = Animator.StringToHash("revive");

    private void Start()
    {
        _score = 0;
        _money = 0;
        _playerTargetPos = 15;
        _currentRoadLocation = 90;
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
        UpdateUI();
        CheckPlayerPosition();
        CheckCrash();
    }

    private void CheckPlayerPosition()
    {
        if (player.transform.position.z >= _playerTargetPos)
        {
            GenerateRoad();
            _playerTargetPos += 15;
        }
    }

    private void GenerateRoad()
    {
        var createdRoad = Instantiate(roadPrefab, new Vector3(0, 0, _currentRoadLocation), Quaternion.identity);
        createdRoad.transform.SetParent(roadContainer.transform);
        _currentRoadLocation += 15;
        Destroy(roadContainer.transform.GetChild(0).gameObject);
    }

    public static void IncreaseMoney(int money)
    {
        _money += money;
    }

    private IEnumerator StopAllCars()
    {
        yield return new WaitForSeconds(1);
        for (var i = 0; i < carContainer.transform.childCount; i++)
        {
            carContainer.transform.GetChild(i).GetComponent<TrafficCars>().Stop();
        }
    }

    private void UpdateUI()
    {
        moneyText.text = _money.ToString();
        speedText.text = (Mathf.Round(player.GetComponent<Player>().GetSpeed() * 5)).ToString();
        scoreText.text = (_score).ToString();
    }

    private IEnumerator IncreaseScore()
    {
        while (!Player.hasCrashed)
        {
            _score += 1;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    private void CheckCrash()
    {
        if (!Player.hasCrashed || crashInit) return;
        followCamera.Priority -= 1;
        deathCamera.Priority += 1;
        crashInit = true;
        StartCoroutine(StopAllCars());
        crash.Play();
        playerSource.Stop();
        DOTween.Kill(speedIndicator.transform);
        gameUI.SetActive(false);
        menuUI.SetActive(true);
        if (playedAd) return;
        playedAd = true;
        rewardedAdsButton.LoadAd();
    }

    public void ContinueGame()
    {
        for (int i = 0; i < carContainer.transform.childCount; i++)
        {
            DOTween.instance.DOKill(carContainer.transform.GetChild(i).transform);
            DOTween.Kill(carContainer.transform.GetChild(i).transform);
            Destroy(carContainer.transform.GetChild(i).gameObject);
        }
        StartCoroutine(ReviveRoutine());
    }

    private IEnumerator ReviveRoutine()
    {
        followCamera.Priority += 1;
        deathCamera.Priority -= 1;
        menuUI.SetActive(false);
        gameUI.SetActive(true);
        carAnimator.SetTrigger(Revive);
        yield return new WaitForSeconds(1.5f);
        reviveCountDown.gameObject.SetActive(true);
        reviveCountDown.text = "3";
        yield return new WaitForSeconds(1);
        reviveCountDown.text = "2";
        yield return new WaitForSeconds(1);
        reviveCountDown.text = "1";
        yield return new WaitForSeconds(1);
        reviveCountDown.gameObject.SetActive(false);
        Player.hasCrashed = false;
        playerSource.Play();
        speedIndicator.transform.DORotate(new Vector3(0, 0, -180), 90).SetRelative().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        StartCoroutine(IncreaseScore());
        StartCoroutine(player.GetComponent<Player>().SpeedIncreaseCoroutine());
        StartCoroutine(player.GetComponent<Player>().CarSoundCoroutine());
        crashInit = false;
    }

    public void ShowScoreUI()
    {
        uiAnimator.SetTrigger(ShowScore);
        var highScore = PlayerPrefs.GetInt("HighScore", 0);
        var currentMoney = PlayerPrefs.GetInt("Money", 0);
        currentMoney += _money;
        if (_score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", _score);
            recordText.SetActive(true);
        }
        collectedMoney.text = _money.ToString();
        totalMoney.text = currentMoney.ToString();
        currentScore.text = _score.ToString();
        allTimeScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        PlayerPrefs.SetInt("Money", currentMoney);
        PlayerPrefs.Save();
    }

    public void PlayAgain()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        DOTween.KillAll();
        SceneManager.LoadScene(currentSceneName);
        SpawnPointsIndex.Clear();
        Player.hasCrashed = false;
    }

    private IEnumerator StartGame()
    {
        SetColor();
        reviveCountDown.gameObject.SetActive(true);
        reviveCountDown.text = "3";
        yield return new WaitForSeconds(1);
        reviveCountDown.text = "2";
        yield return new WaitForSeconds(1);
        reviveCountDown.text = "1";
        yield return new WaitForSeconds(1);
        reviveCountDown.gameObject.SetActive(false);
        player.GetComponent<Player>().enabled = true;
        StartCoroutine(IncreaseScore());
        speedIndicator.transform.DORotate(new Vector3(0, 0, -180), 90).SetRelative().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        for (int i = 0; i < 10; i++)
        {
            GenerateRoad();
        }
    }

    private void SetColor()
    {
        var currentColor = PlayerPrefs.GetInt("Color", 0);
        var currentMaterials = carRenderer.materials;
        currentMaterials[0] = colors[currentColor];
        carRenderer.materials = currentMaterials;
    }
    
    public void GoToMenu()
    {
        DOTween.KillAll();
        Player.hasCrashed = false;
        SpawnPointsIndex.Clear();
        Player.hasCrashed = false;
        SceneManager.LoadScene(0);
    }
}