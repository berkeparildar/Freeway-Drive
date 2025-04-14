using System.Collections;
using Scripts.Player;
using UnityEngine;

namespace Player
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] gearCarSounds;
        [SerializeField] private AudioClip crashSound;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float startingPitch;
        [SerializeField] private float pitchIncreaseAmount;
        [SerializeField] private float targetPitch;
        [SerializeField] private int gearCarSoundIndex;
        [SerializeField] private AnimationController animationController;

        private void Awake()
        {
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
        }

        private void OnDestroy()
        {
            PlayerManager.PlayerCrashed -= OnPlayerCrashed;
        }

        private void OnEnable()
        {
            StartCoroutine(CarSoundRoutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnPlayerCrashed()
        {
            audioSource.loop = false;
            audioSource.pitch = 1;
            audioSource.clip = crashSound;
            audioSource.volume -= 0.3f;
            targetPitch = 10;
            StopCoroutine(CarSoundRoutine());
            audioSource.Play();
        }

        private IEnumerator CarSoundRoutine()
        {
            if (gearCarSoundIndex == gearCarSounds.Length - 1)
            {
                yield break;
            }
            
            while (audioSource.pitch < targetPitch)
            {
                audioSource.pitch += pitchIncreaseAmount;
                yield return new WaitForSeconds(0.5f);
            }
            
            animationController.IncreaseGear();
            yield return StartCoroutine(GearShiftRoutine());
            startingPitch += 0.5f;
            gearCarSoundIndex++;
            audioSource.pitch = startingPitch;
            audioSource.clip = gearCarSounds[gearCarSoundIndex];
            audioSource.Play();
            yield return StartCoroutine(GearShiftRoutine());
            yield return StartCoroutine(CarSoundRoutine());
        }
        
        private IEnumerator GearShiftRoutine()
        {
            while (audioSource.volume >= 0.5f)
            {
                audioSource.volume -= 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
            while (audioSource.volume < 0.5f)
            {
                audioSource.volume += 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}