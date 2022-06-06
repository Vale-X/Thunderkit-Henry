using RoR2EditorKit.Common;
using RoR2EditorKit.Core.Inspectors;
using ThunderKit.Core.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(ChildLocator))]
    public sealed class ChildLocatorInspector : ComponentInspector<ChildLocator>
    {
        private SerializedProperty nameTransformPairs;
        private VisualElement inspectorData;
        private ListViewHelper listView;
        protected override void OnEnable()
        {
            base.OnEnable();
            nameTransformPairs = serializedObject.FindProperty($"transformPairs");

            OnVisualTreeCopy += () =>
            {
                inspectorData = DrawInspectorElement.Q<VisualElement>("InspectorDataContainer");
            };
        }
        protected override void DrawInspectorGUI()
        {
            var data = new ListViewHelper.ListViewHelperData(
                nameTransformPairs,
                inspectorData.Q<ListView>("nameTransformPairs"),
                inspectorData.Q<IntegerField>("arraySize"),
                CreateCLContainer,
                BindCLContainer);
            listView = new ListViewHelper(data);
        }

        private void SetNamesToTransformNames(DropdownMenuAction act)
        {
            foreach (SerializedProperty property in nameTransformPairs)
            {
                var name = property.FindPropertyRelative("name");
                var transform = property.FindPropertyRelative("transform");

                if (transform.objectReferenceValue)
                {
                    name.stringValue = transform.objectReferenceValue.name;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        private VisualElement CreateCLContainer() => TemplateHelpers.GetTemplateInstance("ChildLocatorEntry", null, (path) =>
        {
            return path.Contains(Constants.PackageName);
        });

        private void BindCLContainer(VisualElement arg1, SerializedProperty arg2)
        {
            var field = arg1.Q<PropertyField>("name");
            field.bindingPath = arg2.FindPropertyRelative("name").propertyPath;

            field = arg1.Q<PropertyField>("transform");
            field.bindingPath = arg2.FindPropertyRelative("transform").propertyPath;
        }
    }
}
