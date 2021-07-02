using System;
using System.Collections.Generic;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ThunderHenry.Modules
{
    internal static class Helpers
    {
        internal const string agilePrefix = "<style=cIsUtility>Agile.</style> ";

        // Add any extra debug methods you wanna do here. 
        public static void AwakeDebug()
        {
            PrintEntityStatesForConfigNames();
            ItemDisplayTestDebug(0);
        }

        // Entity State Configurations require very specific 'Assembly Qualified Names' in order to work correctly.
        // This should print you the name that you need to put into the Entity State Configuration.
        // replace `ThunderHenry.SkillStates.SlashCombo` with a skillstate in your mod. 
        private static void PrintEntityStatesForConfigNames()
        {
            var state = new SerializableEntityStateType(typeof(ThunderHenry.SkillStates.PunchCombo));
            Debug.LogWarning(ThunderHenryPlugin.MODNAME + ": DEBUG: TypeName print: " + state.typeName);
        }

        // Print out the first keyAsset and followerPrefab in an item display ruleset, to see if things work properly.
        private static void ItemDisplayTestDebug(int contentPackBodyIndex)
        {
            var bodyPrefab = Prefabs.bodyPrefabs[contentPackBodyIndex];

            var bodyModel = bodyPrefab.GetComponentInChildren<CharacterModel>();

            Debug.LogWarning(ThunderHenryPlugin.MODNAME + ": DEBUG: ItemDisplay KeyAsset: " + bodyModel.itemDisplayRuleSet.keyAssetRuleGroups[0].keyAsset);
            Debug.LogWarning(ThunderHenryPlugin.MODNAME + ": DEBUG: ItemDisplay FollowerPrefab: " + bodyModel.itemDisplayRuleSet.keyAssetRuleGroups[0].displayRuleGroup.rules[0].followerPrefab);
        }

        public static T[] Append<T>(ref T[] array, List<T> list)
        {
            var orig = array.Length;
            var added = list.Count;
            Array.Resize<T>(ref array, orig + added);
            list.CopyTo(array, orig);
            return array;
        }

        public static Func<T[], T[]> AppendDel<T>(List<T> list) => (r) => Append(ref r, list);

    }

    internal static class ArrayHelper
    {
        public static T[] Append<T>(ref T[] array, List<T> list)
        {
            var orig = array.Length;
            var added = list.Count;
            Array.Resize<T>(ref array, orig + added);
            list.CopyTo(array, orig);
            return array;
        }

        public static Func<T[], T[]> AppendDel<T>(List<T> list) => (r) => Append(ref r, list);
    }
}