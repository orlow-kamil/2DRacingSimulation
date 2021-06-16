using RacingSimulation.Manager;
using RacingSimulation.Player;
using UnityEngine;

namespace RacingSimulation.Road
{
    public class FinishBorder : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController _))
            {
                if (SceneManager.Instance.IfVisitedAllCheckPoints())
                {
                    SceneManager.Instance.UpdateBestTime();
                    SceneManager.Instance.EndLoop = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController _))
            {
                SceneManager.Instance.StartTimerLoop();
                SceneManager.Instance.ResetCheckPoints();
            }
        }
    }
}