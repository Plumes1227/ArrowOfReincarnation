using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUiCounter : MonoBehaviour
{
    [Header("=== 玩家輸入控制 ===")]
    [SerializeField] PlayerInput playerInput;
    [Header("=== CANVAS ===")]
    [SerializeField] Canvas gamePlayCanvas;
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Canvas gameoverCanvas;
    [SerializeField] Canvas topScoreCanvas;
    [Header("=== 按鈕 ===目前0作用,為手動設定按鈕事件")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button topScoreButton;
    [SerializeField] Button backHomeButton;
    [Header("=== 音效控制 ===")]
    [SerializeField] Toggle bgmToggle;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] Slider sfxSlider;

    void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;
        GameManager.onGameOver += OnGameOver;
        bgmSlider.value = AudioManager.Instance.bgmSliderValue;
        sfxSlider.value = AudioManager.Instance.sfxSliderValue;
        bgmToggle.isOn = AudioManager.Instance.bgmToggleBool;
        sfxToggle.isOn = AudioManager.Instance.sfxToggleBool;
    }

    void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;
        GameManager.onGameOver -= OnGameOver;
        AudioManager.Instance.bgmToggleBool = bgmToggle.isOn;
        AudioManager.Instance.sfxToggleBool = sfxToggle.isOn;
        AudioManager.Instance.bgmSliderValue = bgmSlider.value;
        AudioManager.Instance.sfxSliderValue = sfxSlider.value;
    }
    private void Pause()
    {
        gamePlayCanvas.enabled = false;
        settingsCanvas.enabled = true;
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynanicUpdateMode();
        StopCoroutine(OnAudioAdjusCoroutine());
        StartCoroutine(OnAudioAdjusCoroutine());
    }
    void Unpause()
    {
        StopCoroutine(OnAudioAdjusCoroutine());
    }

    public void OnResumeButtonClick()
    {
        gamePlayCanvas.enabled = true;
        settingsCanvas.enabled = false;
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.Unpause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }
    public void OnTopScoreButtonClick()
    {        
        gameoverCanvas.enabled = false;
        topScoreCanvas.enabled = true;
        topScoreCanvas.GetComponent<TopScoreUiCounter>().UpdateHighScoreLeaderboard();
        topScoreCanvas.GetComponent<TopScoreUiCounter>().UpdatePlayScoreText();        
    }

    public void OnBackHomeButtonClick()
    {        
        settingsCanvas.enabled = false;
        gameoverCanvas.enabled = false;
        topScoreCanvas.enabled = false;
        SceneLoader.Instance.LoadGameStartScene();
    }

    //遊戲結束時
    private void OnGameOver()
    {
        gamePlayCanvas.enabled = false;
        gameoverCanvas.enabled = true;
        ScoreManager.Instance.SavePlayerScoreData();
    }

    public void Toggle_BGM()
    {
        if(bgmToggle.isOn)
        {
            AudioManager.Instance.AudioMixerObj.SetFloat("BGM",0);
        }else 
        {
            AudioManager.Instance.AudioMixerObj.SetFloat("BGM",-80);
        }
    }
    public void Toggle_SFX()
    {
        if(sfxToggle.isOn)
        {
            AudioManager.Instance.AudioMixerObj.SetFloat("SFX",0);
        }else 
        {
            AudioManager.Instance.AudioMixerObj.SetFloat("SFX",-80);
        }
    }

    IEnumerator OnAudioAdjusCoroutine()
    {
        while(true)
        {
            if(bgmToggle.isOn) AudioManager.Instance.AudioMixerObj.SetFloat("BGM", (bgmSlider.value * 20) -20);
            if(sfxToggle.isOn) AudioManager.Instance.AudioMixerObj.SetFloat("SFX", (sfxSlider.value * 20) -20);
            yield return null;
        }
    }
}
