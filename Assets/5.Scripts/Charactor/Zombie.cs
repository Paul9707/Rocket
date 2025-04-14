using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    /// <summary>
    /// 좀비의 상태를 정의하는 열거형
    /// </summary>
    private enum State
    {
        Idle,
        Move,
        Jump,
        Attack,
        PullBack,
        Die
    }

    #region Private 변수
    private Vector3 moveInput; // 이동 입력값
    private Vector3 currentPosition; // 현재 위치
    private bool isMove = true; // 이동 가능한지 여부
    private bool canAttack = false; // 공격 가능한지 여부
    private bool pullBack = false; // 뒤로 밀어내기 여부
    private bool onZombie = false; // 좀비가 위에있나 여부


    private State currentState; // 현재 상태
    private StateMachine stateMachine; // 상태 머신
    private IdleState idleState; // 정지 상태
    private MoveState moveState; // 이동 상태
    private JumpState jumpState; // 점프 상태
    private AttackState attackState; // 공격 상태
    private PullBackState pullBackState; // 뒤로 밀어내기 상태

    private WaitForSeconds waitTime = new WaitForSeconds(0.5f); // 대기 시간
    private WaitForSeconds pullBackTime = new WaitForSeconds(0.5f); // 대기 시간
    private WaitForSeconds jumpCoolTime = new WaitForSeconds(2.0f); // 점프 쿨타임
    private IEnumerator JumpCR; // 점프 쿨타임 코루틴
    private IEnumerator PullBackCR; // 뒤로 밀어내기 코루틴
    #endregion

    #region Public 변수
    public Rigidbody2D rb; // 좀비의 리지드바디
    public SpriteRenderer[] spriteRenderer; // 스프라이트 렌더러
    public Animator anim; // zombie의 애니메이터

    public float moveSpeed = 3.0f; // 이동 속도
    public int laneNumber; // 좀비의 레인 번호

    public bool jumpCool = false; // 점프 쿨타임 여부
    public bool canJump = true; // 점프가 가능한지 여부
    #endregion

    void Awake()
    {
        // 상태 머신 초기화
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        jumpState = new JumpState(this);
        attackState = new AttackState(this);
        pullBackState = new PullBackState(this);
        stateMachine = new StateMachine(idleState);
        rb = GetComponent<Rigidbody2D>();
        moveInput = new Vector3(1, 0, 0);
        currentState = State.Idle; // 초기 상태를 Idle로 설정
    }
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        laneNumber = Random.Range(1, 4); // 1~3 사이의 랜덤한 레인 번호를 생성
        switch (laneNumber)
        {
            case 1:
                gameObject.tag = "Lane1";
                for (int i = 0; i < spriteRenderer.Length; i++)
                {
                    spriteRenderer[i].sortingLayerID = SortingLayer.NameToID("Lane1");
                }
                IgnorLayer(laneNumber);
                break;
            case 2:
                gameObject.tag = "Lane2";
                for (int i = 0; i < spriteRenderer.Length; i++)
                {
                    spriteRenderer[i].sortingLayerID = SortingLayer.NameToID("Lane2");
                }
                IgnorLayer(laneNumber);
                break;
            case 3:
                gameObject.tag = "Lane3";
                for (int i = 0; i < spriteRenderer.Length; i++)
                {
                    spriteRenderer[i].sortingLayerID = SortingLayer.NameToID("Lane3");
                }
                IgnorLayer(laneNumber);
                break;
        }
    }
    void Update()
    {
        OnStateChanged();
        stateMachine.UpdateState();
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rb.position + new Vector2(-0.35f, 0.5f), Vector3.down / 5, new Color(0, 1, 0));
        Debug.DrawRay(rb.position + new Vector2(0, 1.1f), Vector3.left / 5, new Color(0, 1, 0));
        Debug.DrawRay(rb.position + new Vector2(-0.2f, 0f), Vector3.right / 5, new Color(0, 1, 0));

        CheckFront();
        CheckTop();
        CheckDown();
    }

    /// <summary>
    /// 상태가 변경될 때 호출되는 함수
    /// </summary>
    public void OnStateChanged()
    {
        switch (currentState)
        {
            case State.Idle:
                if (isMove == true)
                {
                    stateMachine.ChangeState(moveState);
                    currentState = State.Move;
                }
                if (isMove == false && canJump == true && jumpCool == false)
                {
                    stateMachine.ChangeState(jumpState);
                    currentState = State.Jump;
                    JumpCoolTime();
                }
                if (canAttack == true)
                {
                    stateMachine.ChangeState(attackState);
                    currentState = State.Attack;
                }
                break;
            case State.Move:
                if (canAttack == true)
                {
                    stateMachine.ChangeState(attackState);
                    currentState = State.Attack;
                }
                else if (canAttack == false && isMove == false)
                {
                    stateMachine.ChangeState(idleState);
                    currentState = State.Idle;
                }
                else if (isMove == false && canJump == true && jumpCool == false)
                {
                    stateMachine.ChangeState(jumpState);
                    currentState = State.Jump;
                    JumpCoolTime();
                }
                break;
            case State.Jump:
                if (isMove == true)
                {
                    stateMachine.ChangeState(moveState);
                    currentState = State.Move;
                }
                else if (canAttack == true)
                {
                    stateMachine.ChangeState(attackState);
                    currentState = State.Attack;
                }
                else if (isMove == false && canJump == false)
                {
                    stateMachine.ChangeState(idleState);
                    currentState = State.Idle;
                }
                break;
            case State.Attack:
                if (canAttack == false && isMove == true)
                {
                    stateMachine.ChangeState(moveState);
                    currentState = State.Move;
                }
                else if (pullBack == true && onZombie == false)
                {
                    currentPosition = transform.position; // 현재 위치 저장
                    stateMachine.ChangeState(pullBackState);
                    currentState = State.PullBack;
                }
                break;
            case State.PullBack:
                if (pullBack == false && isMove == false)
                {
                    stateMachine.ChangeState(idleState);
                    currentState = State.Idle;
                }
                else if (pullBack == false && isMove == true)
                {
                    stateMachine.ChangeState(moveState);
                    currentState = State.Move;
                }
                break;

        }
    }


    /// <summary>
    /// 충돌을 감지하는 이벤트 함수
    /// </summary>
    /// <param name="collision">충돌체</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero")) // 플레이어와 충돌했을 경우
        {
            if (pullBack == false)
            {
                canAttack = true; // 공격 가능
            }
            isMove = false; // 이동 불가능
            canJump = false; // 점프 불가능
            GameManager.Instance.ZombieHit(); // 좀비가 충돌했을 때 호출되는 이벤트
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            onZombie = false; // 좀비가 바닥에 있음
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero")) // 플레이어와 충돌 중일 경우
        {
            canAttack = true; // 공격 가능
            isMove = false; // 이동 불가능
            canJump = false; // 점프 불가능
            GameManager.Instance.ZombieHit();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero")) // 플레이어와 충돌이 끝났을 경우
        {
            canAttack = false; // 공격 불가능
            isMove = true; // 이동 가능
        }
    }


    /// <summary>
    /// 자신의 레인에 맞는 레이어를 설정하는 함수
    /// </summary>
    /// <param name="laneNumber">자신의 레인</param>
    private void IgnorLayer(int laneNumber)
    {
        switch (laneNumber)
        {
            case 1:
                gameObject.layer = LayerMask.NameToLayer("Lane1");
                break;
            case 2:
                gameObject.layer = LayerMask.NameToLayer("Lane2");
                break;
            case 3:
                gameObject.layer = LayerMask.NameToLayer("Lane3");
                break;
        }
    }


    /// <summary>
    /// 좀비가 앞에 장애물이 있는지 확인 및 밀어내기를 실행하는 함수
    /// </summary>
    private void CheckFront()
    {
        int layerMask = 1 << gameObject.layer;
        RaycastHit2D hit = Physics2D.Raycast(rb.position + new Vector2(-0.35f, 0.5f), Vector3.down, 0.2f, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == gameObject.layer)
            {
                isMove = false;
                if (pullBack == true) // 뒤로가기 중 앞에 좀비가 있다면 뒤로가기를 멈추는 조건
                {
                    pullBack = false;
                }
                {
                    pullBack = false;
                }
            }
        }
        else
        {
            isMove = true;
            if (pullBack == true && PullBackCR == null)
            {
                PullBackCR = PullBackCool();
                StartCoroutine(PullBackCR); // 뒤로 밀어내기 쿨타임 시작
                PullBackCR = null; // 코루틴 리셋
                if (gameObject.transform.position.x < currentPosition.x + 1)
                {
                    PullBackCR = PullBackTarget();
                    StartCoroutine(PullBackCR); // 목표지점까지 밀어내기 시작
                    PullBackCR = null; // 코루틴 리셋
                }
                else
                {
                    pullBack = false;
                }
            }
        }

    }

    /// <summary>
    /// 좀비 머리위에 장애물이 있는지 확인하는 함수
    /// </summary>
    private void CheckTop()
    {
        int layerMask = 1 << gameObject.layer;
        RaycastHit2D hit = Physics2D.Raycast(rb.position + new Vector2(0, 1.1f), Vector3.left, 0.2f, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == gameObject.layer)
            {
                canJump = false;
                if (canAttack == true && pullBack == false)
                {
                    pullBack = true;
                }
            }
        }
    }


    /// <summary>
    /// 좀비가 아래에 착지했는지 확인하는 함수
    /// </summary>
    private void CheckDown()
    {
        int layerMask = 1 << gameObject.layer;
        RaycastHit2D hit = Physics2D.Raycast(rb.position + new Vector2(-0.2f, 0f), Vector3.down, 0.2f, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == gameObject.layer && canAttack == false)
            {
                canJump = true;
            }
        }
    }



    /// <summary>
    ///  쿨타임을 시작하는 함수
    /// </summary>
    private void JumpCoolTime()
    {
        if (JumpCR == null)
        {
            JumpCR = CoolTimeChecker();
            StartCoroutine(JumpCR); // 쿨타임 시작
            JumpCR = null; // 코루틴 리셋
        }

    }

    /// <summary>
    /// 쿨타임을 시작하는 코루틴
    /// </summary>
    /// <param name="target">해당스킬 쿨타임 여부</param>
    /// <returns></returns>
    private IEnumerator CoolTimeChecker()
    {
        jumpCool = true; // 쿨타임 시작
        yield return jumpCoolTime; // 대기 시간
        jumpCool = false; // 쿨타임 종료
    }

    /// <summary>
    /// 밀어내기 지속시간을 설정하는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator PullBackCool()
    {
        yield return pullBackTime; // 대기 시간

    }

    /// <summary>
    /// 목표지점까지 뒤로가기를 작동하게 하는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator PullBackTarget()
    {
        while (gameObject.transform.position.x < currentPosition.x + 0.4f)
        {
            yield return null;
        }
        pullBack = false; // 밀어내기 종료
    }
}
