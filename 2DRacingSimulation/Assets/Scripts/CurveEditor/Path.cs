using System.Collections.Generic;
using UnityEngine;

namespace RacingSimulation.CurveEditor
{
    [System.Serializable]
    public class Path
    {
        public int NumPoints => this.numPoints;
        public int NumSegment => this.numSegments;
        public Vector2 this[int index] => this.points[index];

        public bool IsClosed 
        {
            get => this.isClosed;
            set
            {
                if (this.isClosed != value)
                {
                    this.isClosed = value;

                    if (isClosed)
                    {
                        this.points.Add(this.points[this.numPoints - 1] * 2 - this.points[this.numPoints - 2]);
                        this.points.Add(this.points[0] * 2 - this.points[1]);
                    }
                    else
                    {
                        this.points.RemoveRange(this.numPoints - 2, 2);
                    }
                }
            }
        }

        [SerializeField][HideInInspector] private List<Vector2> points;
        [SerializeField][HideInInspector] private bool isClosed;

        private int numPoints => this.points.Count;
        private int numSegments => this.numPoints / 3;

        public Path(Vector2 centre)
        {
            this.points = new List<Vector2>
            {
                centre + Vector2.left,
                centre + (Vector2.left + Vector2.up) * .5f,
                centre + (Vector2.right + Vector2.down) * .5f,
                centre + Vector2.right
            };
        }

        public void AddSegment(Vector2 anchorPos)
        {
            this.points.Add(this.points[this.numPoints - 1] * 2 - this.points[this.numPoints - 2]);
            this.points.Add((this.points[this.numPoints - 1] + anchorPos) * .5f);
            this.points.Add(anchorPos);
        }

        public void DeleteSegment(int anchorIndex)
        {
            if (this.numSegments > 2 || !this.isClosed && this.numSegments > 1)
            {
                if (anchorIndex == 0)
                {
                    if (this.isClosed)
                    {
                        this.points[this.numPoints - 1] = this.points[2];
                    }
                    this.points.RemoveRange(0, 3);
                }
                else if (anchorIndex == this.numPoints - 1 && !this.isClosed)
                {
                    this.points.RemoveRange(anchorIndex - 2, 3);
                }
                else
                {
                    this.points.RemoveRange(anchorIndex - 1, 3);
                }
            }
        }

        public List<Vector2> GetPointsInSegment(int index)
        {
            var segment = new List<Vector2>
            {
                this.points[3 * index],
                this.points[3 * index + 1],
                this.points[3 * index + 2],
                this.points[this.LoopIndex(index * 3 + 3)]
            };
            return segment;
        }


        public void MovePoint(int index, Vector2 newPos)
        {
            Vector2 delta = newPos - this.points[index];
            this.points[index] = newPos;

            if (index % 3 == 0)
            {
                if (index + 1 < this.numPoints || this.isClosed)
                {
                    this.points[this.LoopIndex(index + 1)] += delta;
                }
                if (index - 1 > 0 || this.isClosed)
                {
                    this.points[this.LoopIndex(index - 1)] += delta;
                }
            }
            else
            {
                this.MoveControlPoint(index, newPos);
            }
        }

        public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
        {
            List<Vector2> evenlySpacedPoints = new List<Vector2>();
            evenlySpacedPoints.Add(this.points[0]);
            Vector2 previousPoints = this.points[0];
            float distanceSinceLatsEvenPoint = 0;

            for (int segmentIndex = 0; segmentIndex < this.numSegments; segmentIndex++)
            {
                var p = this.GetPointsInSegment(segmentIndex);
                float controlNetLeght = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
                float estimatedCurveLenght = Vector2.Distance(p[0], p[3]) + .5f * controlNetLeght;
                int divisions = Mathf.CeilToInt(estimatedCurveLenght * resolution * 10);
                float t = 0f;
                while (t <= 1f)
                {
                    t += 1f/divisions;
                    Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                    distanceSinceLatsEvenPoint += Vector2.Distance(previousPoints, pointOnCurve);

                    while (distanceSinceLatsEvenPoint >= spacing)
                    {
                        float overShootDistance = distanceSinceLatsEvenPoint - spacing;
                        Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoints - pointOnCurve).normalized * overShootDistance;
                        evenlySpacedPoints.Add(newEvenlySpacedPoint);
                        distanceSinceLatsEvenPoint = overShootDistance;
                        previousPoints = newEvenlySpacedPoint;
                    }

                    previousPoints = pointOnCurve;
                }
            }
            return evenlySpacedPoints.ToArray();
        }

        private void MoveControlPoint(int index, Vector2 newPos)
        {
            bool nextPointIsAnchor = (index + 1) % 3 == 0;
            int correspondingIndex = nextPointIsAnchor ? index + 2 : index - 2;
            int anchorIndex = nextPointIsAnchor ? index + 1 : index - 1;

            if (correspondingIndex >= 0 && correspondingIndex < this.numPoints || this.isClosed)
            {
                int currentAnchorIndex = this.LoopIndex(anchorIndex);
                int currentCorrespondingIndex = this.LoopIndex(correspondingIndex);

                float distance = (this.points[currentAnchorIndex] - this.points[currentCorrespondingIndex]).magnitude;
                Vector2 direction = (this.points[currentAnchorIndex] - newPos).normalized;

                this.points[currentCorrespondingIndex] = this.points[currentAnchorIndex] + direction * distance;
            }
        }

        private int LoopIndex(int index) => (index + this.numPoints) % this.numPoints;
    }
}