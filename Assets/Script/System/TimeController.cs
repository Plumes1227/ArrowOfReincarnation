using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f,1f)] float bulletTimeScale = 0.1f;
    
    [SerializeField] GameObject _eff_SpeedLine;
    float defaultFixedDeltatime;
    float timeScaleBeforePause;
    float t;

    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltatime = Time.fixedDeltaTime;
    }

    void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
    }
    void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
    }
    private void OnGameOver()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = defaultFixedDeltatime * Time.timeScale; 
        StopAllCoroutines();
    }

    /// <summary>
    /// 暫停時間,儲存當前時間流速
    /// </summary>
    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 恢復暫停,還原儲存的時間流速
    /// </summary>
    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;        
    }

    /// <summary>
    /// 慢進,依照傳入的值,緩進到慢速時間流速
    /// </summary>
    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }

    /// <summary>
    /// 慢出,依照傳入的值，慢出到原始時間流速
    /// </summary>
    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    /// <summary>
    /// 子彈時間，進入慢速狀態，並在一定時間後回到原本狀態
    /// </summary>
    /// <param name="inDuration">慢進時間</param>
    /// <param name="outDuration">慢出時間</param>
    /// <param name="keepingDuration">維持時長</param>
    public void BulletTime(float inDuration, float outDuration, float keepingDuration)
    {
        _eff_SpeedLine.SetActive(true);
        StartCoroutine(slowInKeepAndOutDuration(inDuration, keepingDuration, outDuration));
    }

    /// <summary>
    /// 緩入緩出協程，依照傳入的值進行，緩入時間>維持時間>緩出時間。
    /// </summary>
    /// <param name="inDuration">緩入時間</param>
    /// <param name="keepingDuration">維持時間</param>
    /// <param name="outDuration">緩出時間</param>
    IEnumerator slowInKeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    /// <summary>
    /// 緩速協程，依照傳入的值，緩進到慢速時間流速並維持。
    /// </summary>
    IEnumerator SlowInCoroutine(float duration)
    {        
        t = 0f;
        while (t < 1f)
        {
            if(GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltatime * Time.timeScale;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 緩出協程，依照傳入的值，緩出到原始時間流速。
    /// </summary>
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;
        while (t < 1f)
        {
            if(GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltatime * Time.timeScale;                
            }
            yield return null;
        }
    }
}
