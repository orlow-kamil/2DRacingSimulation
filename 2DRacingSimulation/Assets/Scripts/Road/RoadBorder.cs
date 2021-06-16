using RacingSimulation.Player;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RacingSimulation.Manager;

namespace RacingSimulation.Road
{
    [RequireComponent(typeof(Collider2D))]
    public class RoadBorder : MonoBehaviour
    {
        private List<ICheckPoint> checkpoints = new List<ICheckPoint>();

        private void Awake()
        {
            this.checkpoints = FindObjectsOfType<MonoBehaviour>().OfType<ICheckPoint>().ToList();

            if (!this.checkpoints.Any())
                throw new System.NullReferenceException($"Checkpoints not found.");
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController player))
            {
                Debug.Log($"{player.name} is out of road.");
                Transform checkPoint = ChooseClosestCheckPoint(player.transform);
                player.transform.position = checkPoint.position;
                player.RestartPlayer();
                SceneManager.Instance.AddPenaltyTime();
            }
        }

        private Transform ChooseClosestCheckPoint(Transform player)
        {
            var distances = this.GetCalculatedDistances(player);
            float minDistance = distances.Min();
            return this.checkpoints.Where(x => this.GetCalculateDistance(x, player) == minDistance).First().Transform;
        }

        private List<float> GetCalculatedDistances(Transform player)
        {
            var distances = new List<float>();
            foreach (var checkPoint in this.checkpoints)
            {
                float distance = this.GetCalculateDistance(checkPoint, player);
                distances.Add(distance);
            }
            return distances.ToList();
        }

        private float GetCalculateDistance(ICheckPoint checkPoint, Transform player) => Vector2.Distance(checkPoint.Transform.position, player.position);
    }
}