using UnityEngine;

namespace RacingSimulation.CurveEditor
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(PathCreator))]
    public class RoadCreator : MonoBehaviour
    {
        public bool AutoUpdate => this.autoUpdate;

        [SerializeField][Range(.05f, 1.5f)] private float spacing = 1f;
        [SerializeField] private float roadWidth = 1f;       
        [SerializeField] private float tiling = 1f;       
        [SerializeField] private bool autoUpdate;

        public void UpdateRoad()
        {
            Path path = this.GetComponent<PathCreator>().Path;
            Vector2[] points = path.CalculateEvenlySpacedPoints(this.spacing);
            this.GetComponent<MeshFilter>().mesh = this.CreateRoadMesh(points, path.IsClosed);

            int textureRepeat = Mathf.RoundToInt(this.tiling * points.Length * this.spacing * .05f);
            this.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
        }

        private Mesh CreateRoadMesh(Vector2[] points, bool isClosed)
        {
            Vector3[] verts = new Vector3[points.Length * 2];
            Vector2[] uvs = new Vector2[verts.Length];
            int numTris = 2 * (points.Length - 1) + (isClosed ? 2 : 0);
            int[] tris = new int[numTris * 3];
            int vertIndex = 0;
            int triIndex = 0;

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 forward = Vector2.zero;
                if (i < points.Length - 1 || isClosed)
                {
                    int cycleIndex = (i + 1) % points.Length;
                    forward += points[cycleIndex] - points[i];
                }
                if (i > 0 || isClosed)
                {
                    int cycleIndex = (i - 1 + points.Length) % points.Length;
                    forward += points[i] - points[cycleIndex];
                }
                forward.Normalize();
                Vector2 left = new Vector2(-forward.y, forward.x);

                verts[vertIndex] = points[i] + .5f * this.roadWidth * left;
                verts[vertIndex + 1] = points[i] - .5f * this.roadWidth * left;

                float completionPercent = i / (float)(points.Length - 1);
                float v = 1 - Mathf.Abs(2 * completionPercent - 1);
                uvs[vertIndex] = new Vector2(0, v);
                uvs[vertIndex + 1] = new Vector2(1, v);

                if (i < points.Length - 1 || isClosed)
                {
                    tris[triIndex] = vertIndex;
                    tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                    tris[triIndex + 2] = vertIndex + 1;

                    tris[triIndex + 3] = vertIndex + 1;
                    tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                    tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
                }

                vertIndex += 2;
                triIndex += 6;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;
            return mesh;
        }
    }
}