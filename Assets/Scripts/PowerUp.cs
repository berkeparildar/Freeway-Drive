using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private Material _ghostMat;
    private GameObject _player;
    public DG.Tweening.Core.TweenerCore<Quaternion, Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> turnTween;
    private bool _isGhost;

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
                    DOTween.Kill(transform);
                    transform.GetChild(0).gameObject.SetActive(false);
                    break;
                case "Handle":
                    IncreaseHandling();
                    DOTween.Kill(transform);
                    Destroy(this.gameObject);   
                    break;
                case "Money":
                    var randomMoney = Random.Range(40, 61);
                    GameManager.IncreaseMoney(randomMoney);
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