using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    public AttackState(Zombie zombie) : base(zombie) { }

    #region Private 변수
    private Animator anim; // zombie의 애니메이터
    #endregion 


    public override void OnStateEnter()
    {
        if (anim == null)
        { 
            anim = zombie.anim;
        }
        // 공격 애니메이션 재생
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
