using UnityEditor;
using UnityEngine;
using RacingSimulation.ThirdParty;

namespace RacingSimulation.CurveEditor
{
    [CustomEditor(typeof(RoadCreator))]
    public class RoadEditor : Editor
    {
        private RoadCreator creator;
        private int roadIndex = 0;

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

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
         
            if (GUILayout.Button("Generate new snapshot"))
            {
                string name = $"Road_{this.roadIndex++}";
                string texturePath = this.SetLocalPath(name);

                var transparencyCapture = this.GenerateRoad(name);
                transparencyCapture.StartCoroutine(transparencyCapture.Capture(texturePath)); 
            }
        }

        private string SetLocalPath(string name)
        {
            string localPath = $"Assets/Textures/Snapshots/{name}.png";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            return localPath;
        }

        private TransparencyCaptureToFile GenerateRoad(string prefabName)
        {
            GameObject newRoad = new GameObject(prefabName, typeof(MeshFilter), typeof(MeshRenderer), typeof(TransparencyCaptureToFile));

            newRoad.GetComponent<MeshFilter>().mesh = this.creator.CloneRoadMesh();          
            newRoad.GetComponent<MeshRenderer>().material = this.creator.GetComponent<MeshRenderer>().sharedMaterial;

            return newRoad.GetComponent<TransparencyCaptureToFile>();
        }
    }
}