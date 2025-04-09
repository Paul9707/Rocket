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
    public int stageCount; // ���� �������� ��ȣ
    public event Action OnStageStart; // �������� ���� �̺�Ʈ
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

    private void Start()
    {
        stageCount = 1; // �ʱ� �������� ��ȣ ����
    }
    /// <summary>
    /// �������� ���۽� �̺�Ʈ ȣ��
    /// </summary>
    public void StageStart()
    {
        OnStageStart?.Invoke();
    }
}
