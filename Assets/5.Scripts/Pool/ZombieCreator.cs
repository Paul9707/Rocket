using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{

    #region Private 변수
    private ObjectPool objectPool; // 오브젝트 풀
    private WaitForSeconds waitTime = new WaitForSeconds(1.0f); // 대기 시간
    #endregion
    #region Public 변수
    public bool isSpawn = true; // 좀비 생성 여부
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
