using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBG : MonoBehaviour
{

    #region Private 변수
    private Vector3 moveInput = new Vector3(1, 0, 0);// 이동 입력값
    private IEnumerator MoveCoroutine; // 코루틴 변수
    #endregion
    #region Public 변수
    public float setY; // Y축 위치
    #endregion
    void Start()
    {
        GameManager.Instance.OnZombieHit += StopMove; // 좀비가 충돌했을 때 호출되는 이벤트에 StopMove 함수 등록
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
    /// 배경 이동을 멈추는 함수
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
    /// 배경 이동을 시작하는 함수
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
