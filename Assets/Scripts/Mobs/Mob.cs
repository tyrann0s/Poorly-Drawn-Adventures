using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField]
    private int staminaRestoreAmount;

    private SpriteRenderer spriteRenderer;

    private AnimationController animationController;
    private SoundController soundController;
    private MobUI ui;
    public MobUI UI => ui;

    [SerializeField]
    private GameObject rivalPosition;
    public GameObject RivalPosition => rivalPosition;

    [SerializeField]
    private ParticleSystem deathVFX, getDamageVFX, shieldHitVFX;

    public bool IsHostile { get; set; }
    public bool IsActivated { get; set; }
    public bool IsDead { get; set; }
    public bool IsHaveAction { get; set; }
    public bool IsTarget { get; set; }
    public bool IsUnderDefense { get; set; }

    private GameManager gameManager;
    private UIManager uiManager;
    private UISounds uiSounds;
    private QueueManager queueManager;

    public Action CurrentAction { get; set; } = new Action();

    public Vector3 OriginPosition { get; set; }

    [SerializeField]
    private MobData mobData;
    public MobData MobData => mobData;

    private int mobHP, mobStamina;
    public int MobHP => mobHP;
    public int MobStamina => mobStamina;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        uiSounds = FindObjectOfType<UISounds>();
        queueManager = FindObjectOfType<QueueManager>();
        animationController = GetComponent<AnimationController>();
        soundController = GetComponent<SoundController>();
        ui = GetComponent<MobUI>();
    }

    private void Start()
    {
        animationController.PlayIdle_Animation();

        mobHP = mobData.MaxHP;
        mobStamina = mobData.MaxStamina;

        if (IsHostile) rivalPosition.transform.localPosition = new Vector3(-rivalPosition.transform.localPosition.x, 0, 0);
    }

    public void MirrorMob()
    {
        spriteRenderer.flipX = true;
    }

    private void OnMouseEnter()
    {
        if (!IsHostile && !IsHaveAction && !IsDead && !IsActivated && !gameManager.ControlLock)
        {
            ui.MobCursor.Show();
            uiSounds.ButtonHover();
        }

        if (!IsHostile && !IsDead && gameManager.ActivatedMob == null) uiManager.UpdateCurrentMobPanel(this);
        if (IsHostile && !IsDead) uiManager.UpdateEnemyMobPanel(this);

        if (IsHostile && gameManager.ControlLock)
        {
            ui.MobCursor.ZoomIn();
        }
    }

    private void OnMouseExit()
    {
        if (!IsHostile && !IsHaveAction && !IsDead && !IsActivated && !gameManager.ControlLock)
        {
            ui.MobCursor.Hide();
        }

        if (!IsHostile && !IsDead && gameManager.ActivatedMob == null) uiManager.UpdateCurrentMobPanel(null);
        if (IsHostile && !IsDead) uiManager.UpdateEnemyMobPanel(null);

        if (IsHostile && gameManager.ControlLock)
        {
            ui.MobCursor.ZoomOut();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!IsHostile && !IsHaveAction && !IsDead && !gameManager.ControlLock)
            {
                if (IsActivated)
                {
                    Deactivate();
                }
                else
                {
                    Activate();                  
                }
            }

            if (IsHostile && gameManager.ControlLock)
            {
                ui.MobCursor.PickTarget();
                gameManager.PickingMob.CurrentAction.TargetInstance = this;
                gameManager.PickingMob.ActionPrepared();
                gameManager.PickingMob.Deactivate();
                foreach (Mob mob in gameManager.EnemyMobs)
                {
                    if (mob != this) mob.Deactivate();
                }
                gameManager.ControlLock = false;
            }
        }
    }

    public void Activate()
    {
        foreach (Mob mob in gameManager.PlayerMobs)
        {
            if (mob != this)
            {
                mob.IsActivated = false;
                gameManager.ActivatedMob = this;
                mob.UI.HideUI();
            }
        }

        soundController.Pick();
        ui.Activate();
        IsActivated = true;
        uiManager.UpdateCurrentMobPanel(this);
    }

    public void Deactivate()
    {
        ui.HideUI();
        IsActivated = false;
        gameManager.ActivatedMob = null;
        uiManager.UpdateCurrentMobPanel(null);
    }

    public void GetDamage(int amount)
    {
        if (IsUnderDefense)
        {
            soundController.GetShieldDamage();
            ui.ShowText("Blocked!", Color.blue);
            shieldHitVFX.Play();
        }
        else
        {
            mobHP -= amount;
            ui.UpdateHPBar(mobHP);
            ui.ShowText(amount.ToString(), Color.red);
            if (mobHP > 0)
            {
                soundController.GetDamage();
                getDamageVFX.Play();
                animationController.PlayGetDamage_Animation();
                if (IsActivated) uiManager.UpdateCurrentMobPanel(this);
            }
            else Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        soundController.Death();
        ui.MobDeath();
        gameManager?.MobDied(this);
        animationController.PlayDie_Animation();
        deathVFX.Play();
    }

    public void SkpTurn()
    {
        CurrentAction.MobAction = ActionType.SkipTurn;
        CurrentAction.MobInstance = this;
        Deactivate();

        ActionPrepared();
    }

    public void Defense()
    {
        CurrentAction.MobAction = ActionType.Defense;
        CurrentAction.MobInstance = this;
        Deactivate();

        ActionPrepared();
    }

    public void Attack1()
    {
        Attack(ActionType.Attack1);
    }

    public void Attack2()
    {
        Attack(ActionType.Attack2);
    }

    private void ActionPrepared()
    {
        IsHaveAction = true;
        transform.DOMove(rivalPosition.transform.position, .3f);
        gameManager.ReadyToFight();
        uiSounds.ActionConfirm();
    }

    private void Attack(ActionType action)
    {
        CurrentAction.MobAction = action;
        CurrentAction.MobInstance = this;
        ui.ShowTargetCursor();
        gameManager.ControlLock = true;
        gameManager.PickingMob = this;
    }

    public void PerformSkipTurn()
    {
        ui.ShowText("+" + staminaRestoreAmount + " stamina!", Color.green);
        mobStamina += staminaRestoreAmount;
        if (mobStamina > mobData.MaxStamina) mobStamina = mobData.MaxStamina;
        NextAction();
    }

    public void PerformDefense()
    {
        mobStamina -= mobData.DefenseCost;
        IsUnderDefense = true;
        ui.ShowText("Defense!", Color.blue);
        uiSounds.ShieldActivation();
        ui.ShowShield();
    }

    public void NextAction()
    {
        queueManager.NextAction();
    }

    public void PerformAttack()
    {
        soundController.StartMove();
        animationController.PlayRun_Animation();
        DOTween.Sequence()
            .Append(transform.DOMove(CurrentAction.TargetInstance.RivalPosition.transform.position, 1f))
            .OnComplete(PlayAttackAnimation);
    }

    private void PlayAttackAnimation()
    {
        soundController.StopMove();
        switch (CurrentAction.MobAction)
        {
            case ActionType.Attack1:
                animationController.PlayAttack1_Animation();
                soundController.Attack1();
                break;

            case ActionType.Attack2:
                animationController.PlayAttack2_Animation();
                soundController.Attack2();
                break;
        }
    }

    private void MakeDamage()
    {
        switch (CurrentAction.MobAction)
        {
            case ActionType.Attack1:
                StartCoroutine(DamageCoroutine(mobData.Attack1Damage, mobData.Attack1Cost));
                break;

            case ActionType.Attack2:
                StartCoroutine(DamageCoroutine(mobData.Attack2Damage, mobData.Attack2Cost));
                break;
        } 
    }

    private IEnumerator DamageCoroutine(int damage, int cost)
    {
        CurrentAction.TargetInstance.GetDamage(damage);
        mobStamina -= cost;

        yield return new WaitForSeconds(.5f);
        GoToOriginPosition(true);

        yield return new WaitForSeconds(1);
        NextAction();
    }

    public void GoToOriginPosition(bool withSound)
    {
        if (transform.position == OriginPosition) return;

        FlipMob();
        if (withSound) soundController.StartMove();
        animationController.PlayRun_Animation();
        DOTween.Sequence()
            .Append(transform.DOMove(OriginPosition, 1f))
            .OnComplete(FlipMob);
    }

    private void FlipMob()
    {
        if (spriteRenderer.flipX) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        soundController.StopMove();
        animationController.PlayIdle_Animation();
    }

    public bool CheckStamina()
    {
        switch (CurrentAction.MobAction)
        {
            case ActionType.SkipTurn:
                return true;

            case ActionType.Defense:
                if (mobStamina >= mobData.DefenseCost) return true;
                else return false;

            case ActionType.Attack1:
                if (mobStamina >= mobData.Attack1Cost) return true;
                else return false;

            case ActionType.Attack2:
                if (mobStamina >= mobData.Attack2Cost) return true;
                else return false;
        }

        return false;
    }
}
