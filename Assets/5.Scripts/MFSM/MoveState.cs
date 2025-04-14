using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(Zombie zombie) : base(zombie) { }

    #region Private 변수
    private Animator anim; // zombie의 애니메이터
    private Vector3 moveInput = new Vector3(-1, 0, 0); // 이동 입력값
    #endregion

    public override void OnStateEnter()
    {
        // 애니메이터가 없다면 애니메이터를 가져온다.
        if (anim == null)
        {
            anim = zombie.anim;
        }
    }

    public override void OnStateUpdate()
    {
        ZombieMove();
    }

    public override void OnStateExit()
    {

    }

    /// <summary>
    /// 좀비를 이동시키는 로직이 담긴 함수
    /// </summary>
    private void ZombieMove()
    {
        zombie.transform.position += moveInput * zombie.moveSpeed * Time.deltaTime;

    }
}

