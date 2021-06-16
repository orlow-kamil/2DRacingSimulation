using RacingSimulation.Player;
using UnityEngine;

namespace RacingSimulation.Road
{
    [RequireComponent(typeof(Collider2D))]
    public class CheckPoint : MonoBehaviour, ICheckPoint
    {
        public bool Visited { get => this.visited; set => this.visited = value; }
        public int Index => this.transform.GetSiblingIndex();
        public Transform Transform => this.transform;

        private bool visited = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController player))
            {
                player.CurrentCheckPointIndex = this.Index;
                this.visited = true;
            }
        }
    }
}