using RoR2;
using System;
using UnityEngine;
using ThunderHenry.Modules;
using RoR2.Networking;

namespace ThunderHenry.Achievements
{
   /* internal class ThunderHenryUnlock : UnlockableCreator.ThunderHenryUnlockable
    {
        public override string Prefix => ThunderHenryPlugin.developerPrefix + "_THUNDERHENRY_BODY_UNLOCK_";
        public override string AchievementNameToken => Prefix + "ACHIEVEMENT_NAME";
        public override string AchievementDescToken => Prefix + "ACHIEVEMENT_DESC";
        public override UnlockableDef UnlockableDef => Modules.Assets.mainAssetBundle.LoadAsset<UnlockableDef>("Characters.ThunderHenry");
        public override Sprite Sprite => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texHenryAchievement");

        public override void Initialize()
        {
            UnlockableCreator.AddUnlockable<ThunderHenryUnlock>(true);
        }

        public override void OnInstall()
        {
            base.OnInstall();

            GameNetworkManager.onServerSceneChangedGlobal += StageCheck;
        }
        public override void OnUninstall()
        {
            base.OnUninstall();

            GameNetworkManager.onServerSceneChangedGlobal -= StageCheck;
        }

        private void StageCheck(string sceneName)
        {
            if (sceneName == "blackbeach" || sceneName == "blackbeach2")
            {
                base.Grant();
            }
        }

        

        public override string AchievementIdentifier => Prefix + "ACHIEVEMENT_ID";
        public override string UnlockableIdentifier => Prefix + "REWARD_ID";
        public override string PrerequisiteUnlockableIdentifier => Prefix + "PREREQ_ID";

        public override Func<string> GetHowToUnlock => () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString(AchievementNameToken),
            Language.GetString(AchievementDescToken)
        });
        public override Func<string> GetUnlocked { get; } = () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("ROBVALE_THUNDERHENRY_BODY_UNLOCK_ACHIEVEMENT_NAME"),
            Language.GetString("ROBVALE_THUNDERHENRY_BODY_UNLOCK_ACHIEVEMENT_DESC")
        });

        
    }*/
}