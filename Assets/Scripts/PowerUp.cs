using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private Material _ghostMat;
    private GameObject _player;
    public DG.Tweening.Core.TweenerCore<Quaternion, Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> turnTween;
    private bool _isGhost;
    public GameObject powerUpText;

    void Start()
    {
        _player = GameObject.Find("Player");
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
                    _isGhost = true;
                    var ghostPop = Instantiate(powerUpText, transform.position, Quaternion.identity);
                    ghostPop.transform.SetParent(_player.transform);
                    var colorG = ghostPop.GetComponent<TextMeshPro>().color;
                    var targetColorG = new Color(colorG.r, colorG.g, colorG.b, 0);
                    ghostPop.GetComponent<TextMeshPro>().DOColor(targetColorG, 1);
                    ghostPop.transform.DOMoveY(2, 1).SetRelative().OnComplete(() => {
                        Destroy(ghostPop);
                    });
                    DOTween.Kill(transform);
                    transform.GetChild(0).gameObject.SetActive(false);
                    break;
                case "Handle":
                    IncreaseHandling();
                    var handlingPop = Instantiate(powerUpText, transform.position, Quaternion.identity);
                    handlingPop.transform.SetParent(_player.transform);
                    var color = handlingPop.GetComponent<TextMeshPro>().color;
                    var targetColor = new Color(color.r, color.g, color.b, 0);
                    handlingPop.GetComponent<TextMeshPro>().DOColor(targetColor, 1);
                    handlingPop.transform.DOMoveY(2, 1).SetRelative().OnComplete(() => {
                        Destroy(handlingPop);
                    });
                    DOTween.Kill(transform);
                    Destroy(this.gameObject);   
                    break;
                case "Money":
                    var randomMoney = Random.Range(40, 61);
                    GameManager.IncreaseMoney(randomMoney);
                    var moneyPop = Instantiate(powerUpText, transform.position, Quaternion.identity);
                    moneyPop.transform.SetParent(_player.transform);
                    moneyPop.GetComponent<TextMeshPro>().text = randomMoney + "$";
                    var colorM = moneyPop.GetComponent<TextMeshPro>().color;
                    var targetColorM = new Color(colorM.r, colorM.g, colorM.b, 0);
                    moneyPop.GetComponent<TextMeshPro>().DOColor(targetColorM, 1);
                    moneyPop.transform.DOMoveY(2, 1).SetRelative().OnComplete(() => {
                        Destroy(moneyPop);
                    });
                    DOTween.Kill(transform);
                    Destroy(this.gameObject);
                    break;
            }
        }
    }

    private IEnumerator GhostRoutine()
    {
        _player.GetComponent<BoxCollider>().enabled = false;
        var playerRenderer = _player.transform.GetChild(0).GetComponent<MeshRenderer>();
        var currentMaterials = playerRenderer.materials;
        Material[] newMaterials = new Material[currentMaterials.Length];
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = _ghostMat;
        }
        playerRenderer.materials = newMaterials;
        yield return new WaitForSeconds(5);
        _player.GetComponent<BoxCollider>().enabled = true;
        playerRenderer.materials = currentMaterials;
        Destroy(gameObject);
    }

    private void IncreaseHandling()
    {
        var player = _player.GetComponent<Player>();
        player.IncreaseHandling();
    }

    private void KillIfBehind()
    {
        if (_player.transform.position.z > transform.position.z + 10 && !_isGhost)
        {
            DOTween.Kill(transform);
            Destroy(this.gameObject);
        }
    }
}