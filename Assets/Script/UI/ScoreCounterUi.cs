using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounterUi : MonoBehaviour
{
    static Text TimeCounterUi;
    static Text KillCounterUi;
    int hourTime, minTime, secTime;
    int totalTime;
    WaitForSeconds waitTime = new WaitForSeconds(1f);

   void Awake()
    {
        TimeCounterUi = transform.GetChild(0).GetComponent<Text>();
        KillCounterUi = transform.GetChild(1).GetComponent<Text>();
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
        totalTime = minTime*60 +secTime;     
        ScoreManager.Instance.SurviveTotalTimeSave(totalTime, TimeCounterUi.text);
        
    }

    void Start()
    {
        ScoreManager.Instance.SaveScoreData(); //換地方
    }
    public void TimeCountingStart()
    {
        StartCoroutine(nameof(ClockUiCoroutine));
    }

    IEnumerator ClockUiCoroutine()
    {
        while(true)
        {
            secTime ++;            
            //TimeCounterUi.text = string.Format("{0} {1}:{2}:{3}", "Time " , hourTime.ToString("00"),minTime.ToString("00"), secTime.ToString("00"));      小時版本
            TimeCounterUi.text = string.Format("{0} {1}:{2}", "Time " , minTime.ToString("00"), secTime.ToString("00"));
            if(secTime >=60)
            {
                secTime = 0;
                minTime++;
            }
            if(minTime >=60)
            {
                minTime = 0;
                hourTime++;
            }
            yield return waitTime;
        }
    }
    public static void UpdateUi(int amount) => KillCounterUi.text = "Kill  "+ amount.ToString();
    
    
}

