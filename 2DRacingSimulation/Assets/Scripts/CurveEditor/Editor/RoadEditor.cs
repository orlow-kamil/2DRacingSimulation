using UnityEditor;
using UnityEngine;

namespace RacingSimulation.CurveEditor
{
    [CustomEditor(typeof(RoadCreator))]
    public class RoadEditor : Editor
    {
        private RoadCreator creator;
        private int roadIndex = 1;

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
         
            if (GUILayout.Button("Save new road"))
            {
                string name = $"Road_{this.roadIndex++}";
                string localPath = this.SetLocalPath(name);
                CreateAndSavePrefab(name, localPath);
            }
        }

        private string SetLocalPath(string name)
        {
            string localPath = $"Assets/Prefabs/GeneratedRoads/{name}.prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            return localPath;
        }

        private void CreateAndSavePrefab(string name, string localPath)
        {
            GameObject newRoad = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
            Mesh copiedMesh = this.creator.CloneMesh();

            newRoad.GetComponent<MeshFilter>().sharedMesh = copiedMesh;
            newRoad.GetComponent<MeshRenderer>().sharedMaterial = this.creator.GetComponent<MeshRenderer>().sharedMaterial;
            newRoad.GetComponent<MeshCollider>().sharedMesh = copiedMesh;
        }
    }
}