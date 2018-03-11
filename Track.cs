using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace Track
{
	public class Track : MonoBehaviour 
	{
        [SerializeField]
        private PathMetaData _pathMetaData = null;
        public PathMetaData PathMetaData { get { return _pathMetaData; } }

        [SerializeField]
        List<Transform> _trackPoints = new List<Transform>();

        private void Reset() 
        {
            _pathMetaData = new PathMetaData(this.transform);
        }

        private void OnDrawGizmos()
        {
            for (int i = 1; i < _trackPoints.Count; i++)
            {
                Debug.DrawLine(_trackPoints[i - 1].position, _trackPoints[i].position, Color.red);
            }
        }

        public int Count { get { return _trackPoints.Count; } }

        public Transform this[int index]
        {
            get { return _trackPoints[LoopIndex(index)]; }
            set { _trackPoints[LoopIndex(index)] = value; }
        }

        public void Add(Transform transform)
        {
            _trackPoints.Add(transform);
        }
        public void Clear()
        {
            _trackPoints.Clear();
        }

        private int LoopIndex(int index)
        {
            return (index + Count) % Count;
        }
    }
}
