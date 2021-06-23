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
            ItemDisplayTestDebug();
        }

        // Entity State Configurations require very specific 'Assembly Qualified Names' in order to work correctly.
        // This should print you the name that you need to put into the Entity State Configuration.
        private static void PrintEntityStatesForConfigNames()
        {
            var state = new SerializableEntityStateType(typeof(ThunderHenry.SkillStates.SlashCombo));
            Debug.LogWarning(ThunderHenryPlugin.MODNAME + ": DEBUG: TypeName print: " + state.typeName);
        }

        private static void ItemDisplayTestDebug()
        {
            var bodyPrefab = Prefabs.bodyPrefabs[0];

            var bodyModel = bodyPrefab.GetComponentInChildren<CharacterModel>();

            Debug.LogWarning(bodyModel.itemDisplayRuleSet.keyAssetRuleGroups[0].keyAsset);
            Debug.LogWarning(bodyModel.itemDisplayRuleSet.keyAssetRuleGroups[0].displayRuleGroup.rules[0].followerPrefab);
        }

        internal static string ScepterDescription(string desc)
        {
            return "\n<color=#d299ff>SCEPTER: " + desc + "</color>";
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