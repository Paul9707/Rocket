using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullBackState : BaseState
{

    public PullBackState(Zombie zombie) : base(zombie)
    {
        this.zombie = zombie;
    }

    #region
    private float force = 5.0f; // 밀어내는 힘
    #endregion
    public override void OnStateEnter()
    {
       
    }

    public override void OnStateUpdate()
    {
        zombie.rb.MovePosition(zombie.rb.position + Vector2.right * force * Time.deltaTime); // 밀어내기
    }
    public override void OnStateExit()
    {

    }


}
