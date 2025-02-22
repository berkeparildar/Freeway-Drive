using Game;
using Scripts.Player;
using UnityEngine;

namespace CollectibleSystem
{
    public abstract class Collectible: MonoBehaviour
    {
        [SerializeField] protected PlayerManager player;

        protected void Awake()
        {
            player ??= FindObjectOfType<PlayerManager>();
        }

        protected void Update()
        {
            KillIfBehind();
        }

        private void KillIfBehind()
        {
            if (player.transform.position.z > transform.position.z + 10)
            {
                PoolManager.ReturnCollectible(this);
            }
        }
    }
}