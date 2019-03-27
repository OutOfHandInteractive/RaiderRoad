using UnityEngine;
using System.Collections;

public class EnemyAnimator
{
    public Animator animator;

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
