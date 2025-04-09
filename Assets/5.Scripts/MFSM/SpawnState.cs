using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : BaseState
{
    public SpawnState(Zombie zombie) : base(zombie) { }

    public override void OnStateEnter()
    {
 
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        // 좀비의 레인 번호에 따라 태그와 물리연산 우선순위를 설정 및 시작 위치를 설정
        switch (zombie.laneNumber)
        {
            case 1:
                zombie.tag = "Lane1";
                zombie.spriteRenderer.sortingLayerID = SortingLayer.NameToID("Lane1");
                IgnorLayer(1);
                SetPosition(1);
                break;
            case 2:
                zombie.tag = "Lane2";
                zombie.spriteRenderer.sortingLayerID = SortingLayer.NameToID("Lane2");
                IgnorLayer(2);
                SetPosition(2);
                break;
            case 3:
                zombie.tag = "Lane3";
                zombie.spriteRenderer.sortingLayerID = SortingLayer.NameToID("Lane3");
                IgnorLayer(3);
                SetPosition(3);
                break;
        }
    }

    /// <summary>
    /// 다른 레인의 위치한 좀비들과의 물리연산을 무시하기 위한 함수
    /// </summary>
    /// <param name="laneNumber">자신의 레인</param>
    private void IgnorLayer(int laneNumber)
    {
        switch (laneNumber)
        {
            case 1:
                zombie.gameObject.layer = LayerMask.NameToLayer("Lane1");
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lane1"), LayerMask.NameToLayer("Lane2"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lane1"), LayerMask.NameToLayer("Lane3"), true);
                break;
            case 2:
                zombie.gameObject.layer = LayerMask.NameToLayer("Lane2");
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lane2"), LayerMask.NameToLayer("Lane1"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lane2"), LayerMask.NameToLayer("Lane3"), true);
                break;
            case 3:
                zombie.gameObject.layer = LayerMask.NameToLayer("Lane3");
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lane3"), LayerMask.NameToLayer("Lane1"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Lane3"), LayerMask.NameToLayer("Lane2"), true);
                break;
        }
    }

    /// <summary>
    /// 좀비가 레인별 시작위치를 설정하는 함수
    /// </summary>
    /// <param name="laneNumber">자신의 레인</param>
    private void SetPosition(int laneNumber)
    {
        switch (laneNumber)
        {
            case 1:
                zombie.transform.position = new Vector3(20.0f, -3.0f, 0);
                break;
            case 2:
                zombie.transform.position = new Vector3(20.0f, -3.3f, 0);
                break;
            case 3:
                zombie.transform.position = new Vector3(20.0f, -3.6f, 0);
                break;
        }
    }
}
