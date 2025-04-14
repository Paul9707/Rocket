using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{

    #region Private ����
    private IEnumerator MoveCoroutine; // �ڷ�ƾ ����
    #endregion
    #region Public ����
    #endregion
    void Start()
    {
        GameManager.Instance.OnZombieHit += StopMove; // ���� �浹���� �� ȣ��Ǵ� �̺�Ʈ�� StopMove �Լ� ���
        StartMove(); // ���� ȸ�� ����
    }


    private IEnumerator MoveWheel()
    {
        // ���� ȸ�� �ִϸ��̼�
        while (true)
        {
            transform.Rotate(Vector3.back * 5f);
            yield return null;
        }
    }


    public void StopMove()
    {
        if (MoveCoroutine != null)
        {
            StopCoroutine(MoveCoroutine);
            MoveCoroutine = null; // �ڷ�ƾ�� ���߰� null�� ����
        }
    }

    public void StartMove()
    {
        if (MoveCoroutine != null)
        {
            StopCoroutine(MoveCoroutine);
        }
        MoveCoroutine = MoveWheel();
        StartCoroutine(MoveCoroutine);
    }

}
