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
    private float force = 5.0f; // �о�� ��
    #endregion
    public override void OnStateEnter()
    {
       
    }

    public override void OnStateUpdate()
    {
        zombie.rb.MovePosition(zombie.rb.position + Vector2.right * force * Time.deltaTime); // �о��
    }
    public override void OnStateExit()
    {

    }


}
