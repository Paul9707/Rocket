using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{

    #region Private ����
    private ObjectPool objectPool; // ������Ʈ Ǯ
    private WaitForSeconds waitTime = new WaitForSeconds(0.5f); // ��� �ð�
    private int spawnCount = 0; // ������ ���� ��
    #endregion
    #region Public ����
    public bool isSpawn = true; // ���� ���� ����
    #endregion

    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
        StartCoroutine(SpawnObject());
    }
    /// <summary>
    /// Ǯ�ȿ� �ִ� ������Ʈ�� ������ �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnObject()
    {
        while (spawnCount < 50)
        {
            GameObject zombie = objectPool.GetObject();
            zombie.transform.position = gameObject.transform.position;
            spawnCount++;
            yield return waitTime;
        }
    }
}
