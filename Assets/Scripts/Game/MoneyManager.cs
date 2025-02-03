using System;
using Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField] private int money;
        [SerializeField] private UIManager uiManager;

        public static UnityAction<int> CollectedMoney;

        private void Awake()
        {
            CollectedMoney += OnCollectedMoney;
            PlayerManager.PlayerCrashed += OnPlayerCrashed;
        }
        
        private void OnDestroy()
        {
            CollectedMoney -= OnCollectedMoney;
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
        }
    }
}