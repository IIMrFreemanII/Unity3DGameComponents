using UnityEngine;

namespace GameComponents.CurveEditor.Scripts
{
    public class PathCreator : MonoBehaviour
    {
        [HideInInspector]
        public Path path;

        public void CreatePath()
        {
            path = new Path(transform.position);
        }
    }
}
