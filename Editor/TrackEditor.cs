using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using System.Collections;
using System.Collections.Generic;
using System;

namespace Track
{
	[CustomEditor(typeof(Track))]
    public class TrackEditor : Editor
    {
        private PathMetaData _path = null;
        private Transform _transform = null;

        private Track _track = null;
        private void OnEnable() 
        {
            _track = (target as Track);
            _transform = _track.transform;
            _path = _track.PathMetaData;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Add segment"))
            {
                AddSegment();
            }

            if (GUILayout.Button("Bake path"))
            {
                BakePath();
            }

            if (GUILayout.Button("Clear path"))
            {
                ClearPath();
            }
        }

        private void ClearPath()
        {
            for (int i = 0; i < _track.Count; i++)
            {
                DestroyImmediate(_track[i].gameObject);
                EditorUtility.DisplayProgressBar("Clearing path", "Deleting objects", (float)(i/ _track.Count));
            }
            _track.Clear();
            EditorUtility.ClearProgressBar();
        }

        private void AddSegment()
        {
            Undo.RecordObject(target, "Add segment");
            _path.AddSegment();
            SceneView.RepaintAll();
        }

        protected void OnSceneGUI()
        {
            for (int i = 0; i < _path.SegmentCount; i++)
            {
                Vector3[] segment = _path.GetSegment(i);
                int startIndex = _path.GetSegmentStartIndex(i);

                for (int j = 0; j < PathMetaData.PATH_SEGMENT_COUNT; j++)
                {
                    Vector3 newPosition = Vector3.zero;
                    Color oldColor = Handles.color;

                    Handles.color = j == 0 || j == PathMetaData.PATH_SEGMENT_COUNT - 1 ? Color.yellow : Handles.color;
                    newPosition = Handles.FreeMoveHandle(segment[j], _transform.rotation, .1f, Vector3.zero, Handles.SphereHandleCap);
                    MovePoint(newPosition, segment[j], startIndex, j);
                    Handles.color = oldColor;
                }

                Handles.DrawBezier(segment[0], segment[3], segment[1], segment[2], Color.green, null, 3f);
                Handles.DrawLine(segment[0], segment[1]);
                Handles.DrawLine(segment[3], segment[2]);
            }
        }

        private void MovePoint(Vector3 newPosition, Vector3 vector3, int startIndex, int v)
        {
            Vector3 delta = newPosition - vector3;
            if (newPosition != vector3)
            {
                Undo.RecordObject(target, "Path edited");
                if(v == 0 || 3 == v)
                {
                    int index = startIndex + v;
                    _path[index - 1] += index == 0 ? Vector3.zero : delta;
                    _path[index + 1] += index == _path.Count - 1 ? Vector3.zero : delta;
                }

                _path[startIndex + v] = newPosition;
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(_transform.gameObject.scene);
            }
        }

        public void BakePath()
        {
            ClearPath();

            float deltaT = 1f / _path.Resolution;
            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i < _path.SegmentCount; i++)
            {
                for (int j = 0; j < _path.Resolution; j++)
                {
                    points.Add( _path.EvaluateSegment(i, deltaT * j));
                    EditorUtility.DisplayProgressBar("Baking path", "Calculating positions", (float)(_path.TotalPointsCount / points.Count));
                }
            }

            for (int i = 0; i < points.Count; i++)
            {
                GameObject gameObject = new GameObject();
                gameObject.transform.SetParent(_track.transform);
                _track.Add(gameObject.transform);
                _track[_track.Count -1].transform.position = points[i];
                EditorUtility.DisplayProgressBar("Baking path", "Cretins positions", (float)(i / points.Count));

            }

            for (int i = 1; i < _track.Count; i++)
            {
                _track[i - 1].LookAt(_track[i].position);
                EditorUtility.DisplayProgressBar("Baking path", "Rotating positions", (float)(i / points.Count));
            }

            EditorUtility.ClearProgressBar();
        }
    }
}