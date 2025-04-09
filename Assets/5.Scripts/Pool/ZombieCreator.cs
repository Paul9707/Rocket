using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{

    #region Private ����
    private ObjectPool objectPool; // ������Ʈ Ǯ
    private WaitForSeconds waitTime = new WaitForSeconds(1.0f); // ��� �ð�
    #endregion
    #region Public ����
    public bool isSpawn = true; // ���� ���� ����
    #endregion
    
    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
      
        StageStart();
    }


    public void StageStart()
    {
        StartCoroutine(SpawnZombie());
    }

    private IEnumerator SpawnZombie()
    {
        while (isSpawn)
        { 
            GameObject zombie = objectPool.GetObject();
            yield return waitTime;
        }
    }
}
