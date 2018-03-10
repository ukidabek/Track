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
        public List<Transform> TrackPoints { get { return _trackPoints; } }

        private void Reset() 
        {
            _pathMetaData = new PathMetaData(this.transform);
        }
    }
}
