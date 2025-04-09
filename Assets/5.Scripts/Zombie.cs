using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    /// <summary>
    /// ������ ���¸� �����ϴ� ������
    /// </summary>
    private enum State
    {
        Spawn,
        Move,
        Attack,
        Die
    }

    #region Private ����
    private Vector3 moveInput; // �̵� �Է°�
    private bool isMove = true; // �̵� �������� ����
    private bool canAttack = false; // ���� �������� ����

    private State currentState; // ���� ����
    private StateMachine stateMachine; // ���� �ӽ�
    private SpawnState spawnState; // ���� ����
    private MoveState moveState; // �̵� ����
    private AttackState attackState; // ���� ����
    
    #endregion

    #region Public ����
    public float moveSpeed = 3.0f; // �̵� �ӵ�
    public Animator anim; // zombie�� �ִϸ�����
    public SpriteRenderer spriteRenderer; // ��������Ʈ ������
    public int laneNumber; // ������ ���� ��ȣ
    #endregion

    void Awake()
    {
        // ���� �ӽ� �ʱ�ȭ
        spawnState = new SpawnState(this);
        moveState = new MoveState(this);
        attackState = new AttackState(this);
        stateMachine = new StateMachine(spawnState);
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (isMove == true)
        {
            ZombieMove();
        }

    }


    public void OnStateChanged()
    {
        switch (currentState)
        { 
            case State.Spawn:
                stateMachine.ChangeState(moveState);
                break;
            case State.Move:
                if (canAttack == true)
                { 
                    stateMachine.ChangeState(attackState);
                }
                break;
                case State.Attack:
                break;

        }
    }




    private void ZombieMove()
    {
        moveInput = new Vector3(1, 0, 0);
        gameObject.transform.position -= moveInput * moveSpeed * Time.deltaTime;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            isMove = false;

            Debug.Log("���� �浹�߽��ϴ�.");
        }
    }

    /// <summary>
    /// ���� �������� ���θ� Ȯ���ϴ� �Լ�
    /// </summary>
    public void CheckAttack()
    {
        canAttack = true;
    }
    
}
