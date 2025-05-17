using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayIdle_Animation()
    {
        animator.Play("Idle");
    }

    public void PlayRun_Animation()
    {
        animator.Play("Run");
    }

    public void PlayAttack1_Animation()
    {
        animator.Play("Attack1");
    }

    public void PlayAttack2_Animation()
    {
        animator.Play("Attack2");
    }

    public void PlayGetDamage_Animation()
    {
        animator.Play("GetDamage");
    }

    public void PlayDie_Animation()
    {
        animator.Play("Die");
    }
}
