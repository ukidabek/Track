using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Track
{
    public class Locomotive : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5;

        [SerializeField]
        private float _angularSpeed = 120f;

        [SerializeField]
        private Track _track = null;

        [SerializeField]
        private int _currentIndex = 0;

		private float Distance
		{
            get { return Vector3.Distance(transform.position, _track[_currentIndex].position); }
        }

        private float _toNextPointDistance = 0;
		private void Awake() 
		{
            _currentIndex = 0;
            enabled = _track != null;
			if(enabled)
			{
                transform.position = _track[_currentIndex].position;
                transform.rotation = _track[_currentIndex].rotation;
            }
		}

        private void Update() 
		{
            if(Distance < 0.1f)
			{
                _currentIndex++;
				if(_currentIndex == _track.Count)
                    _currentIndex = 0;
                
                _toNextPointDistance = Vector3.Distance(transform.position, _track[_currentIndex + 1].position);
            }

            transform.position = Vector3.MoveTowards(transform.position, _track[_currentIndex].position, Time.deltaTime * _speed);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, _track[_currentIndex].rotation, Time.deltaTime * _angularSpeed);
        }
    }
}