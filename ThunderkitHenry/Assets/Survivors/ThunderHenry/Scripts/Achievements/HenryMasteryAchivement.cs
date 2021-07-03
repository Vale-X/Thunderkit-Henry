using RoR2;
using System;
using UnityEngine;
using ThunderHenry.Modules;

namespace ThunderHenry.Achievements
{
    internal class ThunderHenryMastery : UnlockableCreator.ThunderHenryUnlockable
    {
        public override string Prefix => ThunderHenryPlugin.developerPrefix + "_THUNDERHENRY_BODY_UNLOCK_";
        public override string AchievementNameToken => Prefix + "MASTERY_ACHIEVEMENT_NAME";
        public override string AchievementDescToken => Prefix + "MASTERY_ACHIEVEMENT_DESC";
        public override UnlockableDef UnlockableDef => Modules.Assets.mainAssetBundle.LoadAsset<UnlockableDef>("Characters.ThunderHenry");
        public override Sprite Sprite => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texHenryAchievement");

        public override void Initialize()
        {
            UnlockableCreator.AddUnlockable<ThunderHenryMastery>(true);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            Run.onClientGameOverGlobal += RunEndHenry;
        }
        public override void OnUninstall()
        {
            base.OnUninstall();
            Run.onClientGameOverGlobal -= RunEndHenry;
        }

        private void RunEndHenry(Run run, RunReport runReport)
        {
            if (run is null) return;
            if (runReport is null) return;

            if (!runReport.gameEnding) return;

            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                if (difficultyDef != null && difficultyDef.countsAsHardMode)
                {
                    if (base.meetsBodyRequirement)
                    {
                        base.Grant();
                    }
                }
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


    }
}