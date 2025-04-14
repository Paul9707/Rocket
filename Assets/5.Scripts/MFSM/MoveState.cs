using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(Zombie zombie) : base(zombie) { }

    #region Private ����
    private Animator anim; // zombie�� �ִϸ�����
    private Vector3 moveInput = new Vector3(-1, 0, 0); // �̵� �Է°�
    #endregion

    public override void OnStateEnter()
    {
        // �ִϸ����Ͱ� ���ٸ� �ִϸ����͸� �����´�.
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
    /// ���� �̵���Ű�� ������ ��� �Լ�
    /// </summary>
    private void ZombieMove()
    {
        zombie.transform.position += moveInput * zombie.moveSpeed * Time.deltaTime;

    }
}

