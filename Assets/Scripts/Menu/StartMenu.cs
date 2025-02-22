using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class StartMenu : MonoBehaviour
    {   
        [SerializeField] private Material[] colors;
        [SerializeField] private MeshRenderer carRenderer;
        [SerializeField] private Animator UIAnimator;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Button[] buttons;
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int Back = Animator.StringToHash("back");

        private void Awake() {
            if (PlayerPrefs.GetInt("Money", 0) < 100)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }
            }
            else
            {
                buttons[PlayerPrefs.GetInt("Color", 0)].interactable = false;
            }
            var currentMats = carRenderer.materials;
            currentMats[0] = colors[(PlayerPrefs.GetInt("Color", 0))];
            carRenderer.materials = currentMats;
        }
        
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void SetColor(int index)
        {
            var currentColor =  PlayerPrefs.GetInt("Color", 0);
            var currentMaterials = carRenderer.materials;
            currentMaterials[0] = colors[index];
            carRenderer.materials = currentMaterials;
            PlayerPrefs.SetInt("Color", index);
            var currentMoney = PlayerPrefs.GetInt("Money", 0);
            currentMoney -= 100;
            PlayerPrefs.SetInt("Money", currentMoney);
            moneyText.text = PlayerPrefs.GetInt("Money", 0).ToString();
            buttons[currentColor].interactable = true;
            buttons[index].interactable = false;
            if (PlayerPrefs.GetInt("Money", 0) < 100)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }
            }
            PlayerPrefs.Save();
        }

        public void ShowCustomizationMenu()
        {
            UIAnimator.SetTrigger(Show);
            moneyText.text = PlayerPrefs.GetInt("Money", 0).ToString();
        }

        public void GoBackToMenu()
        {
            UIAnimator.SetTrigger(Back);
        }
    }
}
