using UnityEngine;

namespace RacingSimulation.CurveEditor
{
    public class PathCreator : MonoBehaviour
    {
        public Path Path => this.path;

        public Color AnchorColor = Color.red;
        public Color ControlColor = Color.white;
        public Color SegmentColor = Color.green;
        public Color SelectedSegmentColor = Color.yellow;

        public float AnchorDiameter = .1f;
        public float ControlDiameter = .075f;
        public bool DisplayControlPoints = true;
        
        [SerializeField][HideInInspector] private Path path;

        public void CreatePath()
        {
            this.path = new Path(this.transform.position);
        }

        private void Reset()
        {
            this.CreatePath();
        }
    }
}