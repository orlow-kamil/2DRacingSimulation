using UnityEditor;
using UnityEngine;

namespace RacingSimulation.CurveEditor
{
    [CustomEditor(typeof(RoadCreator))]
    public class RoadEditor : Editor
    {
        private RoadCreator creator;

        private void OnEnable()
        {
            this.creator = (RoadCreator)target;
        }

        private void OnSceneGUI()
        {
            if (this.creator.AutoUpdate && Event.current.type == EventType.Repaint)
            {
                this.creator.UpdateRoad();
            }
        }
    }
}