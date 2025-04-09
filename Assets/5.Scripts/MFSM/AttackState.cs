using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    public AttackState(Zombie zombie) : base(zombie) { }

    #region Private ����
    private Animator anim; // zombie�� �ִϸ�����
    #endregion 


    public override void OnStateEnter()
    {
        if (anim == null)
        { 
            anim = zombie.anim;
        }
        // ���� �ִϸ��̼� ���
        anim.SetBool("IsAttacking", true);    
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateExit()
    {
        anim.SetBool("IsAttacking", false);
    }
}
