using RoR2;
using RoR2EditorKit.Core.Inspectors;
using RoR2EditorKit.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(CharacterModel))]
    public class CharacterModelInspector : ComponentInspector<CharacterModel>
    {
        VisualElement inspectorData;
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
            var baseRendererInfos = inspectorData.Q<PropertyField>("baseRendererInfos");
            AddSimpleContextMenu(baseRendererInfos, new ContextMenuData(
                "Auto Populate",
                AutoPopulateRenderers));

            var baseLightInfos = inspectorData.Q<PropertyField>("baseLightInfos");
            AddSimpleContextMenu(baseLightInfos, new ContextMenuData(
                "Auto Populate",
                AutoPopulateLights));
        }

        private void AutoPopulateRenderers(DropdownMenuAction act)
        {
            var root = TargetType.gameObject.GetRootObject();
            List<CharacterModel.RendererInfo> renderInfos = new List<CharacterModel.RendererInfo>();

            foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>())
            {
                if (renderer is SkinnedMeshRenderer smr)
                {
                    renderInfos.Add(new CharacterModel.RendererInfo
                    {
                        defaultMaterial = smr.sharedMaterial,
                        renderer = smr,
                    });
                }
                else if (renderer is MeshRenderer mr)
                {
                    renderInfos.Add(new CharacterModel.RendererInfo
                    {
                        defaultMaterial = mr.sharedMaterial,
                        renderer = mr
                    });
                }
            }

            TargetType.baseRendererInfos = renderInfos.ToArray();
            serializedObject.UpdateAndApply();
        }

        private void AutoPopulateLights(DropdownMenuAction act)
        {
            var root = TargetType.gameObject.GetRootObject();
            List<CharacterModel.LightInfo> lightInfos = new List<CharacterModel.LightInfo>();

            foreach (Light light in root.GetComponentsInChildren<Light>())
            {
                lightInfos.Add(new CharacterModel.LightInfo
                {
                    light = light,
                    defaultColor = light.color
                });
            }
        }
    }
}
