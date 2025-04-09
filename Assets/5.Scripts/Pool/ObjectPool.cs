using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Private ����
    [SerializeField] private int objectCount = 20; // Ǯ�� ������ ������Ʈ ��
    private Queue<GameObject> objectQueue; // ������Ʈ Ǯ
    #endregion

    #region Public ����
    public GameObject objectPrefab; // Ǯ�� ������ ������Ʈ ������
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
    /// Ǯ�� ������Ʈ�� �����ϴ� �Լ�
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
    /// Ǯ�� �ִ� ������Ʈ�� ������ �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        // Ǯ�� ������Ʈ�� ������ ���� ����
        if (objectQueue.Count == 0)
        {
            CreateObject(6);
        }

        GameObject targetObj = objectQueue.Dequeue();
        targetObj.SetActive(true);
        return targetObj;
    }

    /// <summary>
    /// ���� ������Ʈ�� �ٽ� Ǯ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="targetObj">���� ������Ʈ</param>
    public void ReturnObject(GameObject targetObj)
    {
        targetObj.SetActive(false);
        objectQueue.Enqueue(targetObj);
    }
}
