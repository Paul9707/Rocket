using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    /// <summary>
    /// ������ ���¸� �����ϴ� ������
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

    #region Private ����
    private Vector3 moveInput; // �̵� �Է°�
    private Vector3 currentPosition; // ���� ��ġ
    private bool isMove = true; // �̵� �������� ����
    private bool canAttack = false; // ���� �������� ����
    private bool pullBack = false; // �ڷ� �о�� ����
    private bool onZombie = false; // ���� �����ֳ� ����


    private State currentState; // ���� ����
    private StateMachine stateMachine; // ���� �ӽ�
    private IdleState idleState; // ���� ����
    private MoveState moveState; // �̵� ����
    private JumpState jumpState; // ���� ����
    private AttackState attackState; // ���� ����
    private PullBackState pullBackState; // �ڷ� �о�� ����

    private WaitForSeconds waitTime = new WaitForSeconds(0.5f); // ��� �ð�
    private WaitForSeconds pullBackTime = new WaitForSeconds(0.5f); // ��� �ð�
    private WaitForSeconds jumpCoolTime = new WaitForSeconds(2.0f); // ���� ��Ÿ��
    private IEnumerator JumpCR; // ���� ��Ÿ�� �ڷ�ƾ
    private IEnumerator PullBackCR; // �ڷ� �о�� �ڷ�ƾ
    #endregion

    #region Public ����
    public Rigidbody2D rb; // ������ ������ٵ�
    public SpriteRenderer[] spriteRenderer; // ��������Ʈ ������
    public Animator anim; // zombie�� �ִϸ�����

    public float moveSpeed = 3.0f; // �̵� �ӵ�
    public int laneNumber; // ������ ���� ��ȣ

    public bool jumpCool = false; // ���� ��Ÿ�� ����
    public bool canJump = true; // ������ �������� ����
    #endregion

    void Awake()
    {
        // ���� �ӽ� �ʱ�ȭ
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        jumpState = new JumpState(this);
        attackState = new AttackState(this);
        pullBackState = new PullBackState(this);
        stateMachine = new StateMachine(idleState);
        rb = GetComponent<Rigidbody2D>();
        moveInput = new Vector3(1, 0, 0);
        currentState = State.Idle; // �ʱ� ���¸� Idle�� ����
    }
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        laneNumber = Random.Range(1, 4); // 1~3 ������ ������ ���� ��ȣ�� ����
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
    /// ���°� ����� �� ȣ��Ǵ� �Լ�
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
                    currentPosition = transform.position; // ���� ��ġ ����
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
    /// �浹�� �����ϴ� �̺�Ʈ �Լ�
    /// </summary>
    /// <param name="collision">�浹ü</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero")) // �÷��̾�� �浹���� ���
        {
            if (pullBack == false)
            {
                canAttack = true; // ���� ����
            }
            isMove = false; // �̵� �Ұ���
            canJump = false; // ���� �Ұ���
            GameManager.Instance.ZombieHit(); // ���� �浹���� �� ȣ��Ǵ� �̺�Ʈ
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            onZombie = false; // ���� �ٴڿ� ����
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero")) // �÷��̾�� �浹 ���� ���
        {
            canAttack = true; // ���� ����
            isMove = false; // �̵� �Ұ���
            canJump = false; // ���� �Ұ���
            GameManager.Instance.ZombieHit();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero")) // �÷��̾�� �浹�� ������ ���
        {
            canAttack = false; // ���� �Ұ���
            isMove = true; // �̵� ����
        }
    }


    /// <summary>
    /// �ڽ��� ���ο� �´� ���̾ �����ϴ� �Լ�
    /// </summary>
    /// <param name="laneNumber">�ڽ��� ����</param>
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
    /// ���� �տ� ��ֹ��� �ִ��� Ȯ�� �� �о�⸦ �����ϴ� �Լ�
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
                if (pullBack == true) // �ڷΰ��� �� �տ� ���� �ִٸ� �ڷΰ��⸦ ���ߴ� ����
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
                StartCoroutine(PullBackCR); // �ڷ� �о�� ��Ÿ�� ����
                PullBackCR = null; // �ڷ�ƾ ����
                if (gameObject.transform.position.x < currentPosition.x + 1)
                {
                    PullBackCR = PullBackTarget();
                    StartCoroutine(PullBackCR); // ��ǥ�������� �о�� ����
                    PullBackCR = null; // �ڷ�ƾ ����
                }
                else
                {
                    pullBack = false;
                }
            }
        }

    }

    /// <summary>
    /// ���� �Ӹ����� ��ֹ��� �ִ��� Ȯ���ϴ� �Լ�
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
    /// ���� �Ʒ��� �����ߴ��� Ȯ���ϴ� �Լ�
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
    ///  ��Ÿ���� �����ϴ� �Լ�
    /// </summary>
    private void JumpCoolTime()
    {
        if (JumpCR == null)
        {
            JumpCR = CoolTimeChecker();
            StartCoroutine(JumpCR); // ��Ÿ�� ����
            JumpCR = null; // �ڷ�ƾ ����
        }

    }

    /// <summary>
    /// ��Ÿ���� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="target">�ش罺ų ��Ÿ�� ����</param>
    /// <returns></returns>
    private IEnumerator CoolTimeChecker()
    {
        jumpCool = true; // ��Ÿ�� ����
        yield return jumpCoolTime; // ��� �ð�
        jumpCool = false; // ��Ÿ�� ����
    }

    /// <summary>
    /// �о�� ���ӽð��� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator PullBackCool()
    {
        yield return pullBackTime; // ��� �ð�

    }

    /// <summary>
    /// ��ǥ�������� �ڷΰ��⸦ �۵��ϰ� �ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator PullBackTarget()
    {
        while (gameObject.transform.position.x < currentPosition.x + 0.4f)
        {
            yield return null;
        }
        pullBack = false; // �о�� ����
    }
}
