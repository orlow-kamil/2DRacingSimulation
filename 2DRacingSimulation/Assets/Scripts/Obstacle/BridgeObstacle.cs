using RacingSimulation.Player;
using UnityEngine;

namespace RacingSimulation.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    public class BridgeObstacle : MonoBehaviour, IObstacle
    {
        public float Force => 0f;

        [SerializeField] private bool active = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.active && collision.gameObject.TryGetComponent(out PlayerController player))
            {
                player.StartInvincibleMode();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (this.active && collision.gameObject.TryGetComponent(out PlayerController player))
            {
                player.StopInvincibleMode();
            }
        }
    }
}