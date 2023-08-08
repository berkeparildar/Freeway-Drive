using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private Car _car;
    private float _speed;
    private Animator _carAnimator;
    private float _turnSpeed;
    private bool _canTurnAgain = true;
    private float _roadInitTarget = 5;
    private GameManager _gameManager;
    void Start()
    {
        _speed = 3;
        _turnSpeed = 2; 
        _car = transform.GetChild(0).GetComponent<Car>();
        _carAnimator = _car.GetComponent<Animator>();
        StartCoroutine(SpeedIncreaseCoroutine());
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.forward);
        if (Input.GetKeyDown(KeyCode.A) && transform.position.x != -10 && _canTurnAgain)
        {
            _canTurnAgain = false;
            _carAnimator.SetTrigger("left");
            transform.DOMoveX(-2.5f, _turnSpeed).SetRelative().OnComplete(() => {_canTurnAgain = true;});
        }
        else if (Input.GetKeyDown(KeyCode.D) && transform.position.x != -2.5f && _canTurnAgain)
        {
            _canTurnAgain = false;
            _carAnimator.SetTrigger("right");
            transform.DOMoveX(2.5f, _turnSpeed).SetRelative().OnComplete(() => {_canTurnAgain = true;});
        }
    }

    private IEnumerator SpeedIncreaseCoroutine()
    {
        while (true)
        {
            _speed += 1;
            yield return new WaitForSeconds(2);
        }
    }

    private void CreateRoad()
    {
        if (transform.position.z >= _roadInitTarget)
        {
            _roadInitTarget += 5;
        }
    }
}
