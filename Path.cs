using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Track
{
	[Serializable]
	public class Path
	{
        [SerializeField, HideInInspector]
        private List<Vector3> _points = new List<Vector3>();
    }
}
