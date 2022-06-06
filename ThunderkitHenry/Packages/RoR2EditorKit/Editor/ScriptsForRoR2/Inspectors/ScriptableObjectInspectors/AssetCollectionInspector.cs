using RoR2;
using RoR2EditorKit.Core.Inspectors;
using RoR2EditorKit.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(AssetCollection))]
    public class AssetCollectionInspector : ScriptableObjectInspector<AssetCollection>
    {
        VisualElement inspectorDataContainer;
        ListViewHelper helper;

        protected override void OnEnable()
        {
            base.OnEnable();
            OnVisualTreeCopy += () =>
            {
                var container = DrawInspectorElement.Q<VisualElement>("Container");
                inspectorDataContainer = container.Q<VisualElement>("InspectorDataContainer");
            };
        }
        protected override void DrawInspectorGUI()
        {
            var assets = inspectorDataContainer.Q<ListView>("assets");
            var arraySize = inspectorDataContainer.Q<IntegerField>("arraySize");
            var data = new ListViewHelper.ListViewHelperData(
                serializedObject.FindProperty("assets"),
                assets,
                arraySize,
                () => new ObjectField(),
                BindElement);

            helper = new ListViewHelper(data);
        }

        private void BindElement(VisualElement arg1, SerializedProperty arg2)
        {
            ObjectField objField = arg1 as ObjectField;
            objField.SetObjectType<UnityEngine.Object>();
            objField.label = ObjectNames.NicifyVariableName(objField.name);
            objField.bindingPath = arg2.propertyPath;
        }
    }
}