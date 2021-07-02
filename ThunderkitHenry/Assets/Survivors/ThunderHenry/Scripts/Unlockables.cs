using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Achievements;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using ThunderHenry.Achievements;

namespace ThunderHenry.Modules
{
    internal static class Unlockables
    {
        private static readonly HashSet<string> usedRewardIds = new HashSet<string>();
        internal static List<AchievementDef> achievementDefs = new List<AchievementDef>();
        internal static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();
        private static readonly List<(AchievementDef achievementDef, UnlockableDef unlockableDef, string unlockableName)> moddedUnlocks = new List<(AchievementDef achievementDef, UnlockableDef unlockableDef, string unlockableName)>();
        private static List<SerializableAchievement> conditions = new List<SerializableAchievement>();
        
        private static bool addingUnlockables;
        public static bool ableToAdd { get; private set; } = false;
        internal static void Init()
        {
            foreach(AchievementUnlockable def in Assets.mainContentPack.unlockableDefs)
            {
                AddUnlock(def);
            }
        }

        internal static AchievementUnlockable AddUnlock(AchievementUnlockable instance)
        {
            instance.Initialize();

            string unlockableIdentifier = instance.UnlockableIdentifier;

            if (!usedRewardIds.Add(unlockableIdentifier))
            {
                throw new InvalidOperationException($"The unlockable identifier '{unlockableIdentifier}' is already used by another mod or used by the base game.");
            }

            unlockableDefs.Add(instance);
            achievementDefs.Add(instance.achievementDef);
            moddedUnlocks.Add((instance.achievementDef, instance, instance.UnlockableIdentifier));

            if (!addingUnlockables)
            {
                addingUnlockables = true;
                IL.RoR2.AchievementManager.CollectAchievementDefs += BCollectAchievementDefs;
                IL.RoR2.UnlockableCatalog.Init += BInit_Il;
            }

            return instance;
        }

        internal static UnlockableDef AddAchievementUnlockable<TUnlockable>() where TUnlockable : AchievementUnlockable, new()
        {
            TUnlockable instance = new TUnlockable();

            instance.Initialize();

            string unlockableIdentifier = instance.UnlockableIdentifier;

            if (!usedRewardIds.Add(unlockableIdentifier))
            {
                throw new InvalidOperationException($"The unlockable identifier '{unlockableIdentifier}' is already used by another mod or used by the base game.");
            }

            unlockableDefs.Add(instance);
            achievementDefs.Add(instance.achievementDef);
            moddedUnlocks.Add((instance.achievementDef, instance, instance.UnlockableIdentifier));

            if (!addingUnlockables)
            {
                addingUnlockables = true;
                IL.RoR2.AchievementManager.CollectAchievementDefs += BCollectAchievementDefs;
                IL.RoR2.UnlockableCatalog.Init += BInit_Il;
            }

            return instance;
        }

        public static ILCursor BCallDel_<TDelegate>(this ILCursor cursor, TDelegate target, out Int32 index) where TDelegate : Delegate
        {
            index = cursor.EmitDelegate<TDelegate>(target);
            return cursor;
        }
        public static ILCursor BCallDel_<TDelegate>(this ILCursor cursor, TDelegate target)
            where TDelegate : Delegate => cursor.BCallDel_(target, out _);

        private static void BInit_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(typeof(UnlockableCatalog), nameof(UnlockableCatalog.SetUnlockableDefs)))
            .BCallDel_(Modules.ArrayHelper.AppendDel(unlockableDefs));

        private static void BCollectAchievementDefs(ILContext il)
        {
            var f1 = typeof(AchievementManager).GetField("achievementIdentifiers", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            if (f1 is null) throw new NullReferenceException($"Could not find field in {nameof(AchievementManager)}");
            var cursor = new ILCursor(il);
            _ = cursor.GotoNext(MoveType.After,
                x => x.MatchEndfinally(),
                x => x.MatchLdloc(1)
            );

            void EmittedDelegate(List<AchievementDef> list, Dictionary<String, AchievementDef> map, List<String> identifiers)
            {
                ableToAdd = false;
                for (Int32 i = 0; i < moddedUnlocks.Count; ++i)
                {
                    var (ach, unl, unstr) = moddedUnlocks[i];
                    if (ach is null) continue;
                    identifiers.Add(ach.identifier);
                    list.Add(ach);
                    map.Add(ach.identifier, ach);
                }
            }

            _ = cursor.Emit(OpCodes.Ldarg_0);
            _ = cursor.Emit(OpCodes.Ldsfld, f1);
            _ = cursor.EmitDelegate<Action<List<AchievementDef>, Dictionary<String, AchievementDef>, List<String>>>(EmittedDelegate);
            _ = cursor.Emit(OpCodes.Ldloc_1);
        }
    }

    /*internal static class UnlockableCreator
    {
        private static readonly HashSet<string> usedRewardIds = new HashSet<string>();
        internal static List<AchievementDef> achievementdefs = new List<AchievementDef>();
        internal static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();
        private static readonly List<(AchievementDef achievementDef, UnlockableDef unlockableDef, string unlockableName)> moddedUnlocks = new List<(AchievementDef achievementDef, UnlockableDef unlockableDef, string unlockableName)>();

        private static bool addingUnlockables;

        public static bool ableToAdd { get; private set; } = false;

        internal static UnlockableDef CreateNewUnlockable(UnlockableInfo unlockableInfo, UnlockableDef unlockableDef)
        {
            var finishedUnlockable = unlockableDef;
            finishedUnlockable.cachedName = unlockableDef.nameToken;
            finishedUnlockable.getHowToUnlockString = unlockableInfo.HowToUnlockString;
            finishedUnlockable.getUnlockedString = unlockableInfo.UnlockedString;
            finishedUnlockable.sortScore = unlockableInfo.SortScore;

            return finishedUnlockable;
        }
        public static UnlockableDef AddUnlockable<TUnlockable>(bool serverTracked) where TUnlockable : BaseAchievement, IModdedUnlockableDataProvider, new()
        {
            TUnlockable instance = new TUnlockable();

            string unlockableIdentifier = instance.UnlockableIdentifier;

            if (!usedRewardIds.Add(unlockableIdentifier))
            {
                throw new InvalidOperationException($"The unlockable identifier '{unlockableIdentifier}' is already used by another mod or used by the base game.");
            }


            UnlockableInfo unlockableInfo = new UnlockableInfo
            {
                HowToUnlockString = instance.GetHowToUnlock,
                UnlockedString = instance.GetUnlocked,
                SortScore = 200,
            };

            UnlockableDef finishedUnlockable = CreateNewUnlockable(unlockableInfo, instance.UnlockableDef);
            AchievementDef achievementDef = new AchievementDef
            {
                identifier = instance.AchievementIdentifier,
                unlockableRewardIdentifier = finishedUnlockable.nameToken,
                prerequisiteAchievementIdentifier = instance.PrerequisiteUnlockableIdentifier,
                nameToken = instance.AchievementNameToken,
                descriptionToken = instance.AchievementDescToken,
                achievedIcon = instance.Sprite,
                type = instance.GetType(),
                serverTrackerType = (serverTracked ? instance.GetType() : null)
            };

            unlockableDefs.Add(finishedUnlockable);
            achievementdefs.Add(achievementDef);

            moddedUnlocks.Add((achievementDef, finishedUnlockable, instance.UnlockableIdentifier));

            if (!addingUnlockables)
            {
                addingUnlockables = true;
                IL.RoR2.AchievementManager.CollectAchievementDefs += CollectAchievementDefs;
                IL.RoR2.UnlockableCatalog.Init += Init_Il;
            }
            return finishedUnlockable;
        }
        internal struct UnlockableInfo
        {
            internal Func<string> HowToUnlockString;
            internal Func<string> UnlockedString;
            internal int SortScore;
        }
        internal interface IModdedUnlockableDataProvider
        {
            string AchievementIdentifier { get; }
            string UnlockableIdentifier { get; }
            string AchievementNameToken { get; }
            string PrerequisiteUnlockableIdentifier { get; }
            UnlockableDef UnlockableDef { get; }
            string AchievementDescToken { get; }
            Sprite Sprite { get; }
            Func<string> GetHowToUnlock { get; }
            Func<string> GetUnlocked { get; }
        }

        public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target, out Int32 index)
where TDelegate : Delegate
        {
            index = cursor.EmitDelegate<TDelegate>(target);
            return cursor;
        }
        public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target)
            where TDelegate : Delegate => cursor.CallDel_(target, out _);

        private static void Init_Il(ILContext il) => new ILCursor(il)
    .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(typeof(UnlockableCatalog), nameof(UnlockableCatalog.SetUnlockableDefs)))
    .CallDel_(Modules.ArrayHelper.AppendDel(unlockableDefs));

        private static void CollectAchievementDefs(ILContext il)
        {
            var f1 = typeof(AchievementManager).GetField("achievementIdentifiers", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            if (f1 is null) throw new NullReferenceException($"Could not find field in {nameof(AchievementManager)}");
            var cursor = new ILCursor(il);
            _ = cursor.GotoNext(MoveType.After,
                x => x.MatchEndfinally(),
                x => x.MatchLdloc(1)
            );

            void EmittedDelegate(List<AchievementDef> list, Dictionary<String, AchievementDef> map, List<String> identifiers)
            {
                ableToAdd = false;
                for (Int32 i = 0; i < moddedUnlocks.Count; ++i)
                {
                    var (ach, unl, unstr) = moddedUnlocks[i];
                    if (ach is null) continue;
                    identifiers.Add(ach.identifier);
                    list.Add(ach);
                    map.Add(ach.identifier, ach);
                }
            }

            _ = cursor.Emit(OpCodes.Ldarg_0);
            _ = cursor.Emit(OpCodes.Ldsfld, f1);
            _ = cursor.EmitDelegate<Action<List<AchievementDef>, Dictionary<String, AchievementDef>, List<String>>>(EmittedDelegate);
            _ = cursor.Emit(OpCodes.Ldloc_1);
        }
        internal abstract class ThunderHenryUnlockable : BaseAchievement, IModdedUnlockableDataProvider
        {
            #region Implementation
            public void Revoke()
            {
                if (base.userProfile.HasAchievement(this.AchievementIdentifier))
                {
                    base.userProfile.RevokeAchievement(this.AchievementIdentifier);
                }
                base.userProfile.RevokeUnlockable(UnlockableCatalog.GetUnlockableDef(this.UnlockableDef.nameToken));
            }
            #endregion

            #region Contract
            public abstract string Prefix { get; }
            public abstract string AchievementIdentifier { get; }
            public abstract string UnlockableIdentifier { get; }
            public abstract string AchievementNameToken { get; }
            public abstract string PrerequisiteUnlockableIdentifier { get; }
            public abstract UnlockableDef UnlockableDef { get; }
            public abstract string AchievementDescToken { get; }
            public abstract Sprite Sprite { get; }
            public abstract Func<string> GetHowToUnlock { get; }
            public abstract Func<string> GetUnlocked { get; }
            #endregion

            #region Virtuals
            public override void OnGranted() => base.OnGranted();
            public override void OnInstall()
            {
                base.OnInstall();
            }
            public override void OnUninstall()
            {
                base.OnUninstall();
            }
            public override Single ProgressForAchievement() => base.ProgressForAchievement();
            public override BodyIndex LookUpRequiredBodyIndex()
            {
                return base.LookUpRequiredBodyIndex();
            }
            public override void OnBodyRequirementBroken() => base.OnBodyRequirementBroken();
            public override void OnBodyRequirementMet() => base.OnBodyRequirementMet();

            public virtual void Initialize() { }

            public override bool wantsBodyCallbacks { get => base.wantsBodyCallbacks; }
            #endregion
        }
    }*/
}
