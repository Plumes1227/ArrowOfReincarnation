using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    
    [SerializeField] ScoreCounterUi scoreCounterUi;
    [SerializeField] GameObject enemyPrefabs;
    [SerializeField] int maxEnemyAmonut;
    public int killEnemyAmount;
    bool isOnBuffTime;
    
    [SerializeField] bool isOnStart;
    WaitForSeconds delayGenerateEnemyTime;
    WaitForSeconds veryShortTime;
    [SerializeField] List<GameObject> enemyList;     //敵人列表

    protected override void Awake()
    {
        base.Awake();
        delayGenerateEnemyTime = new WaitForSeconds(1.6f);
        veryShortTime =  new WaitForSeconds(0.01f);
        enemyList = new List<GameObject>();
    }
    
    void Start()
    {
        if (isOnStart)
        {
            isOnStart = false;
            scoreCounterUi.TimeCountingStart();
            StartCoroutine(nameof(ReleaseEnemyCoroutine));
        }
    }

    public void ReleaseEnemy()
    {
        if(GameManager.GameState != GameState.Playing) return;
        //if(killEnemyAmount >= maxEnemyAmonut) return;
        StartCoroutine(nameof(ReleaseEnemyCoroutine));
    }

    IEnumerator ReleaseEnemyCoroutine()
    {        
        yield return new WaitForSeconds(0.4f);
        if(isOnBuffTime) 
        {
            yield return delayGenerateEnemyTime;
            isOnBuffTime = false;
        }
        if(GameManager.GameState == GameState.Playing && enemyList.Count <= maxEnemyAmonut)
        {
            enemyList.Add(PoolManager.Release(enemyPrefabs));
            if(killEnemyAmount <= maxEnemyAmonut-2)
            {
                enemyList.Add(PoolManager.Release(enemyPrefabs));
            }
        }
    }

    /// <summary>
    /// 刪除列表中的敵人
    /// </summary>
    /// <param name="enemy">要刪除的敵人</param>
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);

    /// <summary>
    /// 技能效果，凍結所有敵人
    /// </summary>
    public void FreezeAllEnemy()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<Enemy>().Freeze();
        }
        //StartCoroutine(nameof(FreezeAllEnemyCoroution));
    }
    IEnumerator FreezeAllEnemyCoroution()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<Enemy>().Freeze();
            yield return veryShortTime;
        }
    }

    /// <summary>
    /// 技能效果，殺死所有敵人
    /// </summary>
    public void RemoveAllEnemy()
    {
        if(isOnBuffTime) return;
        isOnBuffTime = true;
        StartCoroutine(nameof(RemoveAllEnemyCoroution));
    }
    IEnumerator RemoveAllEnemyCoroution()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<Enemy>().Death();
            yield return veryShortTime;
        }
    }
}
