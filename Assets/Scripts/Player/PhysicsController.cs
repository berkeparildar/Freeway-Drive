using Scripts.Player;
using UnityEngine;

namespace Player
{
    public class PhysicsController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "TrafficCar":
                    PlayerManager.PlayerCrashed?.Invoke();
                    break;
                case "GhostPowerUp":
                    break;
            }
        }
    }
}