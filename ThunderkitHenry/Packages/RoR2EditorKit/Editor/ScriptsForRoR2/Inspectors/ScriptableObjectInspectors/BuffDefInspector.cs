using RoR2;
using RoR2EditorKit.Core.Inspectors;
using RoR2EditorKit.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using static RoR2EditorKit.Utilities.AssetDatabaseUtils;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(RoR2.BuffDef))]
    public sealed class BuffDefInspector : ScriptableObjectInspector<BuffDef>, IObjectNameConvention
    {
        private PropertyValidator<UnityEngine.Object> eliteDefValidator;
        private PropertyValidator<UnityEngine.Object> startSfxValidator;
        private VisualElement inspectorData = null;

        public string Prefix => "bd";

        public bool UsesTokenForPrefix => false;

        protected override void OnEnable()
        {
            base.OnEnable();
            serializedObject.FindProperty(nameof(BuffDef.iconPath)).stringValue = "";
            serializedObject.ApplyModifiedProperties();

            OnVisualTreeCopy += () =>
            {
                var container = DrawInspectorElement.Q<VisualElement>("Container");
                inspectorData = container.Q<VisualElement>("InspectorDataContainer");
                eliteDefValidator = new PropertyValidator<UnityEngine.Object>(inspectorData.Q<PropertyField>("eliteDef"), DrawInspectorElement);
                startSfxValidator = new PropertyValidator<UnityEngine.Object>(inspectorData.Q<PropertyField>("startSfx"), DrawInspectorElement);
            };
        }
        protected override void DrawInspectorGUI()
        {
            var color = inspectorData.Q<PropertyField>("buffColor");
            AddSimpleContextMenu(color, new ContextMenuData(
                "Set Color to Elite Color",
                SetColor,
                statusCheck =>
                {
                    if (TargetType.eliteDef)
                        return DropdownMenuAction.Status.Normal;
                    return DropdownMenuAction.Status.Hidden;
                }));

            SetupEliteValidator(eliteDefValidator);
            eliteDefValidator.ForceValidation();

            SetupSfxValidator(startSfxValidator);
            startSfxValidator.ForceValidation();
        }

        private void SetColor(DropdownMenuAction act)
        {
            TargetType.buffColor = TargetType.eliteDef.color;
        }

        private void SetupEliteValidator(PropertyValidator<UnityEngine.Object> validator)
        {
            validator.AddValidator(() =>
            {
                var ed = GetEliteDef();
                return ed && !ed.eliteEquipmentDef;
            },
            $"You've associated an EliteDef to this buff, but the EliteDef has no EquipmentDef assigned!", MessageType.Warning);

            validator.AddValidator(() =>
            {
                var ed = GetEliteDef();
                return ed && ed.eliteEquipmentDef && !ed.eliteEquipmentDef.passiveBuffDef;
            },
            "You've assigned an EliteDef to this buff, but the EliteDef's EquippmentDef has no assigned passiveBuffDef!", MessageType.Warning);

            validator.AddValidator(() =>
            {
                var ed = GetEliteDef();
                return ed && ed.eliteEquipmentDef && ed.eliteEquipmentDef.passiveBuffDef && ed.eliteEquipmentDef.passiveBuffDef != TargetType;
            }, $"You've associated an EliteDef to this buff, but the assigned EliteDef's EquippmentDef's ppassiveBuffDef is not the inspected buffDef!", MessageType.Warning);

            EliteDef GetEliteDef() => validator.ChangeEvent == null ? TargetType.eliteDef : (EliteDef)validator.ChangeEvent.newValue;
        }

        private void SetupSfxValidator(PropertyValidator<UnityEngine.Object> validator)
        {
            validator.AddValidator(() =>
            {
                var nsed = GetEventDef();
                return nsed && nsed.eventName.IsNullOrEmptyOrWhitespace();
            },
            $"You've associated a NetworkSoundEventDef to this buff, but the EventDef's eventName is Null, Empty or Whitespace!", MessageType.Warning);

            NetworkSoundEventDef GetEventDef() => validator.ChangeEvent == null ? TargetType.startSfx : (NetworkSoundEventDef)validator.ChangeEvent.newValue;
        }

        public PrefixData GetPrefixData()
        {
            return new PrefixData(() =>
            {
                var origName = TargetType.name;
                TargetType.name = Prefix + origName;
                UpdateNameOfObject(TargetType);
            });
        }
    }
}