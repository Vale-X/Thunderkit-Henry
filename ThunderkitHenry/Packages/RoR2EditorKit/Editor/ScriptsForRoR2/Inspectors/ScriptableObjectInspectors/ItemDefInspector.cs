using RoR2;
using RoR2EditorKit.Core.Inspectors;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(ItemDef))]
    public class ItemDefInspector : ScriptableObjectInspector<ItemDef>
    {
        private VisualElement inspectorDataHolder;
        private VisualElement itemTierHolder;
        private VisualElement tokenHolder;

        private PropertyField itemTierDef;

        protected override void OnEnable()
        {
            base.OnEnable();
            OnVisualTreeCopy += () =>
            {
                var container = DrawInspectorElement.Q<VisualElement>("Container");
                inspectorDataHolder = container.Q<VisualElement>("InspectorDataContainer");
                itemTierHolder = inspectorDataHolder.Q<VisualElement>("ItemTierContainer");
                itemTierDef = itemTierHolder.Q<PropertyField>("itemTierDef");
                tokenHolder = inspectorDataHolder.Q<VisualElement>("TokenContainer");
            };
        }
        protected override void DrawInspectorGUI()
        {
            var deprecatedTierProp = serializedObject.FindProperty("deprecatedTier");
            var enumValue = (ItemTier)deprecatedTierProp.enumValueIndex;
            var deprecatedTier = itemTierHolder.Q<PropertyField>("deprecatedTier");
            deprecatedTier.RegisterCallback<ChangeEvent<string>>(OnTierEnumSet);

            itemTierDef.style.display = enumValue == ItemTier.AssignedAtRuntime ? DisplayStyle.Flex : DisplayStyle.None;

            AddSimpleContextMenu(tokenHolder, new ContextMenuData(
                "Set Tokens",
                SetTokens,
                callback =>
                {
                    var tokenPrefix = Settings.TokenPrefix;
                    if (string.IsNullOrEmpty(tokenPrefix))
                        return DropdownMenuAction.Status.Disabled;
                    return DropdownMenuAction.Status.Normal;
                }));
        }

        private void SetTokens(DropdownMenuAction action)
        {
            string tokenBase = $"{Settings.GetPrefixUppercase()}_ITEM_{serializedObject.targetObject.name.ToUpperInvariant().Replace(" ", "")}_";
            TargetType.nameToken = $"{tokenBase}NAME";
            TargetType.pickupToken = $"{tokenBase}PICKUP";
            TargetType.descriptionToken = $"{tokenBase}DESC";
            TargetType.loreToken = $"{tokenBase}LORE";
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnTierEnumSet(ChangeEvent<string> evt)
        {
            string val = evt.newValue.Replace(" ", "");
            if (Enum.TryParse(val, out ItemTier newTier))
            {
                itemTierDef.style.display = newTier == ItemTier.AssignedAtRuntime ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}