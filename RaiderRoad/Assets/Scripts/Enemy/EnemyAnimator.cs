using UnityEngine;
using System.Collections;

/// <summary>
/// Adapter class to handle talking to the animator on the Raiders
/// </summary>
public class EnemyAnimator
{
    private Animator animator;

    public EnemyAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public bool Running
    {
        get
        {
            return animator.GetBool("Running");
        }
        set
        {
            animator.SetBool("Running", value);
        }
    }

    public bool Grounded
    {
        get
        {
            return animator.GetBool("Grounded");
        }
        set
        {
            animator.SetBool("Grounded", value);
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
        Grounded = false;
    }
}
