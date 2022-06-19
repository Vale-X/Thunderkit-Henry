using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace ThunderHenry.Modules.Components
{
    // just a class to run some custom code for things like weapon models
    // Attached as a component onto ThunderHenryBody.
    public class HenryController : MonoBehaviour, IOnKilledOtherServerReceiver
    {
        private CharacterBody characterBody;
        private ChildLocator childLocator;

        private List<TrailRenderer> thunderTrails = new List<TrailRenderer>();

        // This is where the passive is for ThunderHenry, but passives can be anywhere.
        // Put your passive (if you want one) wherever you think is appropriate.
        public void OnKilledOtherServer(DamageReport damageReport)
        {
            // Add the buff
            characterBody.AddTimedBuff(Buffs.speedBuff, StaticValues.speedBuffDuration, StaticValues.speedBuffMaxStacks);

            // Refresh all buff instance timers
            for (int i = 0; i < characterBody.timedBuffs.Count; i++)
            {
                if (characterBody.timedBuffs[i].buffIndex == Buffs.speedBuff.buffIndex)
                {
                    characterBody.timedBuffs[i].timer = StaticValues.speedBuffDuration;
                }
            }
        }

        private void Awake()
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();

            Invoke("CheckWeapon", 0.2f);
        }

        private void Start()
        {
            thunderTrails.AddRange(characterBody.gameObject.GetComponentsInChildren<TrailRenderer>());
        }

        private void FixedUpdate()
        {
            HandleThunderParticles();
        }

        // Run as an Invoke to give time for everything else to be set up and sorted.
        private void CheckWeapon()
        {
            if (this.characterBody.skillLocator.secondary.skillDef.skillNameToken == ThunderHenryPlugin.developerPrefix + "_THUNDERHENRY_BODY_SECONDARY_UZI_NAME")
            {
                this.childLocator.FindChild("GunModel").GetComponent<SkinnedMeshRenderer>().sharedMesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshUzi");
            }
        }

        // Handles trail width for passive
        private void HandleThunderParticles()
        {
            var buffCount = characterBody.GetBuffCount(Buffs.speedBuff);
            float buffScaled = (float)buffCount / StaticValues.speedBuffMaxStacks;

            foreach (TrailRenderer trail in thunderTrails)
            {
                trail.widthMultiplier = Mathf.MoveTowards(trail.widthMultiplier, buffScaled * 0.5f, Time.deltaTime);
            }
        }
    }
}