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
    public Action OnZombieHit; // 좀비가 충돌할 때 호출되는 이벤트

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

    /// <summary>
    /// 외부에서 좀비가 충돌했을 때 호출하는 함수
    /// </summary>
    public void ZombieHit()
    {
        OnZombieHit?.Invoke(); 
    }


}
