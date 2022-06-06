using RoR2EditorKit.Settings;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.Core.Inspectors
{
    [CustomEditor(typeof(EditorInspectorSettings))]
    internal sealed class EditorInspectorSettingsInspector : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            return StaticInspectorGUI(serializedObject);
        }

        internal static VisualElement StaticInspectorGUI(SerializedObject serializedObject, bool forSettingsWindow = false)
        {
            VisualElement container = new VisualElement();

            var propertyField = new PropertyField(serializedObject.FindProperty(nameof(EditorInspectorSettings.enableNamingConventions)));
            if (forSettingsWindow)
                propertyField.AddToClassList("thunderkit-field-input");

            container.Add(propertyField);

            SerializedProperty settings = serializedObject.FindProperty(nameof(EditorInspectorSettings.inspectorSettings));

            foreach (SerializedProperty prop in settings)
            {
                var propField = new PropertyField(prop);

                if (forSettingsWindow)
                    propField.AddToClassList("thunderkit-field-input");

                container.Add(propField);
            }
            if (forSettingsWindow)
            {
                container.AddToClassList("thunderkit-field");
                container.style.flexDirection = FlexDirection.Column;
            }

            return container;
        }
    }
}
