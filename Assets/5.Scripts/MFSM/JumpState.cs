using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(Zombie zombie) : base(zombie) { }

    #region Private ���� 
    #endregion
    public override void OnStateEnter()
    {
        if (zombie.jumpCool == false && zombie.canJump == true)
        {
            zombie.rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse); // ���� ���� �߰�
            if (zombie.rb.velocity.y > 5f)
            {
               
                zombie.rb.velocity = new Vector2(zombie.rb.velocity.x, 5f); // �ִ� ���� �ӵ� ����
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
