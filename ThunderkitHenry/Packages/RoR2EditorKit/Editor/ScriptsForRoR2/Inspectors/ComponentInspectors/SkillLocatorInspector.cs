using RoR2;
using RoR2EditorKit.Core.Inspectors;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(SkillLocator))]
    public sealed class SkillLocatorInspector : ComponentInspector<SkillLocator>
    {
        VisualElement inspectorData;

        Dictionary<GenericSkill, PropertyField> skillToPropField = new Dictionary<GenericSkill, PropertyField>();

        protected override void OnEnable()
        {
            base.OnEnable();

            OnVisualTreeCopy += () =>
            {
                inspectorData = DrawInspectorElement.Q<VisualElement>("InspectorDataContainer");
            };
        }
        protected override void DrawInspectorGUI()
        {
            string[] fieldNames = new string[] { "primary", "secondary", "utility", "special" };
            skillToPropField.Clear();
            foreach (string field in fieldNames)
            {
                var propField = DrawInspectorElement.Q<PropertyField>(field);
                var genericSkill = serializedObject.FindProperty(propField.bindingPath).objectReferenceValue as GenericSkill;

                if (genericSkill)
                {
                    skillToPropField.Add(genericSkill, propField);
                    propField.label = genericSkill.skillName;
                    propField.tooltip = $"Type: \"{genericSkill.GetType().Name}\"" +
                        $"\n\nSkill Family: \"{genericSkill.skillFamily}\"" +
                        $"\n\nEnabled: \"{genericSkill.enabled}\"";
                    propField.RegisterCallback<ChangeEvent<GenericSkill>>(OnGenericSkillChange);
                }
            }
        }

        private void OnGenericSkillChange(ChangeEvent<GenericSkill> evt)
        {
            var genericSkill = evt.newValue;
            if (skillToPropField.TryGetValue(genericSkill, out var field))
            {
                field.label = genericSkill.skillName;
                field.tooltip = $"Type: \"{genericSkill.GetType().Name}\"" +
                    $"\n\nSkill Family: \"{genericSkill.skillFamily}\"" +
                    $"\n\nEnabled: \"{genericSkill.enabled}\"";
            }
        }
    }
}
