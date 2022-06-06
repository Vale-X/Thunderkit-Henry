using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.Core.Inspectors
{
    /// <summary>
    /// Inherit from this class to make your own Scriptable Object Inspectors.
    /// </summary>
    /// <typeparam name="T">The type of scriptable object thats being inspected</typeparam>
    [InitializeOnLoad]
    public abstract class ScriptableObjectInspector<T> : ExtendedInspector<T> where T : ScriptableObject
    {
        static ScriptableObjectInspector()
        {
            finishedDefaultHeaderGUI += DrawEnableToggle;
        }
        private static void DrawEnableToggle(Editor obj)
        {
            if (obj is ScriptableObjectInspector<T> soInspector)
                soInspector.InspectorEnabled = EditorGUILayout.ToggleLeft($"Enable {ObjectNames.NicifyVariableName(soInspector.target.GetType().Name)} Inspector", soInspector.InspectorEnabled);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            InspectorEnabled = InspectorSetting.isEnabled;
            OnVisualTreeCopy += () =>
            {
                try
                {
                    var container = DrawInspectorElement.Q<VisualElement>("Container");
                    if (container != null)
                    {
                        var scriptType = container.Q<Label>("scriptType");
                        if (scriptType != null)
                        {
                            scriptType.text = serializedObject.FindProperty("m_Script").objectReferenceValue.name;
                        }
                    }
                }
                catch (Exception ex) { /*gulp*/ }
            };
        }
    }
}
