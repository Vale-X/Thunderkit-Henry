using UnityEngine;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine.Networking;

namespace ThunderHenry.SkillStates
{
    // Uses RoR2's BasicMeleeAttack, since Henry's melee is super simple.
    // Most of the data for the attack is found in the Skill's EntityStateConfig (Survivors/ThunderHenryDefinitions/Skills/StateConfigs).
    // More complex primary skills should inherit from something else (likely BaseSkillState).
    public class SlashCombo : BasicMeleeAttack, SteppedSkillDef.IStepSetter
    {
        [SerializeField]
        public float baseEarlyExitTime = 0.4f;

        private int comboIndex;
        private float earlyExitTime = 0.4f;

        public void SetStep(int i)
        {
            comboIndex = i;
            swingEffectMuzzleString = (comboIndex == 0 ? "SwingLeft" : "SwingRight");
        }

        public override float CalcDuration()
        {
            earlyExitTime = baseEarlyExitTime / (ignoreAttackSpeed ? 1 : attackSpeedStat);

            return base.CalcDuration();
        }

        public override void PlayAnimation()
        {
            PlayCrossfade("Gesture, Override", "Slash" + (comboIndex + 1), "Slash.playbackRate", duration, 0.05f);
        }

        public override void AuthorityFixedUpdate()
        {
            base.AuthorityFixedUpdate();

            if ((base.duration - earlyExitTime) <= fixedAge)
            {
                AuthorityOnFinish();
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)comboIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            comboIndex = (int)reader.ReadByte();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
