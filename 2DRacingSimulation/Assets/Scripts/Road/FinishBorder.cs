using RacingSimulation.Manager;
using RacingSimulation.Player;
using UnityEngine;

namespace RacingSimulation.Road
{
    public class FinishBorder : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController _) && SceneManager.Instance.LoopCounter != 0)
            {
                SceneManager.Instance.EndLoop = true;
                SceneManager.Instance.UpdateBestTime();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController _))
            {
                SceneManager.Instance.EndLoop = false;
                SceneManager.Instance.StartTimerLoop();
            }
        }
    }
}