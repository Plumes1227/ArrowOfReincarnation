using System.Collections;
using UnityEngine;

/// <summary>
/// 自動禁用腳本,與對象池搭配,可自動禁用或手動禁用對象
/// </summary>
public class AutoDeactivate : MonoBehaviour
{
    [Header("---- 是否使用自動禁用 ----")]
    [SerializeField] bool useAutoDeactivate = true;    //是否使用"自動禁用" 預設是
    [Header("---- 存活時間 ----")]
    [SerializeField] float lifetime = 3f;       //存活時間
    WaitForSeconds waitLifetime;        //禁用等待時間(存活時間)

    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }

    void OnEnable()
    {
        if(!useAutoDeactivate) return; //判斷是否使用自動禁用
        StartCoroutine(DeactivateCoroutine());
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 禁用協程,存活時間到後禁用自身
    /// </summary>
    /// <returns></returns>
    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifetime;
        gameObject.SetActive(false);        
    }
    /// <summary>
    /// 禁用協程,改成time秒後禁用自身
    /// </summary>
    /// <returns></returns>
    IEnumerator DeactivateCoroutine(float time)
    {
        lifetime = time;
        yield return waitLifetime;

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 立即禁用自身API
    /// </summary>
    public void InDeactivate()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 延遲time秒後禁用自身
    /// </summary>
    /// <param name="time">延遲時間參數</param>
    public void InDeactivate(float time)
    {
        StartCoroutine(DeactivateCoroutine(time));
    }    
}

