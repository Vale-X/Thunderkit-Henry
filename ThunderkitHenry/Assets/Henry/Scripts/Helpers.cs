using System;
using System.Collections.Generic;
using EntityStates;
using UnityEngine;

namespace ThunderHenry.Modules
{
    internal static class Helpers
    {
        internal const string agilePrefix = "<style=cIsUtility>Agile.</style> ";
        public static void RunDebug()
        {
            PrintEntityStatesForConfigNames();
        }

        // Entity State Types require very specific 'Assembly Qualified Names' in order to work correctly.
        // This should print you the name that you need to put into the Entity State Configuration.
        private static void PrintEntityStatesForConfigNames()
        {
            foreach (var es in Modules.Assets.mainContentPack.entityStateTypes)
            {
                var state = new SerializableEntityStateType(typeof(ThunderHenry.SkillStates.SlashCombo));
                Debug.LogWarning(ThunderHenryPlugin.MODNAME + ": DEBUG: TypeName print: " + state.typeName);
            }
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