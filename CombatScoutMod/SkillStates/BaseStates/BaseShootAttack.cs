using EntityStates;
using RoR2;
using UnityEngine;

namespace CombatScoutMod.SkillStates.BaseStates
{
    public class BaseShootAttack : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.gunDamageCoefficient;
        public static float baseDuration = 0.15f;
        public static float force = 100f;
        public static float recoil = 0.2f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.tracerEffectPrefab;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = BaseShootAttack.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.1f * this.duration;
            base.characterBody.SetAimTimer(0.01f);
            this.muzzleString = "Muzzle";
            //base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 0.01f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound("CombatScoutShootPistol", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * BaseShootAttack.recoil, -2f * BaseShootAttack.recoil, -0.5f * BaseShootAttack.recoil, 0.5f * BaseShootAttack.recoil);

                    new BulletAttack
                    {
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = BaseShootAttack.damageCoefficient * this.damageStat,
                        force = BaseShootAttack.force,
                        minSpread = 0f,
                        maxSpread = 0.3f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        radius = 0.1f,
                        //weapon = null,
                        tracerEffectPrefab = BaseShootAttack.tracerEffectPrefab,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab
                    }.Fire();
                }
            }
        }
    }
}