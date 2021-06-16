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
        private void Awake()
        {
            SceneManager.Instance.CheckPoints = FindObjectsOfType<MonoBehaviour>().OfType<ICheckPoint>().ToList();

            if (!SceneManager.Instance.CheckPoints.Any())
                throw new System.NullReferenceException($"Checkpoints not found.");
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController player))
            {
                Transform checkPoint = ChooseClosestCheckPoint(player);
                player.transform.SetPositionAndRotation(checkPoint.position, checkPoint.rotation);
                player.RestartPlayer();
                SceneManager.Instance.AddPenaltyTime();
            }
        }

        private Transform ChooseClosestCheckPoint(PlayerController player)
        {
            int currentIndex = player.CurrentCheckPointIndex;
            return SceneManager.Instance.CheckPoints.Where(x => x.Index == currentIndex).First().Transform;
        }
    }
}