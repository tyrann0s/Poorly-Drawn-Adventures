using DG.Tweening;
using Managers;
using UnityEngine;

namespace Mobs
{
    public class MobUI : MobComponent
    {
        [SerializeField]
        private Cursor cursor;
        public Cursor MobCursor => cursor;

        [SerializeField]
        private HPBar hpBar;

        [SerializeField]
        private MobButtons buttons;

        [SerializeField]
        private MobText mobText;

        [SerializeField]
        private GameObject shieldIcon;
        [SerializeField]
        private float shieldScale;
        private Vector3 shieldOiriginPosition;

        protected override void Awake()
        {
            base.Awake();
            if (cursor == null)
            {
                Debug.LogError($"Cursor not found on {transform.name}");
            }

            if (hpBar == null)
            {
                Debug.LogError($"HP Bar not found on {transform.name}");
            }

            if (buttons == null)
            {
                Debug.LogError($"Buttons not found on {transform.name}");
            }

            if (mobText == null)
            {
                Debug.LogError($"MobText not found on {transform.name}");
            }

            if (shieldIcon == null)
            {
                Debug.LogError($"Shield icon not found on {transform.name}");
            }
        }

        private void Start()
        {
            hpBar.Init(GetComponent<Mob>().MobHP);
            shieldOiriginPosition = shieldIcon.transform.localPosition;
        }

        public void Activate()
        {
            buttons.ShowButtons();
            cursor.Activate();
        }

        public void HideUI()
        {
            MobCursor.Deactivate();
            buttons.HideButtons();
        }

        public void MobDeath()
        {
            buttons.HideButtons();
            hpBar.gameObject.SetActive(false);
            cursor.Hide();
        }

        public void ShowTargetCursor()
        {
            foreach (Mob mob in GameManager.Instance.EnemyMobs)
            {
                if (mob == null || !mob.IsHostile || mob.State == MobState.Dead || mob.State == MobState.Stun)
                    continue;

                if (mob.UI != null && mob.UI.MobCursor != null)
                {
                    mob.UI.MobCursor.ShowTarget();
                }
                else
                {
                    Debug.LogError($"UI or Cursor is null on enemy mob {mob.name}");
                }
            }
        }

        public void UpdateHP(float value)
        {
            if (hpBar != null)
            {
                hpBar.UpdateHP(value);
            }
        }

        public void UpdateMaxHP(int value)
        {
            if (hpBar != null)
            {
                hpBar.UpdateMaxHP(value);
            }
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
                .Join(shieldIcon.transform.DOLocalMove(Vector3.zero, .5f))
                .AppendInterval(.3f)
                .OnComplete(() =>
                {
                    var mob = GetComponent<Mob>();
                    if (mob != null)
                    {
                        mob.MobActions.NextAction();
                    }
                    else
                    {
                        Debug.LogError($"Mob component is null in {transform.name}");
                    }
                });
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
