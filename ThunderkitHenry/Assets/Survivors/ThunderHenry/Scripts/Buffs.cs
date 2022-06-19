using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderHenry.Modules
{
    class Buffs
    {
        internal static List<BuffDef> loadedBuffs = new List<BuffDef>();

        //Buffs
        internal static BuffDef armorBuff;
        internal static BuffDef speedBuff;

        //Debuffs


        internal static void Init()
        {
            CollectBuffs();
        }


        // Grabs all the buffDefs in your content pack for reference in code
        // Order should be the same as the SerializedContentPack BuffDefs list.
        private static void CollectBuffs()
        {
            armorBuff = GetBuff("ThunderHenryArmorBuff");
            speedBuff = GetBuff("ThunderHenrySpeedBuff");
        }

        internal static BuffDef GetBuff(string buffName)
        {
            BuffDef def = Assets.mainAssetBundle.LoadAsset<BuffDef>(buffName);
            if (def)
            {
                loadedBuffs.Add(def);
                return def;
            }
            else if (ThunderHenryPlugin.debug) { Debug.LogWarning(ThunderHenryPlugin.MODNAME + ": BuffDef not found: " + def); }
            return null;
        }

        internal static void HandleBuffs(CharacterBody body)
        {
            if (body.HasBuff(armorBuff.buffIndex))
            {
                body.armor += StaticValues.armorBuffValue;
            }

            if (body.HasBuff(speedBuff.buffIndex))
            {
                body.moveSpeed *= 1 + (StaticValues.speedBuffCoefficient * (body.GetBuffCount(speedBuff.buffIndex)));
            }
        }

        internal static void HandleDebuffs(CharacterBody body)
        {

        }
    }
}
