using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Private 변수
    [SerializeField] private int objectCount = 20; // 풀에 생성할 오브젝트 수
    private Queue<GameObject> objectQueue; // 오브젝트 풀
    #endregion

    #region Public 변수
    public GameObject objectPrefab; // 풀에 생성할 오브젝트 프리팹
    #endregion

    private void Awake()
    {
        objectQueue = new Queue<GameObject>();
    }

    void Start()
    {
       CreateObject(objectCount);
    }

    /// <summary>
    /// 풀에 오브젝트를 생성하는 함수
    /// </summary>
    private void CreateObject(int count)
    {
        for (int i = 0; i < objectCount; i++)
        {
            GameObject targetObj = Instantiate(objectPrefab);
            targetObj.SetActive(false);
            objectQueue.Enqueue(targetObj);
        }
    }

    /// <summary>
    /// 풀에 있는 오브젝트를 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        // 풀에 오브젝트가 없으면 새로 생성
        if (objectQueue.Count == 0)
        {
            CreateObject(6);
        }

        GameObject targetObj = objectQueue.Dequeue();
        targetObj.SetActive(true);
        return targetObj;
    }

    /// <summary>
    /// 꺼낸 오브젝트를 다시 풀안으로 반환하는 함수
    /// </summary>
    /// <param name="targetObj">들어올 오브젝트</param>
    public void ReturnObject(GameObject targetObj)
    {
        targetObj.SetActive(false);
        objectQueue.Enqueue(targetObj);
    }
}
