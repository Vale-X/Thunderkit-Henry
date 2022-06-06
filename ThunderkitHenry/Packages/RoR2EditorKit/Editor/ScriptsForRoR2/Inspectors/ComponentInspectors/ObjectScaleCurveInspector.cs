using RoR2;
using RoR2EditorKit.Core.Inspectors;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(ObjectScaleCurve))]
    public sealed class ObjectScaleCurveInspector : ComponentInspector<ObjectScaleCurve>
    {
        VisualElement inspectorDataContainer;
        Label curveLabel;

        PropertyField curveX, curveY, curveZ;
        PropertyField overallCurve;
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
            curveLabel = inspectorDataContainer.Q<Label>("curves");

            curveX = inspectorDataContainer.Q<PropertyField>("curveX");
            curveY = inspectorDataContainer.Q<PropertyField>("curveY");
            curveZ = inspectorDataContainer.Q<PropertyField>("curveZ");
            overallCurve = inspectorDataContainer.Q<PropertyField>("overallCurve");

            var toggle = inspectorDataContainer.Q<PropertyField>("useOverallCurveOnly");
            toggle.RegisterCallback<ChangeEvent<bool>>(OnToggleSet);
            OnToggleSet();
        }

        private void OnToggleSet(ChangeEvent<bool> evt = null)
        {
            bool value = evt == null ? TargetType.useOverallCurveOnly : evt.newValue;

            if (value)
            {
                curveLabel.text = $"Overall Curve";
                curveX.style.display = DisplayStyle.None;
                curveY.style.display = DisplayStyle.None;
                curveZ.style.display = DisplayStyle.None;
                overallCurve.style.display = DisplayStyle.Flex;
            }
            else
            {
                curveLabel.text = $"Curve X, Y & Z";
                curveX.style.display = DisplayStyle.Flex;
                curveY.style.display = DisplayStyle.Flex;
                curveZ.style.display = DisplayStyle.Flex;
                overallCurve.style.display = DisplayStyle.None;
            }
        }
    }
}