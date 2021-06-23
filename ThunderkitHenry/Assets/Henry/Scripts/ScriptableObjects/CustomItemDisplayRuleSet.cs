using System;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using ThunderHenry.Modules;

namespace ThunderHenry.Scriptables
{
    [CreateAssetMenu]
    class CodeBasedIDRS : ItemDisplayRuleSet
    {
        // This is a Scriptable Object for filling in custom Item Display info the old way, but plugging them into the character the new way.
        // (this saves a load of hassle and lets you use ItemDisplayPlacementHelper the same way as before).
        // CREATE A NEW SCRIPT that inherits from this one. Override the SetItemDisplays. Add [CreateAssetMenu] above the class name.
        // Then you can create a new asset based on the new item display ruleset and plug that into your survivor.
        // Look at 'HenryItemDisplayRuleSet' for an example! The Asset is found in Henry > Definitions.
        internal List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

        public virtual void SetItemDisplays()
        {
            itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
        }

        public void ActivateItemDisplays()
        {
            this.SetItemDisplays();
            this.keyAssetRuleGroups = itemDisplayRules.ToArray();
            this.GenerateRuntimeValues();
        }

        public void Awake()
        {
            ActivateItemDisplays();
        }
    }
}
