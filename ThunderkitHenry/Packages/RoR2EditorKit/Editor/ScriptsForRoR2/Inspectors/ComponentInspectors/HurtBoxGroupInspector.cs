using RoR2;
using RoR2EditorKit.Core.Inspectors;
using RoR2EditorKit.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(HurtBoxGroup))]
    public class HurtBoxGroupInspector : ComponentInspector<HurtBoxGroup>
    {
        VisualElement inspectorDataContainer;

        protected override void OnEnable()
        {
            base.OnEnable();

            OnVisualTreeCopy += () =>
            {
                inspectorDataContainer = DrawInspectorElement.Q<VisualElement>("InspectorDataContainer");
            };
        }

        protected override void DrawInspectorGUI()
        {
            var label = inspectorDataContainer.Q<Label>("hurtBoxesLabel");
            AddSimpleContextMenu(label, new ContextMenuData(
                "Auto Populate Hurtboxes",
                AutoPopulateHurtboxes));
        }

        private void AutoPopulateHurtboxes(DropdownMenuAction act)
        {
            var root = TargetType.gameObject.GetRootObject();
            List<HurtBox> hurtBoxes = new List<HurtBox>();

            foreach (HurtBox hurtBox in root.GetComponentsInChildren<HurtBox>())
            {
                hurtBoxes.Add(hurtBox);
            }

            TargetType.hurtBoxes = hurtBoxes.ToArray();
            serializedObject.UpdateAndApply();
        }
    }
}