using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TrafficCars : MonoBehaviour
{
    [SerializeField] private Material[] carColors;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject leftSignal;
    [SerializeField] private GameObject rightSignal;
    [SerializeField] private bool canTurnAgain;
    [SerializeField] private bool turnRight;
    [SerializeField] private bool turnLeft;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Player player;
    [SerializeField] private bool willPlayHorn;
    [SerializeField] private AudioSource hornPlayer;
    private static readonly int Right = Animator.StringToHash("right");
    private static readonly int Left = Animator.StringToHash("left");

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        SetColor();
        SetSpeed();
        StartCoroutine(RollPercentileDice());
        canTurnAgain = true;
        var hornChance =  Random.Range(0, 4);
        willPlayHorn = hornChance == 2;
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
        transform.Translate(speed * Time.deltaTime * Vector3.right);
        if (turnLeft && Math.Abs(transform.position.x - (-10)) > 2 && canTurnAgain)
        {
            canTurnAgain = false;
            turnLeft = false;
            StartCoroutine(TurnLeft());
            
        }
        else if (turnRight && Math.Abs(transform.position.x - (-2.5f)) > 2 && canTurnAgain)
        {
            canTurnAgain = false;
            turnRight = false;
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
                turnRight = true;
            }
            else if (random == 7)
            {
                turnLeft = true;
            }
            yield return new WaitForSeconds(4);
        }
    }

    private IEnumerator TurnLeft()
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
        transform.DOMoveX(-2.5f, turnSpeed).SetRelative().OnComplete(() => {canTurnAgain = true;});
        leftSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        leftSignal.SetActive(false);
    }

    private IEnumerator TurnRight()
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
        transform.DOMoveX(2.5f, turnSpeed).SetRelative().OnComplete(() => {canTurnAgain = true;});
        rightSignal.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        rightSignal.SetActive(false);
    }

    private void SetSpeed()
    {
        var playerSpeed = player.GetSpeed();
        speed = playerSpeed * 0.75f;
        turnSpeed = 2;
    }

    private void KillIfBehind()
    {
        if (Mathf.Abs(player.transform.position.z - transform.position.z) < 1 && willPlayHorn)
        {
            hornPlayer.Play();
            willPlayHorn = false;
        }
        if (player.transform.position.z > transform.position.z + 10)
        {
            gameObject.SetActive(false);
        }
    }

    public void Stop()
    {
        speed = 0;
    }
}