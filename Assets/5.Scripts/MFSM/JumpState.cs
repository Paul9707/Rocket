using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(Zombie zombie) : base(zombie) { }

    #region Private 변수 
    #endregion
    public override void OnStateEnter()
    {
        if (zombie.jumpCool == false && zombie.canJump == true)
        {
            zombie.rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse); // 점프 힘을 추가
            if (zombie.rb.velocity.y > 5f)
            {
               
                zombie.rb.velocity = new Vector2(zombie.rb.velocity.x, 5f); // 최대 점프 속도 제한
            }
        }
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }




}
