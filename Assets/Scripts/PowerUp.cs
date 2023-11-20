using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private Material ghostMat;
    [SerializeField] private GameObject player;
    public DG.Tweening.Core.TweenerCore<Quaternion, Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> turnTween;
    [SerializeField] private bool isGhost;
    [SerializeField] private GameObject powerUpText;

    void Start()
    {
        player = GameObject.Find("Player");
        turnTween = transform.DORotate(new Vector3(0, 180, 0), 2).SetRelative().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void Update() {
        KillIfBehind();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Car"))
        {
            switch (this.tag)
            {
                case "Ghost":
                    StartCoroutine(GhostRoutine());
                    isGhost = true;
                    var ghostPop = Instantiate(powerUpText, transform.position, Quaternion.identity);
                    ghostPop.transform.SetParent(player.transform);
                    var colorG = ghostPop.GetComponent<TextMeshPro>().color;
                    var targetColorG = new Color(colorG.r, colorG.g, colorG.b, 0);
                    ghostPop.GetComponent<TextMeshPro>().DOColor(targetColorG, 1);
                    ghostPop.transform.DOMoveY(2, 1).SetRelative().OnComplete(() => {
                        Destroy(ghostPop);
                    });
                    transform.GetChild(0).gameObject.SetActive(false);
                    break;
                case "Handle":
                    IncreaseHandling();
                    var handlingPop = Instantiate(powerUpText, transform.position, Quaternion.identity);
                    handlingPop.transform.SetParent(player.transform);
                    var color = handlingPop.GetComponent<TextMeshPro>().color;
                    var targetColor = new Color(color.r, color.g, color.b, 0);
                    handlingPop.GetComponent<TextMeshPro>().DOColor(targetColor, 1);
                    handlingPop.transform.DOMoveY(2, 1).SetRelative().OnComplete(() => {
                        Destroy(handlingPop);
                    });
                    gameObject.SetActive(false);
                    break;
                case "Money":
                    var randomMoney = Random.Range(40, 61);
                    GameManager.IncreaseMoney(randomMoney);
                    var moneyPop = Instantiate(powerUpText, transform.position, Quaternion.identity);
                    moneyPop.transform.SetParent(player.transform);
                    moneyPop.GetComponent<TextMeshPro>().text = randomMoney + "$";
                    var colorM = moneyPop.GetComponent<TextMeshPro>().color;
                    var targetColorM = new Color(colorM.r, colorM.g, colorM.b, 0);
                    moneyPop.GetComponent<TextMeshPro>().DOColor(targetColorM, 1);
                    moneyPop.transform.DOMoveY(2, 1).SetRelative().OnComplete(() => {
                        Destroy(moneyPop);
                    });
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

    private IEnumerator GhostRoutine()
    {
        player.GetComponent<BoxCollider>().enabled = false;
        var playerRenderer = player.transform.GetChild(0).GetComponent<MeshRenderer>();
        var currentMaterials = playerRenderer.materials;
        Material[] newMaterials = new Material[currentMaterials.Length];
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = ghostMat;
        }
        playerRenderer.materials = newMaterials;
        yield return new WaitForSeconds(5);
        player.GetComponent<BoxCollider>().enabled = true;
        playerRenderer.materials = currentMaterials;
        transform.GetChild(0).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void IncreaseHandling()
    {
        var player = this.player.GetComponent<Player>();
        player.IncreaseHandling();
    }

    private void KillIfBehind()
    {
        if (player.transform.position.z > transform.position.z + 10 && !isGhost)
        {
            gameObject.SetActive(false);
        }
    }
}