using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance; // �̱��� �ν��Ͻ�

    #region Private ����
    #endregion
    #region Public ����
    public Action OnZombieHit; // ���� �浹�� �� ȣ��Ǵ� �̺�Ʈ

    #endregion
    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this; // �ν��Ͻ��� null�̸� ���� ��ü�� �ν��Ͻ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���� ��ü�� �ı�
        }
    }

    /// <summary>
    /// �ܺο��� ���� �浹���� �� ȣ���ϴ� �Լ�
    /// </summary>
    public void ZombieHit()
    {
        OnZombieHit?.Invoke(); 
    }


}
