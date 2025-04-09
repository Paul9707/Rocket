using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance; // 싱글톤 인스턴스

    #region Private 변수
    #endregion
    #region Public 변수
    public int stageCount; // 현재 스테이지 번호
    public event Action OnStageStart; // 스테이지 시작 이벤트
    #endregion
    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this; // 인스턴스가 null이면 현재 객체를 인스턴스로 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 현재 객체를 파괴
        }
    }

    private void Start()
    {
        stageCount = 1; // 초기 스테이지 번호 설정
    }
    /// <summary>
    /// 스테이지 시작시 이벤트 호출
    /// </summary>
    public void StageStart()
    {
        OnStageStart?.Invoke();
    }
}
