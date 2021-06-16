using UnityEngine;
using UnityEditor;

namespace RacingSimulation.CurveEditor
{
    [CustomEditor(typeof(PathCreator))]
    public class PathEditor : Editor
    {
        public Path Path => this.creator.Path;
        
        private PathCreator creator;


        private void OnEnable()
        {
            this.creator = (PathCreator)target;
            if (this.creator.Path == null)
            {
                this.creator.CreatePath();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Reset"))
            {
                Undo.RecordObject(this.creator, "Reset");
                this.creator.CreatePath();
            }

            bool isClosed = GUILayout.Toggle(this.Path.IsClosed, "Closed");
            if (isClosed != this.Path.IsClosed)
            {
                Undo.RecordObject(this.creator, "Toggle closed");
                this.Path.IsClosed = isClosed;
            }
             
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI()
        {
            this.Input();
            this.Draw();
            
        }

        private void Input()
        {
            Event guiEvent = Event.current;
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            this.InputAddSegment(guiEvent, mousePos);
            this.InputRemoveSegment(guiEvent, mousePos);

            HandleUtility.AddDefaultControl(0);
        }

        private void InputAddSegment(Event guiEvent, Vector2 mousePos)
        {
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                if (!this.Path.IsClosed)
                {
                    Undo.RecordObject(creator, "Add segment");
                    this.Path.AddSegment(mousePos);
                }
            }
        }

        private void InputRemoveSegment(Event guiEvent, Vector2 mousePos)
        {
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                float minDisctanceToAnchor = this.creator.AnchorDiameter * .5f;
                int closestAnchorIndex = -1;
                for (int i = 0; i < this.Path.NumPoints; i += 3)
                {
                    float distance = Vector2.Distance(mousePos, this.Path[i]);
                    if (distance < minDisctanceToAnchor)
                    {
                        minDisctanceToAnchor = distance;
                        closestAnchorIndex = i;
                    }
                }
                if (closestAnchorIndex != -1)
                {
                    Undo.RecordObject(creator, "Remove segment");
                    this.Path.DeleteSegment(closestAnchorIndex);
                }
            }
        }

        private void Draw()
        {
            for (int i = 0; i < this.Path.NumSegment; i++)
            {
                var points = this.Path.GetPointsInSegment(i);
                if (this.creator.DisplayControlPoints)
                {
                    Handles.color = Color.blue;
                    Handles.DrawLine(points[0], points[1]);
                    Handles.DrawLine(points[2], points[3]);
                }
                Color segmentColor = this.creator.SegmentColor;
                Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColor, null, 2f);
            }

            for (int i = 0; i < this.Path.NumPoints; i++)
            {
                if (i % 3 == 0 || this.creator.DisplayControlPoints)
                {
                    Handles.color = (i % 3 == 0) ? this.creator.AnchorColor : this.creator.ControlColor;
                    float handleSize = (i % 3 == 0) ? this.creator.AnchorDiameter : this.creator.ControlDiameter;
                    Vector2 newPos = Handles.FreeMoveHandle(this.Path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                    if (this.Path[i] != newPos)
                    {
                        Undo.RecordObject(creator, "Move point");
                        this.Path.MovePoint(i, newPos);
                    }
                }
            }
        }
    }
}