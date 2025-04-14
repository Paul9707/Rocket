using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{

    #region Private 변수
    private ObjectPool objectPool; // 오브젝트 풀
    private WaitForSeconds waitTime = new WaitForSeconds(0.5f); // 대기 시간
    private int spawnCount = 0; // 생성된 좀비 수
    #endregion
    #region Public 변수
    public bool isSpawn = true; // 좀비 생성 여부
    #endregion

    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
        StartCoroutine(SpawnObject());
    }
    /// <summary>
    /// 풀안에 있는 오브젝트를 가져와 생성하는 코루틴
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
