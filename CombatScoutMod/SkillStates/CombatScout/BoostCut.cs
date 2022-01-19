using EntityStates;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace CombatScoutMod.SkillStates
{
	public class BoostCut : BaseSkillState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (base.isAuthority)
			{
				base.characterMotor.Motor.ForceUnground();
				base.characterMotor.disableAirControlUntilCollision = true;
				Ray aimRay = base.GetAimRay();
				Vector3 xzPlane = Vector3.up;
				Vector3 aimShadow = Vector3.ProjectOnPlane(aimRay.direction, xzPlane);
				this.punchVelocity = aimShadow;
			}
		}

		public override void OnExit()
		{
			base.characterMotor.velocity = Vector3.zero;
			base.characterMotor.disableAirControlUntilCollision = false;
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.characterMotor.velocity = (this.punchVelocity * this.speed + Vector3.down * sGravity);
			base.characterDirection.forward = this.punchVelocity;

			if (base.isAuthority && base.fixedAge >= this.duration)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		protected virtual void OnHitEnemyAuthority()
		{
			Action<BoostCut> action = BoostCut.onHitAuthorityGlobal;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public static event Action<BoostCut> onHitAuthorityGlobal;

		public float duration = 0.2f;

		public float sGravity = 10f; 

		public float speed = 60f;

		protected Vector3 punchVelocity;

	}
}