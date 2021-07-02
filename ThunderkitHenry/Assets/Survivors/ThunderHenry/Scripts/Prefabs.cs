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

        private static PhysicMaterial ragdollMaterial;

        internal static void Init()
        {
            GetPrefabs();
            AddPrefabReferences();
        }

        internal static void AddPrefabReferences()
        {
            ForEachReferences();

            //If you want to change the 'defaults' set in ForEachReferences, then set them for individual bodyPrefabs here.
            //This is if you want to use a custom crosshair or other stuff.

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

                SetupRagdoll(g);
            }
        }

        // Code from the original henry to setup Ragdolls for you.
        // This is so you dont have to manually set the layers for each object in the bones list.
        private static void SetupRagdoll(GameObject model)
        {
            RagdollController ragdollController = model.GetComponent<RagdollController>();

            if (!ragdollController) return;

            if (ragdollMaterial == null) ragdollMaterial = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;

            foreach (Transform i in ragdollController.bones)
            {
                if (i)
                {
                    i.gameObject.layer = LayerIndex.ragdoll.intVal;
                    Collider j = i.GetComponent<Collider>();
                    if (j)
                    {
                        j.material = ragdollMaterial;
                        j.sharedMaterial = ragdollMaterial;
                    }
                }
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
