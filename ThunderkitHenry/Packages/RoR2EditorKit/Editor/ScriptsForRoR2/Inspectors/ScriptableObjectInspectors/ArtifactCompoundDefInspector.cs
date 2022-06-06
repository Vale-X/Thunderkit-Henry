using RoR2;
using RoR2EditorKit.Core.Inspectors;
using RoR2EditorKit.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(ArtifactCompoundDef))]
    public sealed class ArtifactCompoundDefInspector : ScriptableObjectInspector<ArtifactCompoundDef>, IObjectNameConvention
    {
        public string Prefix => "acd";
        public bool UsesTokenForPrefix => false;

        private VisualElement inspectorDataHolder;

        private IMGUIContainer valueMessageContainer;

        private PropertyValidator<int> valueValidator;

        protected override void OnEnable()
        {
            base.OnEnable();
            OnVisualTreeCopy += () =>
            {
                var container = DrawInspectorElement.Q<VisualElement>("Container");
                inspectorDataHolder = container.Q<VisualElement>("InspectorDataContainer");
            };
        }

        protected override void DrawInspectorGUI()
        {
            var value = inspectorDataHolder.Q<PropertyField>("value");
            valueValidator = new PropertyValidator<int>(value, DrawInspectorElement);
            SetupValidator(valueValidator);
            valueValidator.ForceValidation();

            AddSimpleContextMenu(value, new ContextMenuData(
                "Use RNG for Value",
                dma =>
                {
                    var valueProp = serializedObject.FindProperty("value");
                    valueProp.intValue = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                    serializedObject.ApplyModifiedProperties();
                }));
        }

        private void SetupValidator(PropertyValidator<int> validator)
        {
            validator.AddValidator(() => GetValue() == 1, MessageMaker(1, "Circle"), MessageType.Error);

            validator.AddValidator(() => GetValue() == 3, MessageMaker(3, "Triangle"), MessageType.Error);

            validator.AddValidator(() => GetValue() == 5, MessageMaker(5, "Diamond"), MessageType.Error);

            validator.AddValidator(() => GetValue() == 7, MessageMaker(7, "Square"), MessageType.Error);

            validator.AddValidator(() => GetValue() == 11, MessageMaker(11, "Empty"), MessageType.Error);


            int GetValue() => validator.ChangeEvent == null ? TargetType.value : validator.ChangeEvent.newValue;
            string MessageMaker(int value, string vanillaType) => $"Compound value cannot be {value}, as that value is reserved for the {vanillaType} compound";
        }

        public PrefixData GetPrefixData()
        {
            return new PrefixData(() =>
            {
                var origName = TargetType.name;
                TargetType.name = Prefix + origName;
                AssetDatabaseUtils.UpdateNameOfObject(TargetType);
            });
        }
    }
}