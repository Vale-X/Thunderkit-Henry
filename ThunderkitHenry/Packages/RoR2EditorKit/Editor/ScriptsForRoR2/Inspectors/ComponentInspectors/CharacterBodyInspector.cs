using RoR2;
using RoR2EditorKit.Core.Inspectors;
using RoR2EditorKit.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(CharacterBody))]
    public sealed class CharacterBodyInspector : ComponentInspector<CharacterBody>
    {
        public const string CommandoAddress = "RoR2/Base/Commando/CommandoBody.prefab";
        public const string LemurianAddress = "RoR2/Base/Lemurian/LemurianBody.prefab";
        public const string GolemAddress = "RoR2/Base/Golem/GolemBody.prefab";
        public const string BeetleQueenAddress = "RoR2/Base/Beetle/BeetleQueen2Body.prefab";
        public const string BrotherAddress = "RoR2/Base/Brother/BrotherBody.prefab";
        private VisualElement inspectorData;
        private Foldout tokenContainer;
        private Foldout spreadBloomContainer;
        private Foldout baseStatsContainer;
        private Foldout levelStatsContainer;

        protected override void OnEnable()
        {
            base.OnEnable();
            OnVisualTreeCopy += () =>
            {
                inspectorData = DrawInspectorElement.Q<VisualElement>("InspectorDataContainer");
                tokenContainer = inspectorData.Q<Foldout>("TokenContainer");
                spreadBloomContainer = inspectorData.Q<Foldout>("SpreadBloomContainer");
                baseStatsContainer = inspectorData.Q<Foldout>("BaseStatsContainer");
                levelStatsContainer = inspectorData.Q<Foldout>("LevelStatsContainer");
            };
        }

        protected override void DrawInspectorGUI()
        {
            AddSimpleContextMenu(tokenContainer, new ContextMenuData(
                "Set Tokens",
                SetTokens,
                statusCheck =>
                {
                    if (Settings.TokenPrefix.IsNullOrEmptyOrWhitespace() || !TargetType.gameObject)
                        return DropdownMenuAction.Status.Disabled;
                    return DropdownMenuAction.Status.Normal;
                }));

            var baseVision = baseStatsContainer.QContainer<PropertyField>("bVisionDistance");
            AddSimpleContextMenu(baseVision, new ContextMenuData(
                "Set To Infinity",
                x => TargetType.baseVisionDistance = float.PositiveInfinity));

            BuildContextMenu(baseStatsContainer);
        }

        private void BuildContextMenu(Foldout baseStatsContainer)
        {
            Add("Commando");
            Add("Lemurian");
            Add("Golem");
            Add("BeetleQueen");
            Add("Mithrix");

            void Add(string name)
            {
                AddSimpleContextMenu(baseStatsContainer, new ContextMenuData($"Set Base Stats To/{name}", SetBaseStats, check =>
                {
                    return AddressablesUtils.AddressableCatalogExists ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.None;
                }));
            }
        }

        private async void SetBaseStats(DropdownMenuAction obj)
        {
            string bodyName = obj.name.Substring("Set Base Stats To/".Length);
            string address = string.Empty;
            switch (bodyName)
            {
                case "Commando": address = CommandoAddress; break;
                case "Lemurian": address = LemurianAddress; break;
                case "Golem": address = GolemAddress; break;
                case "BeetleQueen": address = BeetleQueenAddress; break;
                case "Mithrix": address = BrotherAddress; break;
            }
            if (address == string.Empty)
                return;

            GameObject vanillaPrefab = await AddressablesUtils.LoadAssetFromCatalog<GameObject>(address);

            /*using (var pb = new ThunderKit.Common.Logging.ProgressBar("Copying Stats"))
            {
                var op = Addressables.LoadAssetAsync<GameObject>(address);
                while (!op.IsDone)
                {
                    await Task.Delay(100);
                    pb.Update($"Loading rpefab from address {address}, this may take a while", null, op.PercentComplete);
                }
                vanillaPrefab = op.Result;
            }*/

            if (!vanillaPrefab)
                return;

            CharacterBody vanillaBody = vanillaPrefab.GetComponent<CharacterBody>();
            if (!vanillaBody)
                return;
            TargetType.baseAcceleration = vanillaBody.baseAcceleration;
            TargetType.baseArmor = vanillaBody.baseArmor;
            TargetType.baseAttackSpeed = vanillaBody.baseAttackSpeed;
            TargetType.baseCrit = vanillaBody.baseCrit;
            TargetType.baseDamage = vanillaBody.baseDamage;
            TargetType.baseJumpCount = vanillaBody.baseJumpCount;
            TargetType.baseJumpPower = vanillaBody.baseJumpPower;
            TargetType.baseMaxHealth = vanillaBody.baseMaxHealth;
            TargetType.baseMaxShield = vanillaBody.baseMaxShield;
            TargetType.baseMoveSpeed = vanillaBody.baseMoveSpeed;
            TargetType.baseRegen = vanillaBody.baseRegen;
            TargetType.baseVisionDistance = vanillaBody.baseVisionDistance;
        }

        private void SetTokens(DropdownMenuAction act)
        {
            string gameObjectName = TargetType.gameObject.name.Replace($" ", "");
            string prefix = $"{Settings.GetPrefixUppercase()}_{gameObjectName.ToUpperInvariant()}_BODY";
            TargetType.baseNameToken = $"{prefix}_NAME";
            TargetType.subtitleNameToken = $"{prefix}_SUBTITLE";
        }

        private void OnRootMotionSet(ChangeEvent<bool> evt = null)
        {
            var rootSpeed = inspectorData.Q<FloatField>("mainRootSpeed");
            bool value = evt == null ? inspectorData.Q<Toggle>("rootMotion").value : evt.newValue;
            rootSpeed.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
