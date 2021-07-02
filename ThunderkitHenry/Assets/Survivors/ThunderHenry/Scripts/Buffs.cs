using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2;

namespace ThunderHenry.Modules
{
    class Buffs
    {
        internal static BuffDef[] buffDefs;

        internal static void Init()
        {
            CollectBuffs();
        }


        // Grabs all the buffDefs in your content pack for reference in code
        // Order should be the same as the SerializedContentPack BuffDefs list.
        private static void CollectBuffs()
        {
            buffDefs = Modules.Assets.mainContentPack.buffDefs.ToArray();
        }
    }
}
