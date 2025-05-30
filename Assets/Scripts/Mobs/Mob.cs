using Cards;
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
        Defense,
        Stun,
        Dead
    }
    
    public class Mob : MonoBehaviour
    {
        [SerializeField]
        private float staminaRestoreAmount;
        public float StaminaRestoreAmount => staminaRestoreAmount;
        
        [SerializeField]
        private MobData mobData;
        public MobData MobData => mobData;
        
        public MobCombatSystem MobCombatSystem { get; private set; }
        public MobInput MobInput { get; private set; }
        public MobActions MobActions { get; private set; }
        public MobMovement MobMovement { get; private set; }
        public MobVFX MobVFX { get; private set; }
        public AnimationController AnimationController { get; private set; }
        public SoundController SoundController { get; private set; }
        public MobUI UI { get; private set; }

        public bool IsHostile { get; set; }
        public MobState State { get; set; }
        public int StunTime { get; set; }

        public Action CurrentAction { get; set; } = new Action();
        
        public float MobHP {get; set;}
        public float MobStamina { get; set; }
    
        public ElementCombo CurrentCombo { get; set; }

        private void Awake()
        {
            MobCombatSystem = GetComponent<MobCombatSystem>();
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
            MobStamina = mobData.MaxStamina;
        }
        
        public void Activate()
        {
            foreach (Mob mob in GameManager.Instance.PlayerMobs)
            {
                if (mob != this)
                {
                    GameManager.Instance.ActivatedMob = this;
                    mob.UI.HideUI();
                }
            }

            SoundController.Pick();
            UI.Activate();
            State = MobState.Activated;
        }

        public void Deactivate()
        {
            UI.HideUI();
            GameManager.Instance.ActivatedMob = null;
        }
    }
}
