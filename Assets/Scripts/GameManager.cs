using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject roadContainer;
    public GameObject carContainer;
    public GameObject roadPrefab;
    public GameObject speedIndicator;
    public GameObject gameUI;
    public GameObject menuUI;
    public GameObject recordText;
    public TextMeshProUGUI reviveCountDown;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI collectedMoney;
    public TextMeshProUGUI totalMoney;
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI allTimeScore;
    public Animator playerAnimator;
    public Animator UIAnimator;
    public MeshRenderer carRenderer;
    private static int _currentRoadLocation;
    private static int _playerTargetPos;
    private static int _money;
    private static int _score;
    private bool _crashInit;
    [SerializeField] private GameObject _player;
    public static Dictionary<string, float> _spawnPointsIndex = new();
    public Material[] colors;
    [SerializeField] RewardedAdsButton rewardedAdsButton;


    void Start()
    {
        _score = 0;
        _money = 0;
        _playerTargetPos = 15;
        _currentRoadLocation = 90;
        _spawnPointsIndex.Add("Bus", 8.32f);
        _spawnPointsIndex.Add("Mini", 7.922f);
        _spawnPointsIndex.Add("Golf", 7.895f);
        _spawnPointsIndex.Add("Pickup", 7.895f);
        _spawnPointsIndex.Add("Doblo", 8.139f);
        _spawnPointsIndex.Add("Sport", 7.938f);
        _spawnPointsIndex.Add("Truck", 8.5f);
        _spawnPointsIndex.Add("Tanker", 8.384f);
        StartCoroutine(StartGame());
    }

    void Update()
    {
        UpdateUI();
        CheckPlayerPosition();
        CheckCrash();
    }

    private void CheckPlayerPosition()
    {
        if (_player.transform.position.z >= _playerTargetPos)
        {
            GenerateRoad();
            _playerTargetPos += 15;
        }
    }

    public void GenerateRoad()
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
        for (int i = 0; i < carContainer.transform.childCount; i++)
        {
            carContainer.transform.GetChild(i).GetComponent<TrafficCars>().Stop();
        }
    }

    private void UpdateUI()
    {
        moneyText.text = _money.ToString();
        speedText.text = (_player.GetComponent<Player>().GetSpeed() * 5).ToString();
        scoreText.text = (_player.GetComponent<Player>().GetSpeed() + _score).ToString();
    }

    private IEnumerator IncreaseScore()
    {
        while (!Player.HasCrashed)
        {
            _score += 1;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    private void CheckCrash()
    {
        if (Player.HasCrashed && !_crashInit)
        {
            _crashInit = true;
            StartCoroutine(StopAllCars());
            DOTween.Kill(speedIndicator.transform);
            gameUI.SetActive(false);
            menuUI.SetActive(true);
            rewardedAdsButton.LoadAd();
        }
    }

    public void ContinueGame()
    {
        for (int i = 0; i < carContainer.transform.childCount; i++)
        {
            Destroy(carContainer.transform.GetChild(i).gameObject);
        }
        StartCoroutine(ReviveRoutine());
    }

    private IEnumerator ReviveRoutine()
    {
        menuUI.SetActive(false);
        gameUI.SetActive(true);
        playerAnimator.SetTrigger("revive");
        yield return new WaitForSeconds(1.5f);
        reviveCountDown.gameObject.SetActive(true);
        reviveCountDown.text = "3";
        yield return new WaitForSeconds(1);
        reviveCountDown.text = "2";
        yield return new WaitForSeconds(1);
        reviveCountDown.text = "1";
        yield return new WaitForSeconds(1);
        reviveCountDown.gameObject.SetActive(false);
        Player.HasCrashed = false;
        speedIndicator.transform.DORotate(new Vector3(0, 0, -180), 90).SetRelative().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        StartCoroutine(IncreaseScore());
        StartCoroutine(_player.GetComponent<Player>().SpeedIncreaseCoroutine());
        _crashInit = false;
    }

    public void ShowScoreUI()
    {
        UIAnimator.SetTrigger("showScore");
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
        _spawnPointsIndex.Clear();
        Player.HasCrashed = false;
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
        _player.GetComponent<Player>().enabled = true;
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
        Player.HasCrashed = false;
        _spawnPointsIndex.Clear();
        Player.HasCrashed = false;
        SceneManager.LoadScene(0);
    }

}
