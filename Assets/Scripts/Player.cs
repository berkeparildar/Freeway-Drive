using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private Car _car;
    private float _speed;
    private Animator _animator;
    private Animator _carAnimator;
    private float _turnSpeed;
    private bool _canTurnAgain = true;
    private bool _hasCrashed;
    void Start()
    {
        _speed = 3;
        _turnSpeed = 2; 
        _car = transform.GetChild(0).GetComponent<Car>();
        _animator = GetComponent<Animator>();
        _carAnimator = _car.GetComponent<Animator>();
        StartCoroutine(SpeedIncreaseCoroutine());
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
        while (!_hasCrashed)
        {
            _speed += 1;
            yield return new WaitForSeconds(4);
        }
        yield return null;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void IncreaseHandling()
    {
        _turnSpeed *= 0.95f;
        _carAnimator.speed /= 0.95f;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            _animator.SetTrigger("crash");
            StartCoroutine(StopCarsRoutine());
            _hasCrashed = true;
            _speed = 0;
        }
    }

    private IEnumerator StopCarsRoutine()
    {
        yield return new WaitForSeconds(1);
        GameManager.StopAllCars();
    }
}
