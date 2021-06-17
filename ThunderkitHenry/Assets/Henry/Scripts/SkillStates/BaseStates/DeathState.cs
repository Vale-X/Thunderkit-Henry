using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;

namespace ThunderHenryMod.SkillStates.BaseStates
{
	public class DeathState : GenericCharacterDeath
	{
		private Vector3 previousPosition;
		private float upSpeedVelocity;
		private float upSpeed;
		private Animator modelAnimator;

		public override void OnEnter()
		{
			base.OnEnter();
			Vector3 vector = Vector3.up * 3f;
			if (base.characterMotor)
			{
				vector += base.characterMotor.velocity;
				base.characterMotor.enabled = false;
			}
			if (base.cachedModelTransform)
			{
				RagdollController component = base.cachedModelTransform.GetComponent<RagdollController>();
				if (component)
				{
					component.BeginRagdoll(vector);
				}
			}
		}

		protected override void PlayDeathAnimation(float crossfadeDuration = 0.1f)
		{
		}
		protected override bool shouldAutoDestroy
		{
			get
			{
				return false;
			}
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (NetworkServer.active && base.fixedAge > 4f)
			{
				EntityState.Destroy(base.gameObject);
			}
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Death;
		}

	}
}
