using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{

    #region Private 변수
    private IEnumerator MoveCoroutine; // 코루틴 변수
    #endregion
    #region Public 변수
    #endregion
    void Start()
    {
        GameManager.Instance.OnZombieHit += StopMove; // 좀비가 충돌했을 때 호출되는 이벤트에 StopMove 함수 등록
        StartMove(); // 바퀴 회전 시작
    }


    private IEnumerator MoveWheel()
    {
        // 바퀴 회전 애니메이션
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
            MoveCoroutine = null; // 코루틴을 멈추고 null로 설정
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
