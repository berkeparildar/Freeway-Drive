using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject roadContainer;
    public GameObject roadPrefab;
    private static int _currentRoadLocation;
    private static int _playerTargetPos;
    [SerializeField] private GameObject _player;

    void Start()
    {
        _playerTargetPos = 5;
        _currentRoadLocation = 65;
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
            _playerTargetPos += 5;
        }
    }

    public void GenerateRoad()
    {
        var createdRoad = Instantiate(roadPrefab, new Vector3(0, 0, _currentRoadLocation), Quaternion.identity);
        createdRoad.transform.SetParent(roadContainer.transform);
        _currentRoadLocation += 5;
        Destroy(roadContainer.transform.GetChild(0).gameObject);
    }
}
