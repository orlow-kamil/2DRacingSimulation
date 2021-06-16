using UnityEngine;

namespace RacingSimulation.Road
{
    public class CheckPoint : MonoBehaviour, ICheckPoint
    {
        public Transform Transform => this.transform;
    }
}