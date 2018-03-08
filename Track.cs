using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace Track
{
	public class Track : MonoBehaviour 
	{
        [SerializeField, HideInInspector]
        private Path _path = new Path();
    }
}
