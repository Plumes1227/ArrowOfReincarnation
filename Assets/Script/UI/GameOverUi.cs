using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUi : MonoBehaviour
{
    [SerializeField] Text timeScoreText;
    [SerializeField] Text killScoreText;

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
        timeScoreText.text = ScoreManager.Instance.SurvivetimeText;
        killScoreText.text = ScoreManager.Instance.KillAmount.ToString();
    }
}
