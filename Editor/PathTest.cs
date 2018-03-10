using UnityEngine;

using NUnit.Framework;

namespace Track
{
    public class PathTest
    {
        private PathMetaData CreatePath(Vector3 position)
        {
            GameObject gameObject = new GameObject();

            gameObject.transform.position = position;
            PathMetaData path = new PathMetaData(gameObject.transform);

            return path;
        }

        [Test]
        public void New_Path_Initialization_Test()
        {
            PathMetaData path = CreatePath(Vector3.zero);

            Assert.AreEqual(4, path.Count);
            Assert.AreEqual(1, path.SegmentCount);
        }

        [Test]
        public void Path_Points_Initialization_Test()
        {
            PathMetaData path = CreatePath(Vector3.zero);

            Assert.AreEqual(Vector3.zero, path[0]);
            Assert.AreEqual(Vector3.forward + Vector3.right, path[1]);
            Assert.AreEqual(-Vector3.forward + Vector3.right, path[2]);
            Assert.AreEqual(Vector3.right * 2, path[3]);
        }

        [Test]
        public void Get_Path_Segment_Test()
        {
            PathMetaData path = CreatePath(Vector3.zero);
            Vector3[] segment = path.GetSegment(0);

            Assert.AreEqual(4, segment.Length);
        }

        [Test]
        public void Path_Index_Loop_Test()
        {
            PathMetaData path = CreatePath(Vector3.zero);
            Vector3 point = path[0];
            Vector3 point1 = path[4];

            Assert.AreEqual(point, point1);

            Vector3 [] segment =  path.GetSegment(0);
            Vector3 [] segment1 = path.GetSegment(1);

            for (int i = 0; i < PathMetaData.PATH_SEGMENT_COUNT; i++)
            {
                Assert.AreEqual(segment[i], segment1[i]);
            }
        }

        [Test]
        public void Path_Point_Position_Set_Test()
        {
            PathMetaData path = CreatePath(Vector3.one);

            Vector3 position = new Vector3(200, 200, 200);

            path[3] = position;

            Assert.AreEqual(position, path[3]);
        }

        [Test]
        public void Path_Distance_Calculation_Test()
        {
            PathMetaData path = CreatePath(Vector3.one);

            path[0] = path[1] = Vector3.zero;
            path[2] = path[3] = Vector3.right;
            
            float distance = path.CalculateSegmentDistance(0);

            Assert.AreEqual(1.5f, distance);
        }

    }
}