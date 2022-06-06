using RoR2EditorKit.Settings;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.Core.Inspectors
{
    //This is also fucking stupid
    [CustomEditor(typeof(MaterialEditorSettings))]
    internal sealed class MaterialEditorSettingsInspector : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            return StaticInspectorGUI(serializedObject);
        }

        internal static VisualElement StaticInspectorGUI(SerializedObject serializedObject, bool forSettingsWindow = false)
        {
            VisualElement container = new VisualElement();

            var propertyField = new PropertyField(serializedObject.FindProperty(nameof(MaterialEditorSettings.EnableMaterialEditor)));
            if (forSettingsWindow)
                propertyField.AddToClassList("thunderkit-field-input");

            container.Add(propertyField);

            SerializedProperty settings = serializedObject.FindProperty(nameof(MaterialEditorSettings.shaderStringPairs));

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
