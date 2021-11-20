using R2API;
using RoR2;
using System.Collections.Generic;
using ThunderHenry.Achievements;
using UnityEngine;

namespace ThunderHenry.Modules
{
    internal class Unlockables
    {
        public static List<UnlockableDef> loadedUnlockableDefs = new List<UnlockableDef>();

        public static void Init()
        {
            AddUnlockables();
        }

        // By not loading unlockable defs, the content is unlocked automatically.
        // Controlling that through a config option, you allow people to choose to skip achievement requirements if they so choose.
        // Unlockables are now handled by R2API, which was updated to support premade UnlockableDefs (which you make in Unity).
        private static void AddUnlockables()
        {
            if (!Config.forceUnlock.Value)
            {
                loadedUnlockableDefs.Add(UnlockableAPI.AddUnlockable<ThunderHenryUnlock>(FindUnlockable("Characters.ThunderHenry")));
                loadedUnlockableDefs.Add(UnlockableAPI.AddUnlockable<ThunderHenryMastery>(FindUnlockable("Skins.ThunderHenry.Alt1")));
                loadedUnlockableDefs.Add(UnlockableAPI.AddUnlockable<ThunderHenryUzi>(FindUnlockable("Skills.ThunderHenry.SecondaryAlt1")));
            }
        }

        private static UnlockableDef FindUnlockable(string name)
        {
            return Modules.Assets.mainAssetBundle.LoadAsset<UnlockableDef>(name);
        }
    }
}
