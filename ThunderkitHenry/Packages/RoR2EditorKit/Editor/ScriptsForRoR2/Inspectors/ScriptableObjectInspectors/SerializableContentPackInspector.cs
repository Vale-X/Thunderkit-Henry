using RoR2.ContentManagement;
using RoR2EditorKit.Core.Inspectors;
using UnityEditor;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(SerializableContentPack), isFallback = false)]
    public class SerializableContentPackInspector : ScriptableObjectInspector<SerializableContentPack>
    {
        protected override bool HasVisualTreeAsset => false;
        protected override void OnEnable()
        {
            base.OnEnable();

            OnRootElementsCleared += () =>
            {
                if (target is SerializableContentPack)
                {
                    RootVisualElement.Add(CreateHelpBox($"The Vanilla SerializableContentPack is no longer supported as it lacks the new fields added to ContentPacks in Survivors of the Void." +
                        $"\n\nSubclassing SerializableContentPack or using R2API's SerializableContentPack is recommended", MessageType.Warning));
                }
            };
        }

        protected override void DrawInspectorGUI()
        {
            DrawInspectorElement.Add(new IMGUIContainer(base.OnInspectorGUI));
        }
    }
}
