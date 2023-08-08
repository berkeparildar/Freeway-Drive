using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TrafficCars : MonoBehaviour
{
    public Material[] carColors;
    private MeshRenderer _meshRenderer;
    private Animator _animator;
    private GameObject _leftSignal;
    private GameObject _rightSignal;
    private bool _canTurnAgain;
    private bool _turnRight;
    private bool _turnLeft;
    private float _speed;
    private float _turnSpeed;
    private Player _player;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
        _leftSignal = transform.GetChild(0).gameObject;
        _rightSignal = transform.GetChild(1).gameObject;
        _player = GameObject.Find("Player").GetComponent<Player>();
        SetColor();
        SetSpeed();
        StartCoroutine(RollPercentileDice());
        _canTurnAgain = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        KillIfBehind();
    }

    private void SetColor()
    {
        var randomNumber = Random.Range(0, carColors.Length);
        var currentMaterials = _meshRenderer.materials;
        currentMaterials[0] = carColors[randomNumber];
        _meshRenderer.materials = currentMaterials;
    }

    private void Movement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.right);
        if (_turnLeft && transform.position.x != -10 && _canTurnAgain)
        {
            _canTurnAgain = false;
            _turnLeft = false;
            StartCoroutine(TurnLeft());
            
        }
        else if (_turnRight && transform.position.x != -2.5f && _canTurnAgain)
        {
            _canTurnAgain = false;
            _turnRight = false;
            StartCoroutine(TurnRight());
            
        }
    }

    private IEnumerator RollPercentileDice()
    {
        while (true)
        {
            var random = Random.Range(0, 11);
            if (random == 3)
            {
                _turnRight = true;
            }
            else if (random == 7)
            {
                _turnLeft = true;
            }
            yield return new WaitForSeconds(4);
        }
    }

    private IEnumerator TurnLeft()
    {
        _leftSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _leftSignal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _leftSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _leftSignal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _animator.SetTrigger("left");
        transform.DOMoveX(-2.5f, _turnSpeed).SetRelative().OnComplete(() => {_canTurnAgain = true;});
        _leftSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _leftSignal.SetActive(false);
    }

    private IEnumerator TurnRight()
    {
        _rightSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _rightSignal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _rightSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _rightSignal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _animator.SetTrigger("right");
        transform.DOMoveX(2.5f, _turnSpeed).SetRelative().OnComplete(() => {_canTurnAgain = true;});
        _rightSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _rightSignal.SetActive(false);
    }

    private void SetSpeed()
    {
        var playerSpeed = _player.GetSpeed();
        _speed = playerSpeed * 0.75f;
        _turnSpeed = 2;
    }

    private void KillIfBehind()
    {
        if (_player.transform.position.z > transform.position.z + 10)
        {
            Destroy(this.gameObject);
        }
    }

    public void Stop()
    {
        _speed = 0;
    }
}