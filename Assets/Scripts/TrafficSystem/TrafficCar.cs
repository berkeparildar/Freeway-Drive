using System.Collections;
using DG.Tweening;
using Game;
using Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TrafficSystem
{
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
        [SerializeField] private int laneIndex;
        [SerializeField] private MeshRenderer cargoRenderer;

        private static readonly int Right = Animator.StringToHash("right");
        private static readonly int Left = Animator.StringToHash("left");

        private void Awake()
        {
            player ??= FindObjectOfType<PlayerManager>();
        }

        private void OnEnable()
        {
            PlayerManager.PlayerCrashed += OnPlayerCrashed;

            SetColor();
            GetSpeed();
            StartCoroutine(ChangeLaneRoutine());
        }

        private void OnDisable()
        {
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;

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
            if (cargoRenderer == null) return;
            var cargoMats = cargoRenderer.materials;
            cargoMats[0] = carColors[randomNumber];
            cargoRenderer.materials = cargoMats;
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
                yield return new WaitForSeconds(2);
                var randomInteger = Random.Range(0, 5);
                if (randomInteger == 0)
                {
                    if (laneIndex > 0)
                    {
                        yield return StartCoroutine(TurnLeftRoutine());
                    }
                }

                if (randomInteger == 1)
                {
                    if (laneIndex < 3)
                    {
                        yield return StartCoroutine(TurnRightRoutine());
                    }
                }

                yield return new WaitForSeconds(2);
            }
        }

        private IEnumerator TurnLeftRoutine()
        {
            laneIndex--;
            leftSignal.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            leftSignal.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            leftSignal.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            leftSignal.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger(Left);
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMoveX(-2.5f, turnSpeed)
                .SetRelative());
            seq.Join(
                DOTween.Sequence()
                    .AppendCallback(() => leftSignal.SetActive(true))
                    .AppendInterval(0.5f)
                    .AppendCallback(() => leftSignal.SetActive(false))
            );
            yield return seq.WaitForCompletion();
        }

        private IEnumerator TurnRightRoutine()
        {
            laneIndex++;
            rightSignal.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            rightSignal.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            rightSignal.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            rightSignal.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger(Right);
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMoveX(2.5f, turnSpeed)
                .SetRelative());
            seq.Join(
                DOTween.Sequence()
                    .AppendCallback(() => rightSignal.SetActive(true))
                    .AppendInterval(0.5f)
                    .AppendCallback(() => rightSignal.SetActive(false))
            );
            yield return seq.WaitForCompletion();
        }

        private void GetSpeed()
        {
            speed = GameManager.PlayerSpeed * 0.75f;
            turnSpeed = 2;
        }

        public void SetLane(int lane)
        {
            laneIndex = lane;
        }

        private void OnPlayerCrashed()
        {
            StartCoroutine(StopRoutine());
            return;

            IEnumerator StopRoutine()
            {
                yield return new WaitForSeconds(1);
                speed = 0;
            }
        }
    }
}