using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameComponents.UIElements
{
    public class SphereBuilder
    {
        // private Sphere m_Sphere;
        // private SerializedObject m_SerializedObject;
        //
        // private Dictionary<string, Editor> m_ModuleEditors;
        //
        // public SphereBuilder(Editor editor)
        // {
        //     m_Sphere = editor.target as Sphere;
        //     m_SerializedObject = editor.serializedObject;
        //     m_ModuleEditors = new Dictionary<string, Editor>();
        // }
        //
        // public void ResetModulePreviewEditor(string moduleName)
        // {
        //     DeletePreviewObject(moduleName);
        //
        //     var previewObj = GetPreviewGameObject(moduleName);
        //     ApplyMaterialToObject(previewObj, m_Turret.Material);
        //
        //     if (!m_ModuleEditors.ContainsKey(moduleName))
        //         m_ModuleEditors.Add(moduleName, null); 
        //
        //     var editor = m_ModuleEditors[moduleName];
        //     if (editor != null)
        //         Editor.DestroyImmediate(editor);
        //
        //     m_ModuleEditors[moduleName] = Editor.CreateEditor(previewObj);
        // }
        //
        // private void DeletePreviewObject(string moduleName)
        // {
        //     var previewObjectProperty = m_SerializedObject.FindProperty("MaterialPreview" + moduleName);
        //     var previewObj = previewObjectProperty.objectReferenceValue as GameObject;
        //     if (previewObj == null)
        //         return;
        //
        //     Object.DestroyImmediate(previewObj);
        //     previewObjectProperty.objectReferenceValue = null;
        //     m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
        // }
        //
        // private GameObject GetPreviewGameObject(string moduleName)
        // {
        //     var objectProperty = m_SerializedObject.FindProperty(moduleName);
        //     var obj = objectProperty.objectReferenceValue as GameObject;
        //     if (obj == null)
        //     {
        //         // CycleModules(moduleName, 0);
        //         obj = objectProperty.objectReferenceValue as GameObject;
        //     }
        //
        //     var previewObjectProperty = m_SerializedObject.FindProperty("MaterialPreview" + moduleName);
        //     var previewObj = previewObjectProperty.objectReferenceValue as GameObject;
        //     if (previewObj == null)
        //     {
        //         previewObj = GameObject.Instantiate(obj) as GameObject;
        //         previewObj.name = "MODULE_PREVIEW_OBJ";
        //         previewObj.hideFlags = HideFlags.HideAndDontSave;
        //         previewObj.transform.Translate(new Vector3(1000, 0, 0));
        //         previewObjectProperty.objectReferenceValue = previewObj;
        //         m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
        //     }
        //
        //     return previewObj;
        // }
    }
}