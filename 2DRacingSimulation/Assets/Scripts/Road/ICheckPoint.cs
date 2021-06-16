using UnityEngine;

namespace RacingSimulation.Road
{
    public interface ICheckPoint
    {
        bool Visited { get; set; }

        int Index { get; }

        Transform Transform { get; }
    }
}