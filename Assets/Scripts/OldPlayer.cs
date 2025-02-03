using System.Collections;
using UnityEngine;
using DG.Tweening;

public class OldPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator carAnimator;
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool canTurnAgain = true;
    [SerializeField] private Vector2 touchStartPosition;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip carSoundTwo;
    [SerializeField] private AudioClip carSoundThree;
    [SerializeField] private int revolutionCount;
    [SerializeField] private int maxRpm;
    [SerializeField] private float startingPitch;
    public static bool hasCrashed;
    private static readonly int Crash = Animator.StringToHash("crash");
    private static readonly int Left = Animator.StringToHash("left");
    private static readonly int Right = Animator.StringToHash("right");

    private void Start()
    {
        //StartCoroutine(SpeedIncreaseRoutine());
        //StartCoroutine(CarSoundCoroutine());
    }

    private void Update()
    {
        //Movement();
    }

    /*
    private void Movement()
    {
        if (hasCrashed) return;
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
        if (Input.touchCount <= 0) return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            touchStartPosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            Vector2 swipeDelta = touch.position - touchStartPosition;

            if (Mathf.Abs(swipeDelta.x) > 192)
            {
                if (swipeDelta.x < 0 && transform.position.x != -10 && canTurnAgain)
                {
                    canTurnAgain = false;
                    carAnimator.SetTrigger(Left);
                    transform.DOMoveX(-2.5f, turnSpeed).SetRelative()
                        .OnComplete(() => { canTurnAgain = true; });
                }
                else if (swipeDelta.x > 0 && transform.position.x != -2.5f && canTurnAgain)
                {
                    canTurnAgain = false;
                    carAnimator.SetTrigger(Right);
                    transform.DOMoveX(2.5f, turnSpeed).SetRelative().OnComplete(() => { canTurnAgain = true; });
                }
            }
        }
    }
    */

    /*
    public IEnumerator SpeedIncreaseRoutine()
    {
        while (!hasCrashed)
        {
            speed += 0.2f;
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }
    */

    public float GetSpeed()
    {
        return speed;
    }

    public void IncreaseHandling()
    {
        turnSpeed *= 0.95f;
        carAnimator.speed /= 0.95f;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Car")) return;
        carAnimator.SetTrigger(Crash);
        hasCrashed = true;
    }*/

    /*
    public IEnumerator CarSoundCoroutine()
    {
        if (!hasCrashed)
        {
            while (revolutionCount < maxRpm)
            {
                if (hasCrashed)
                {
                    yield break;
                }
                audioSource.pitch += 0.001f;
                revolutionCount++;
                yield return new WaitForSeconds(0.033f);
            }
            if (audioSource.clip == carSoundThree)
            {
                yield break;
            }
            carAnimator.SetTrigger(Gear);
            yield return StartCoroutine(GearShiftRoutine(true));
            revolutionCount = 0;
            startingPitch += 0.5f;
            audioSource.pitch = startingPitch;
            audioSource.clip = audioSource.clip == carSoundTwo ? carSoundThree : carSoundTwo;
            audioSource.Play();
            yield return StartCoroutine(GearShiftRoutine(false));
            yield return StartCoroutine(CarSoundCoroutine());
        }
        else
        {
            yield break;
        }
    }
    */
}