using RoR2;
using System;
using UnityEngine;
using ThunderHenry.Modules;
using RoR2.Achievements;

namespace ThunderHenry.Achievements
{
    internal class ThunderHenryFist : UnlockableCreator.ThunderHenryAchievement
    {
        public override string Prefix => ThunderHenryPlugin.developerPrefix + Tokens.henryPrefix + "UNLOCK_";
        public override string AchievementNameToken => Prefix + "FIST_NAME";
        public override string AchievementDescToken => Prefix + "FIST_DESC";
        public override string AchievementIdentifier => Prefix + "FIST_ID";
        public override string UnlockableIdentifier => Prefix + "FIST_REWARD_ID";
        public override string PrerequisiteUnlockableIdentifier => Prefix + "SURVIVOR_ID";
        public override UnlockableDef UnlockableDef => Modules.Assets.mainAssetBundle.LoadAsset<UnlockableDef>("Skills.ThunderHenry.PrimaryAlt1");
        public override Sprite Sprite => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBoxingGlovesIcon");

        private const float requirement = 3.5f;

        public override void Initialize()
        {
            UnlockableCreator.AddUnlockable<ThunderHenryFist>(true);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += this.checkAttackSpeed;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= this.checkAttackSpeed;
        }

        private void checkAttackSpeed()
        {
            if (base.localUser != null && base.meetsBodyRequirement && base.localUser.cachedBody.attackSpeed >= requirement)
            {
                base.Grant();
            }
        }
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Prefabs.bodyPrefabs[0]);
        }
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }
        public override void OnBodyRequirementBroken()
        {
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        public override Func<string> GetHowToUnlock => () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString(AchievementNameToken),
            Language.GetString(AchievementDescToken)
        });
        public override Func<string> GetUnlocked => () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString(AchievementNameToken),
            Language.GetString(AchievementDescToken)
        });


    }
}