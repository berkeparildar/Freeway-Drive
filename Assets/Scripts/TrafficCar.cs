using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Game;
using Scripts.Player;
using Random = UnityEngine.Random;

public class TrafficCar : MonoBehaviour
{
    [SerializeField] private Material[] carColors;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject leftSignal;
    [SerializeField] private GameObject rightSignal;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private PlayerManager player;
    [SerializeField] private float turnCooldown;

    private static readonly int Right = Animator.StringToHash("right");
    private static readonly int Left = Animator.StringToHash("left");

    private void Awake()
    {
        PlayerManager.PlayerCrashed += OnPlayerCrashed;
        player ??= FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        SetColor();
        GetSpeed();
        StartCoroutine(ChangeLaneRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        DisableSignals();
    }

    private void DisableSignals()
    {
        leftSignal.gameObject.SetActive(false);
        rightSignal.gameObject.SetActive(false);
    }

    private void Update()
    {
        Movement();
        KillIfBehind();
    }

    private void SetColor()
    {
        var randomNumber = Random.Range(0, carColors.Length);
        var currentMaterials = meshRenderer.materials;
        currentMaterials[0] = carColors[randomNumber];
        meshRenderer.materials = currentMaterials;
        if (!CompareTag("Truck") && !CompareTag("Tanker")) return;
        var cargo = transform.GetChild(2);
        var cargoMats = cargo.GetComponent<MeshRenderer>().materials;
        cargoMats[0] = carColors[randomNumber];
        cargo.GetComponent<MeshRenderer>().materials = cargoMats;
    }

    private void Movement()
    {
        transform.Translate(speed * Time.deltaTime * transform.forward);
    }

    private void KillIfBehind()
    {
        if (player.transform.position.z > transform.position.z + 10)
        {
            PoolManager.ReturnTrafficCar(this);
        }
    }

    private IEnumerator ChangeLaneRoutine()
    {
        while (true)
        {
            var randomInteger = Random.Range(0, 5);
            
            if (randomInteger == 0)
            {
                if (transform.position.x > -9)
                {
                    yield return StartCoroutine(TurnLeftRoutine());
                }
            }

            if (randomInteger == 1)
            {
                if (transform.position.x < -3.5f)
                {
                    yield return StartCoroutine(TurnRightRoutine());
                }
            }

            yield return new WaitForSeconds(4);
        }
    }

    private IEnumerator TurnLeftRoutine()
    {
        leftSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        leftSignal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        leftSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        leftSignal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger(Left);
        yield return transform.DOMoveX(-2.5f, turnSpeed)
            .SetRelative()
            .WaitForCompletion();
        leftSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        leftSignal.SetActive(false);
    }

    private IEnumerator TurnRightRoutine()
    {
        rightSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        rightSignal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        rightSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        rightSignal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger(Right);
        yield return transform.DOMoveX(2.5f, turnSpeed)
            .SetRelative()
            .WaitForCompletion();
        rightSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        rightSignal.SetActive(false);
    }

    private void GetSpeed()
    {
        speed = GameManager.PlayerSpeed * 0.75f;
        turnSpeed = 2;
    }

    private void OnPlayerCrashed()
    {
        speed = 0;
    }
}