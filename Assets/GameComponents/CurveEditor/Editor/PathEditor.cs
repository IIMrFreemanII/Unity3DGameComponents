using GameComponents.CurveEditor.Scripts;
using UnityEditor;
using UnityEngine;

namespace GameComponents.CurveEditor.Editor
{
    [CustomEditor(typeof(PathCreator))]
    public class PathEditor : UnityEditor.Editor
    {
        private PathCreator creator;
        private Path path;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Create new"))
            {
                Undo.RecordObject(creator, "Create new");
                creator.CreatePath();
                path = creator.path;
                SceneView.RepaintAll();
            }
            
            if (GUILayout.Button("Toggle closed"))
            {
                Undo.RecordObject(creator, "Toggle closed");
                path.ToggleClosed();
                SceneView.RepaintAll();
            }

            bool autoSetControlPoints = GUILayout.Toggle(path.AutoSetControlPoints, "Auto Set Control Points");
            if (autoSetControlPoints != path.AutoSetControlPoints)
            {
                Undo.RecordObject(creator, "Toggle auto set controls");
                path.AutoSetControlPoints = autoSetControlPoints;
            }

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI()
        {
            Input(); 
            Draw();
        }

        private void Input()
        {
            Event guiEvent = Event.current;
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                Undo.RecordObject(creator, "Add segment");
                path.AddSegment(mousePos);
            }
        }

        private void Draw()
        {
            for (int i = 0; i < path.NumSegments; i++)
            {
                Vector2[] points = path.GetPointsInSegment(i);
                
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
                
                Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
            }
            
            //----------------------------------
            
            Handles.color = Color.red;
            for (int i = 0; i < path.NumPoints; i++)
            {
                Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.CylinderHandleCap);

                if (path[i] != newPos)
                {
                    Undo.RecordObject(creator, "Move point");
                    path.MovePoint(i, newPos);
                }
            }
        }

        private void OnEnable()
        {
            creator = (PathCreator) target;
            if (creator.path == null)
            {
                creator.CreatePath();
            }
            path = creator.path;
        }
    }
}