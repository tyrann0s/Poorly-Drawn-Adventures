using System;
using DG.Tweening;
using Managers;
using Mobs.Status_Effects;
using Mobs.UI;
using UnityEngine;

namespace Mobs
{
    public class MobUI : MobComponent
    {
        [SerializeField]
        private Cursor cursor;

        [SerializeField]
        private HPBar hpBar;
        
        [SerializeField]
        private StatusEffectsPanel statusEffectsPanel;

        [SerializeField]
        private MobButtons buttons;

        public MobButtons Buttons => buttons;
        
        [SerializeField]
        private MobInfoPanel mobInfoPanel;

        [SerializeField]
        private MobText mobText;

        [SerializeField]
        private GameObject shieldIcon;
        [SerializeField]
        private float shieldScale;
        private Vector3 shieldOiriginPosition;
        
        [SerializeField] private SpriteRenderer mobSprite;
        private MaterialPropertyBlock mobSpriteMPB;
        [SerializeField] private Color allyColor, enemyColor;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            hpBar.Init(GetComponent<Mob>().MobHP, GetComponent<Mob>().MobData.MaxStamina);
            shieldOiriginPosition = shieldIcon.transform.localPosition;
            
            mobSpriteMPB = new MaterialPropertyBlock();
            
            if (ParentMob.IsHostile) statusEffectsPanel.transform.localPosition = new Vector3(-statusEffectsPanel.transform.localPosition.x, statusEffectsPanel.transform.localPosition.y, 0);
        }

        public void Activate()
        {
            buttons.ShowButtons();
            cursor.Deactivate();
            
            mobSprite.GetPropertyBlock(mobSpriteMPB);
            mobSpriteMPB.SetColor("_Outline_Color", allyColor);
            mobSpriteMPB.SetInt("OUTLINE_ON", 1);
            mobSprite.SetPropertyBlock(mobSpriteMPB);
            
        }

        public void HideUI()
        {
            if (this)
            {
                buttons.HideButtons();
                
                mobSprite.GetPropertyBlock(mobSpriteMPB);
                mobSpriteMPB.SetInt("OUTLINE_ON", 0);
                mobSprite.SetPropertyBlock(mobSpriteMPB);
            }
        }

        public void ShowEnemyHighlight()
        {
            mobSprite.GetPropertyBlock(mobSpriteMPB);
            mobSpriteMPB.SetColor("_Outline_Color", enemyColor);
            mobSpriteMPB.SetInt("OUTLINE_ON", 1);
            mobSprite.SetPropertyBlock(mobSpriteMPB);
            
            mobInfoPanel.gameObject.SetActive(true);
            
            string vulnerableTo = ProgressManager.Instance.RecordsMob.Find(x=> x.mobData == ParentMob.MobData.name).unlockVulnerabilty 
                ? ParentMob.MobData.VulnerableTo.ToString() : "Unknown";
            string immuneTo = ProgressManager.Instance.RecordsMob.Find(x=> x.mobData == ParentMob.MobData.name).unlockImmune 
                ? ParentMob.MobData.ImmuneTo.ToString() : "Unknown";
            
            mobInfoPanel.SetVulnerability(vulnerableTo);
            mobInfoPanel.SetImmunity(immuneTo);
        }

        public void HideEnemyHighlight()
        {
            mobSprite.GetPropertyBlock(mobSpriteMPB);
            mobSpriteMPB.SetInt("OUTLINE_ON", 0);
            mobSprite.SetPropertyBlock(mobSpriteMPB);
            
            mobInfoPanel.gameObject.SetActive(false);
        }

        public void MobDeath()
        {
            if (this)
            {
                buttons.HideButtons();
                hpBar.gameObject.SetActive(false);
                cursor.Hide();
            }
        }

        public void MobResurrect()
        {
            hpBar.gameObject.SetActive(true);
        }
        
        public void ShowCursor()
        {
            cursor.Show();
        }
        
        public void ShowTarget()
        {
            cursor.ShowTarget();
        }
        
        public void HideCursor()
        {
            cursor.Hide();
        }

        public void PickTarget(bool isFinal)
        {
            cursor.PickTarget(isFinal);
        }

        public void ShowTargetCursor()
        {
            switch (ServiceLocator.Get<GameManager>().SelectingState)
            {
                case SelectingState.None:
                    break;
                case SelectingState.Enemy:
                    foreach (Mob mob in ServiceLocator.Get<MobManager>().EnemyMobs)
                    {
                        if (!mob || !mob.IsHostile || mob.State == MobState.Dead)
                            continue;

                        if (mob.UI)
                        {
                            mob.UI.ShowTarget(); // Изменено с ShowTargetCursor() на ShowCursor()
                        }
                        else
                        {
                            Debug.LogError($"UI or Cursor is null on enemy mob {mob.name}");
                        }
                    }
                    break;
                case SelectingState.Player:
                    if (ParentMob.MobData.ActiveSkill is ResurrectSkill)
                    {
                        foreach (Mob mob in ServiceLocator.Get<MobManager>().PlayerMobs)
                        {
                            if (mob.UI && mob != ServiceLocator.Get<GameManager>().ActivatedMob &&
                                mob.State == MobState.Dead)
                            {
                                mob.UI.ShowTarget();
                            }
                        }
                    }
                    else
                    {
                        foreach (Mob mob in ServiceLocator.Get<MobManager>().PlayerMobs)
                        {
                            if (mob.UI && mob != ServiceLocator.Get<GameManager>().ActivatedMob && mob.State != MobState.Dead)
                            {
                                mob.UI.ShowTarget();
                            }
                        }
                    }
                    break;
            }
        }

        public void UpdateHP(float value)
        {
            if (hpBar != null)
            {
                hpBar.UpdateHP(value);
            }
        }
        
        public void UpdateStamina(float value)
        {
            if (hpBar != null)
            {
                hpBar.UpdateStamina(value);
            }
        }

        public void AddStatusEffect(StatusEffect effect)
        {
            statusEffectsPanel.ShowEffect(effect);
        }
        
        public void RemoveStatusEffect(StatusEffect effect)
        {
            statusEffectsPanel.HideEffect(effect);
        }

        public void ShowText(string value, Color color)
        {
            mobText.ShowText(value, color);
        }

        public void ShowShield()
        {
            if (shieldIcon == null)
            {
                Debug.LogError($"Shield icon is null in {transform.name}");
                return;
            }

            DOTween.Sequence()
                .Append(shieldIcon.transform.DOScale(shieldScale, .3f))
                .AppendInterval(.3f);
        }

        public void HideShield()
        {
            DOTween.Sequence()
                .Append(shieldIcon.transform.DOScale(Vector3.zero, .3f))
                .Join(shieldIcon.transform.DOLocalMove(shieldOiriginPosition, .5f));
        }
        
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}