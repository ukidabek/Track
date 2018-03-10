using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Track
{
	[Serializable]
	public class PathMetaData
	{
        public const int PATH_SEGMENT_COUNT = 4;

        [SerializeField]
        private Transform _trackTransform = null;
        
        [SerializeField]
        private List<Vector3> _points = new List<Vector3>();

        [SerializeField]
        private int _resolution = 5;
        public int Resolution { get { return _resolution; } }

        public int TotalPointsCount{ get { return _resolution * SegmentCount; } }
        public PathMetaData()
        {
            _points.Add(Vector3.zero);
            _points.Add(Vector3.forward + Vector3.right);
            _points.Add(-Vector3.forward + Vector3.right);
            _points.Add(Vector3.right * 2);
        }

        public PathMetaData(Transform trackTransform) : this()
        {
            _trackTransform = trackTransform;
        }

        public Vector3 this[int index]
        {
            get { return _trackTransform.TransformPoint(_points[LoopIndex(index)]); }
            set { _points[LoopIndex(index)] = _trackTransform.InverseTransformPoint(value); }
        }

        public int Count { get { return _points.Count; } }
        public int SegmentCount {get { return _points.Count / (PATH_SEGMENT_COUNT - 1); } }


        public int GetSegmentStartIndex(int index)
        {
            return index == 0 ? index : index * (PATH_SEGMENT_COUNT - 1);
        }

        public Vector3[] GetSegment(int index)
        {
            int startIndex = GetSegmentStartIndex(LoopSegmentIndex(index));
            List<Vector3> segmentPoints = new List<Vector3>();
            for (int i = 0; i < PATH_SEGMENT_COUNT; i++)
            {
                segmentPoints.Add(this[startIndex + i]);
            }
            return segmentPoints.ToArray();
        }

        private int LoopIndex(int index)
        {
            return (index + Count) % Count;
        }

        private int LoopSegmentIndex(int index)
        {
            return (index + SegmentCount) % SegmentCount;
        }

        public void AddSegment(Vector3 newPoint)
        {
            _points.Add((_points[_points.Count - 1] * 2) - _points[_points.Count - 2]);
            _points.Add((_points[_points.Count - 1] + newPoint) * .5f);
            _points.Add(newPoint);
        }

        public void AddSegment()
        {
            Vector3 startPoint = _points[_points.Count - 1];
            AddSegment(startPoint + (Vector3.right * 2));
        }

        public float CalculateSegmentDistance(int index)
        {
            Vector3[] segment = GetSegment(index);

            float distance = 0f;
            for (int i = 1; i < segment.Length; i++)
            {
                distance += Vector3.Distance(segment[i - 1], segment[i]);
            }
            distance /= 2;

            return distance + Vector3.Distance(segment[0], segment[segment.Length - 1]);
        }

        private Vector3 EvaluateQuadratic(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 p1 = Vector3.Lerp(a, b, t);
            Vector3 p2 = Vector3.Lerp(b, c, t);

            return Vector3.Lerp(p1, p2, t);
        }

        private Vector3 EvaluateCubic(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            Vector3 p1 = EvaluateQuadratic(a, b, c, t);
            Vector3 p2 = EvaluateQuadratic(b, c, d, t);

            return Vector3.Lerp(p1, p2, t);
        }

        public Vector3 EvaluateSegment(int index, float t)
        {
            Vector3[] segment = GetSegment(index);
            return _trackTransform.TransformPoint(EvaluateCubic(segment[0], segment[1], segment[2], segment[3], t));
        }

    }
}
