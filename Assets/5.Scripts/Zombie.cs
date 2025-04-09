using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    /// <summary>
    /// 좀비의 상태를 정의하는 열거형
    /// </summary>
    private enum State
    {
        Spawn,
        Move,
        Attack,
        Die
    }

    #region Private 변수
    private Vector3 moveInput; // 이동 입력값
    private bool isMove = true; // 이동 가능한지 여부
    private bool canAttack = false; // 공격 가능한지 여부

    private State currentState; // 현재 상태
    private StateMachine stateMachine; // 상태 머신
    private SpawnState spawnState; // 생성 상태
    private MoveState moveState; // 이동 상태
    private AttackState attackState; // 공격 상태
    
    #endregion

    #region Public 변수
    public float moveSpeed = 3.0f; // 이동 속도
    public Animator anim; // zombie의 애니메이터
    public SpriteRenderer spriteRenderer; // 스프라이트 렌더러
    public int laneNumber; // 좀비의 레인 번호
    #endregion

    void Awake()
    {
        // 상태 머신 초기화
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

            Debug.Log("좀비가 충돌했습니다.");
        }
    }

    /// <summary>
    /// 공격 가능한지 여부를 확인하는 함수
    /// </summary>
    public void CheckAttack()
    {
        canAttack = true;
    }
    
}
