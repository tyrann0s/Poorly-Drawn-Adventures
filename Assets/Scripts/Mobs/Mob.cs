using System;
using Cards;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Mobs
{
    public enum MobState
    {
        Idle,
        Ready,
        Activated,
        Attack,
        Dead
    }
    
    public class Mob : MonoBehaviour
    {
        [SerializeField]
        private MobData mobData;
        public MobData MobData => mobData;
        
        public MobCombatSystem MobCombatSystem { get; private set; }
        public MobStatusEffects MobStatusEffects { get; private set; }
        public MobInput MobInput { get; private set; }
        public MobActions MobActions { get; private set; }
        public MobMovement MobMovement { get; private set; }
        public MobVFX MobVFX { get; private set; }
        public AnimationController AnimationController { get; private set; }
        public SoundController SoundController { get; private set; }
        public MobUI UI { get; private set; }

        public bool IsHostile { get; set; }
        public MobState State { get; set; }

        public MobAction CurrentAction { get; set; } = new();
        public MobAction PassiveAction { get; set; } = new();
        
        public float MobHP {get; set;}

        private float mobStamina;

        public bool CheckStamina(float cost)
        {
            return mobStamina >= cost + MobStatusEffects.GetStaminaChange();
        }

        public void SpendStamina(float value)
        {
            mobStamina -= value + MobStatusEffects.GetStaminaChange();
        }
        
        public void RestoreStamina()
        {
            mobStamina = mobData.MaxStamina;
        }
    
        public ElementCombo CurrentCombo { get; set; }

        private void Awake()
        {
            MobCombatSystem = GetComponent<MobCombatSystem>();
            MobStatusEffects = GetComponent<MobStatusEffects>();
            MobInput = GetComponent<MobInput>();
            MobActions = GetComponent<MobActions>();
            MobMovement = GetComponent<MobMovement>();
            MobVFX = GetComponent<MobVFX>();
            AnimationController = GetComponent<AnimationController>();
            SoundController = GetComponent<SoundController>();
            UI = GetComponent<MobUI>();

            if (AnimationController == null) Debug.LogError($"AnimationController not found on {name}");
            if (SoundController == null) Debug.LogError($"SoundController not found on {name}");
            if (UI == null) Debug.LogError($"MobUI not found on {name}");
        }

        private void Start()
        {
            AnimationController.PlayIdle_Animation();

            MobHP = mobData.MaxHP;
            mobStamina = mobData.MaxStamina;

            if (mobData.ActiveSkill == null) Debug.LogError($"АКТИВНЫЙ СКИЛЛ НЕ НАЗНАЧЕН на {MobData.MobName}");
            
            mobData.ActiveSkill?.Initialize(this, mobData.SkillDamage, mobData.SkillCost, mobData.ActiveDuration);
            mobData.PassiveSkill?.Initialize(this, mobData.PassiveDamage, 0, mobData.PassiveDuration);
        }
        
        public void Activate()
        {
            foreach (Mob mob in ServiceLocator.Get<MobManager>().PlayerMobs)
            {
                if (mob != this)
                {
                    mob.Deactivate();
                }
            }

            ServiceLocator.Get<GameManager>().ActivatedMob = this;
            SoundController.Pick();
            UI.Activate();
            State = MobState.Activated;
        }

        public void Deactivate()
        {
            if (State == MobState.Activated) State = MobState.Idle;
            UI.HideUI();
        }

        private void OnDestroy()
        {
            MobData.PassiveSkill?.Cleanup();
        }
    }
}
