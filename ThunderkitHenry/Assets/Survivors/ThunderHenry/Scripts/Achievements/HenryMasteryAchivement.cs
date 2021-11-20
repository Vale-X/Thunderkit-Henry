using RoR2;
using System;
using UnityEngine;
using ThunderHenry.Modules;
using R2API;

namespace ThunderHenry.Achievements
{
    internal class ThunderHenryMastery : ModdedUnlockable
    {
        // this prefix variable is ROBVALE_THUNDERHENRY_BODY_UNLOCK_ by default.
        public string Prefix => ThunderHenryPlugin.developerPrefix + Tokens.henryPrefix + "UNLOCK_";

        // Requires Tokens created in tokens.cs, as they are displayed to the player.
        public override string AchievementNameToken => Prefix + "MASTERY_NAME";
        public override string AchievementDescToken => Prefix + "MASTERY_DESC";

        // Used for referencing and must be unique to the achievement.
        public override string AchievementIdentifier => Prefix + "MASTERY_ID";
        public override string UnlockableIdentifier => Prefix + "MASTERY_REWARD_ID";
        public override string UnlockableNameToken => Prefix + "MASTERY_UNLOCK";

        // If PrerequisiteUnlockableIdentifier matches the name of an existing AchievementIdentifier, 
        // you need to have the Achievement unlocked in order to be able to unlock this achievement.
        // In this case you need to have HenryUnlockAchievement completed in order to meet the requirements for this achivement.
        public override string PrerequisiteUnlockableIdentifier => Prefix + "SURVIVOR_ID";

        public override Sprite Sprite => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texHenryAchievement");

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
            if (run is null) { Debug.LogWarning("RunIsNull"); return; }
            if (runReport is null) { Debug.LogWarning(""); return; }

            if (!runReport.gameEnding) { Debug.LogWarning(""); return; }

            if (runReport.gameEnding.isWin)
            {
                Debug.LogWarning("isWin");
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                if (difficultyDef != null && difficultyDef.countsAsHardMode)
                {
                    Debug.LogWarning("IsHardMode");
                    if (base.meetsBodyRequirement)
                    {
                        Debug.LogWarning("BodyReqMet");
                        base.Grant();
                    }
                }
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