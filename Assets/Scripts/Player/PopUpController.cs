using CollectibleSystem;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PopUpController : MonoBehaviour
    {
        [SerializeField] private GameObject ghostPopUp;
        [SerializeField] private GameObject moneyPopUp;
        [SerializeField] private GameObject handlingPopUp;
        [SerializeField] private TextMeshPro moneyText;

        private void Awake()
        {
            CollectibleGhostPowerUp.CollectedGhostPowerUp += OnCollectedGhostPowerUp;
            CollectibleMoney.CollectedMoney += OnCollectedMoney;
            CollectibleHandlingPowerUp.CollectedHandlingPowerUp += OnCollectedHandlingPowerUp;
        }

        private void OnDestroy()
        {
            CollectibleGhostPowerUp.CollectedGhostPowerUp -= OnCollectedGhostPowerUp;
            CollectibleMoney.CollectedMoney -= OnCollectedMoney;
            CollectibleHandlingPowerUp.CollectedHandlingPowerUp -= OnCollectedHandlingPowerUp;
        }
        
        private void OnCollectedHandlingPowerUp(float arg0)
        {
            if (handlingPopUp.activeSelf) handlingPopUp.SetActive(false);
            handlingPopUp.SetActive(true);
        }
        
        private void OnCollectedMoney(int amount)
        {
            if (moneyPopUp.activeSelf) moneyPopUp.SetActive(false);
            moneyText.text = "+ $" + amount;
            moneyPopUp.gameObject.SetActive(true);
        }
        
        private void OnCollectedGhostPowerUp(int arg0)
        {
            if (ghostPopUp.activeSelf) ghostPopUp.SetActive(false);
            ghostPopUp.SetActive(true);
        }
    }
}