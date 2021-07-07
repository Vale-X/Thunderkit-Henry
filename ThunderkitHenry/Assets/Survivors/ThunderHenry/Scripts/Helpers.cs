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
        // This code wont be run if 'Debug' is off within ThunderHenryPlugin.
        // Set that to off before you release a mod or update.
        public static void AwakeDebug()
        {
            PrintEntityStatesForConfigNames();
        }

        // Entity State Configurations require very specific 'Assembly Qualified Names' in order to work correctly.
        // This should print you the name that you need to put into the Entity State Configuration.
        // replace `ThunderHenry.SkillStates.SlashCombo` with a skillstate in your mod. 
        private static void PrintEntityStatesForConfigNames()
        {
            var state = new SerializableEntityStateType(typeof(ThunderHenry.SkillStates.Shoot));
            Debug.LogWarning(ThunderHenryPlugin.MODNAME + ": DEBUG: TypeName print: " + state.typeName);
        }
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

        public static T[] AppendSingle<T>(ref T[] array, T item)
        {
            List<T> list = new List<T>();
            list.Add(item);
            var backArray = Append<T>(ref array, list);
            return backArray;
        }

        public static Func<T[], T[]> AppendDel<T>(List<T> list) => (r) => Append(ref r, list);
    }
}