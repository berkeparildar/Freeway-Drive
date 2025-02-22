using System;
using CollectibleSystem;
using Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField] private int money;
        [SerializeField] private UIManager uiManager;
        
        private void Awake()
        {
            CollectibleMoney.CollectedMoney += OnCollectedMoney;
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
        }
        
        private void OnDestroy()
        {
            CollectibleMoney.CollectedMoney -= OnCollectedMoney;
        }

        private void OnPlayerCrashed()
        {
            var currentMoney = PlayerPrefs.GetInt("Money", 0);
            currentMoney += money;
            PlayerPrefs.SetInt("Money", currentMoney);
        }
        
        private void OnCollectedMoney(int money)
        {
            this.money += money;
            uiManager.UpdateMoney(money);
        }
    }
}