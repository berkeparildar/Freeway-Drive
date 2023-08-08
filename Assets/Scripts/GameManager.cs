using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject roadContainer;
    public GameObject roadPrefab;
    private static int _currentRoadLocation;
    private static int _playerTargetPos;
    private static int _money;
    [SerializeField] private GameObject _player;
    public static Dictionary<string, float> _spawnPointsIndex = new();

    void Start()
    {
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
        for (int i = 0; i < 10; i++)
        {
            GenerateRoad();
        }
    }

    void Update()
    {
        CheckPlayerPosition();
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

    public static void StopAllCars()
    {
        var carContainer = GameObject.Find("CarContainer");
        for (int i = 0; i < carContainer.transform.childCount; i++)
        {
            carContainer.transform.GetChild(i).GetComponent<TrafficCars>().Stop();
        }
    }
}
