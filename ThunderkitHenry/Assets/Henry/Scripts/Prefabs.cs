using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RoR2;

namespace ThunderHenry.Modules
{
    internal static class Prefabs
    {
        //The order of your SurvivorDefs in your SerializableContentPack determines the order of body/displayPrefabs.


        // Name of any SurvivorDefs you have in your asset bundle.
        // entry 0 of bodyPrefabs/displayPrefabs will be the first entry in survivorDefs, and so on.
        // This lets you reference any bodyPrefabs or displayPrefabs throughout your code.
        //internal static string[] survivorDefs = new string[] { "HenryDef" };
        //variable not actually needed cause you can get which body is which from your ContentPack

        internal static List<GameObject> bodyPrefabs = new List<GameObject>();
        internal static List<GameObject> displayPrefabs = new List<GameObject>();

        internal static void Init()
        {
            GetPrefabs();
            AddPrefabReferences();
        }

        internal static void AddPrefabReferences()
        {
            ForEachReferences();

            //If you want to change the 'defaults' set in ForEachReferences, then set them for individual bodyPrefabs here.
            //This is if you want to use a custom crosshair.

            // bodyPrefabs[0].GetComponent<CharacterBody>().crosshairPrefab = ...whatever you wanna set here.
        }

        private static void ForEachReferences()
        {
            foreach (GameObject g in bodyPrefabs)
            {
                var cb = g.GetComponent<CharacterBody>();
                cb.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/StandardCrosshair");
                cb.preferredPodPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/SurvivorPod");

                var fs = g.GetComponentInChildren<FootstepHandler>();
                fs.footstepDustPrefab = Resources.Load<GameObject>("prefabs/GenericFootstepDust");
            }
        }

        // Find all relevant prefabs within the content pack, per SurvivorDefs.
        internal static void GetPrefabs()
        {
            var d = Assets.mainContentPack.survivorDefs;
            foreach (SurvivorDef s in d)
            {
                bodyPrefabs.Add(s.bodyPrefab);
                displayPrefabs.Add(s.displayPrefab);
            }
        }
    }
}
