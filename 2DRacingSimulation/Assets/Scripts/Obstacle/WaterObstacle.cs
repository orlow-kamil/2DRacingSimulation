using RacingSimulation.Player;
using UnityEngine;

namespace RacingSimulation.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    public class WaterObstacle : MonoBehaviour, IObstacle
    {
        public float Force => this.force;

        [SerializeField] [Range(1f, 7.5f)] private float force = 2.5f;
        [SerializeField] private bool active = true;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (this.active && collision.gameObject.TryGetComponent(out PlayerController player))
            {
                if (player.InvincibleMode) return;
                Vector2 force = this.force * this.transform.up;
                player.Rgbd.AddForce(force);
            }
        }
    }
}