using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(Zombie zombie) : base(zombie) { }

    public override void OnStateEnter()
    {
        zombie.rb.mass = 0.5f; // 좀비의 질량을 0.5로 설정
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        zombie.rb.mass = 1f; // 좀비의 질량을 1로 초기화
    }

}
