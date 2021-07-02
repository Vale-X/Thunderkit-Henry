using RoR2;
using RoR2.Achievements;
using UnityEngine;
using System;
using ThunderHenry.Modules;

namespace ThunderHenry.Achievements
{
    [CreateAssetMenu(menuName = "RoR2/AchievementUnlockable2")]
    class AchievementUnlockable : UnlockableDef
    {
        public SerializableAchievement achievementCondition;
        public string descToken;
        public string unlockedNameToken;
        public string unlockedDescToken;
        public Sprite icon;
        public int sortValue;
        public bool serverTracked;

        internal string AchievementNameToken;
        internal string AchievementIdentifier;
        internal string UnlockableIdentifier;
        internal string PrerequisiteUnlockableIdentifier;

        internal AchievementDef achievementDef;
        internal BaseAchievement baseAchievement;

        internal virtual void Initialize()
        {
            achievementCondition = new SerializableAchievement(typeof(ThunderHenry.Achievements.TestHenryAchievement));
            Debug.LogWarning("0");
            baseAchievement = InstantiateAchievement(achievementCondition.achievementType);
            Debug.LogWarning("1");
            AchievementNameToken = nameToken;
            AchievementIdentifier = AchievementNameToken + "_ID";
            UnlockableIdentifier = AchievementNameToken + "_UNLOCKID";
            PrerequisiteUnlockableIdentifier = AchievementNameToken + "_PREREQ_ID";
            Debug.LogWarning("2");
            getHowToUnlockString = GetHowToUnlock;
            Debug.LogWarning("3");
            getUnlockedString = GetUnlocked;
            Debug.LogWarning("4");
            sortScore = sortValue;
            Debug.LogWarning("5");

            Debug.LogWarning(AchievementIdentifier);
            Debug.LogWarning(nameToken);
            Debug.LogWarning(PrerequisiteUnlockableIdentifier);
            Debug.LogWarning(AchievementNameToken);
            Debug.LogWarning(descToken);
            Debug.LogWarning(icon);
            Debug.LogWarning(baseAchievement.GetType());
            Debug.LogWarning((serverTracked ? achievementDef.GetType() : null));

            achievementDef = new AchievementDef 
            {
                identifier = AchievementIdentifier,
                unlockableRewardIdentifier = nameToken,
                prerequisiteAchievementIdentifier = PrerequisiteUnlockableIdentifier, 
                nameToken = AchievementNameToken,
                descriptionToken = descToken,
                achievedIcon = icon,
                type = baseAchievement.GetType(),
                serverTrackerType = (serverTracked ? achievementDef.GetType() : null)
            };
            Debug.LogWarning("6");

            //achievementCondition.achievementDef = achievement;
        }

        private BaseAchievement InstantiateAchievement(Type achievementType)
        {
            if (achievementType != null && achievementType.IsSubclassOf(typeof(BaseAchievement)))
            {
                return Activator.CreateInstance(achievementType) as BaseAchievement;
            }
            Debug.LogFormat("Bad stateType {0}", new object[]
            {
                (achievementType == null) ? "null" : achievementType.FullName
            });
            return null;
        }

        private Func<string> GetHowToUnlock => () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString(AchievementNameToken),
            Language.GetString(descToken)
        });

        private Func<string> GetUnlocked => () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString(unlockedNameToken),
            Language.GetString(unlockedDescToken)
        });

    }
}
