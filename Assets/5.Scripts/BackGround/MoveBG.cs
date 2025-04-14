using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBG : MonoBehaviour
{

    #region Private ����
    private Vector3 moveInput = new Vector3(1, 0, 0);// �̵� �Է°�
    private IEnumerator MoveCoroutine; // �ڷ�ƾ ����
    #endregion
    #region Public ����
    public float setY; // Y�� ��ġ
    #endregion
    void Start()
    {
        GameManager.Instance.OnZombieHit += StopMove; // ���� �浹���� �� ȣ��Ǵ� �̺�Ʈ�� StopMove �Լ� ���
        StartMove(); 
    }

    private IEnumerator MoveBackGround()
    {
        while (true)
        {
            gameObject.transform.position -= moveInput * 5f * Time.deltaTime;

            if (gameObject.transform.position.x <= -37.9f)
            {
                gameObject.transform.position = new Vector3(75.6f, setY, 0);
            }

            yield return null;
        }
    }

    /// <summary>
    /// ��� �̵��� ���ߴ� �Լ�
    /// </summary>
    public void StopMove()
    {
        if (MoveCoroutine != null)
        {
            StopCoroutine(MoveCoroutine);
            MoveCoroutine = null;
        }
    }

    /// <summary>
    /// ��� �̵��� �����ϴ� �Լ�
    /// </summary>
    public void StartMove()
    {
        if (MoveCoroutine != null)
        {
            StopCoroutine(MoveCoroutine);
        }
        MoveCoroutine = MoveBackGround();
        StartCoroutine(MoveCoroutine);  
    }
}
