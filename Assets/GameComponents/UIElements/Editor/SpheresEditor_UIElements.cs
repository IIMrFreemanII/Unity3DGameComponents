using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameComponents.UIElements.Editor
{
    [CustomEditor(typeof(Sphere))]
    public class SpheresEditor_UIElements : UnityEditor.Editor
    {
        private Sphere _sphere;
        
        private VisualElement _rootElement;
        private VisualTreeAsset _visualTree;
        
        private List<UnityEditor.Editor> objectPreviewEditors;
        
        public void OnEnable()
        {
            _sphere = (Sphere) target;
            
            _rootElement = new VisualElement();
            _visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GameComponents/UIElements/UI/sphere.uxml");

            StyleSheet styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GameComponents/UIElements/UI/sphere.uss");
            _rootElement.styleSheets.Add(styleSheet);
        }

        public override VisualElement CreateInspectorGUI()
        {
            _rootElement.Clear();

            _visualTree.CloneTree(_rootElement);

            UQueryBuilder<VisualElement> builder = _rootElement.Query(classes: new string[] {"sphere-btn"});
            builder.ForEach(AddButtonIcon);

            return _rootElement;
        }

        public void AddButtonIcon(VisualElement button)
        {
            IMGUIContainer icon = new IMGUIContainer(() =>
            {
                string path = $"Assets/GameComponents/UIElements/{button.name}.prefab";

                GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                UnityEditor.Editor editor = GetPreviewEditor(asset);
                editor.OnPreviewGUI(GUILayoutUtility.GetRect(90, 90), null);
            });
            
            button.hierarchy.Add(icon);
            
            // button.RegisterCallback<MouseDownEvent>(e =>
            // {
            //     Debug.Log("Click");
            // });
        }
        
        private UnityEditor.Editor GetPreviewEditor(GameObject asset)
        {
            if (objectPreviewEditors == null)
            {
                objectPreviewEditors = new List<UnityEditor.Editor>();
            }

            //Check if there's already a preview
            foreach(UnityEditor.Editor editor in objectPreviewEditors)
            {
                if((GameObject)editor.target == asset)
                {
                    return editor;
                }
            }

            UnityEditor.Editor newEditor = CreateEditor(asset);
            objectPreviewEditors.Add(newEditor);
            return newEditor;
        }

        private void OnDisable()
        {
            //Cleanup the previews
            if (objectPreviewEditors != null)
            {
                foreach(UnityEditor.Editor e in objectPreviewEditors) {
                    DestroyImmediate(e);
                }
            }
        }
    }
}
