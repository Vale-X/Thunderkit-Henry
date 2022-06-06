using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.Core.Inspectors
{
    /// <summary>
    /// Inherit from this class to make your own Component Inspectors.
    /// </summary>
    public abstract class ComponentInspector<T> : ExtendedInspector<T> where T : MonoBehaviour
    {
        private IMGUIContainer container;
        protected override void OnEnable()
        {
            base.OnEnable();
            container = new IMGUIContainer(DisplayToggle);
            OnVisualTreeCopy += AddToggle;
        }

        private void AddToggle()
        {
            if (!InspectorEnabled)
            {
                RootVisualElement.Add(container);
            }
            else
            {
                try
                {
                    var componentBase = DrawInspectorElement.Q<VisualElement>("ComponentInspectorBase");
                    var container = componentBase.Q<VisualElement>("ComponentToggleContainer");
                    var toggle = container.Q<Toggle>("InspectorToggle");
                    toggle.value = InspectorEnabled;
                    toggle.RegisterValueChangedCallback(cb => InspectorEnabled = cb.newValue);

                    var scriptType = toggle.Q<Label>("scriptType");
                    scriptType.text = serializedObject.FindProperty("m_Script").objectReferenceValue.name;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Cannot setup toggle and header for component inspector {GetType().Name}, resorting to IMGUI container\n\n{ex}");
                    RootVisualElement.Add(container);
                }
            }
        }

        private void DisplayToggle()
        {
            EditorGUILayout.BeginVertical("box");
            InspectorEnabled = EditorGUILayout.ToggleLeft($"Enable {ObjectNames.NicifyVariableName(target.GetType().Name)} Inspector", InspectorEnabled);
            EditorGUILayout.EndVertical();
        }
    }
}