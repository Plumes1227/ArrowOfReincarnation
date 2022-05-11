using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUiCounter : MonoBehaviour
{
    [Header("=== CANVAS ===")]
    [SerializeField] Canvas startCanvas;
    [SerializeField] Canvas settingsCanvas;
    [Header("=== 按鈕 ===")]
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button backButton;
    [SerializeField] Button exitButton;
    [Header("=== 音效控制 ===")]
    [SerializeField] Toggle bgmToggle;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] Slider sfxSlider;
    [Header("=== 動畫控制 ===")]
    [SerializeField] Animator _TitleAm;

    public void OnPlayButtonClick()
    {
        SceneLoader.Instance.LoadGameplayScene();
        GameManager.GameState = GameState.Playing;
        _TitleAm.SetBool("OnStart", true);
    }

    public void OnSettingsButtonClick()
    {
        StartCoroutine(OnAudioAdjusCoroutine());
        startCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }
    public void OnBackButtonClick()
    {
        StopCoroutine(OnAudioAdjusCoroutine());
        startCanvas.enabled = true;
        settingsCanvas.enabled = false;
        AudioManager.Instance.bgmToggleBool = bgmToggle.isOn;
        AudioManager.Instance.sfxToggleBool = sfxToggle.isOn;
        AudioManager.Instance.bgmSliderValue = bgmSlider.value;
        AudioManager.Instance.sfxSliderValue = sfxSlider.value;
    }
    public void OnExitButtonClick()
    {
    #if UNITY_EDITOR        
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    void OnEnable()
    {
        GameManager.GameState = GameState.Playing;
        bgmToggle.isOn = AudioManager.Instance.bgmToggleBool;
        sfxToggle.isOn = AudioManager.Instance.sfxToggleBool;
        bgmSlider.value = AudioManager.Instance.bgmSliderValue;
        sfxSlider.value = AudioManager.Instance.sfxSliderValue;
    }
    public void Toggle_BGM()
    {
        if(bgmToggle.isOn)
        {
            AudioManager.Instance.AudioMixerObj.SetFloat("BGM",0);
            //AudioManager.Instance.bgmToggleBool = true;
        }else 
        {
            AudioManager.Instance.AudioMixerObj.SetFloat("BGM",-80);
            //AudioManager.Instance.bgmToggleBool = false;
        }
    }
    public void Toggle_SFX()
    {
        if(sfxToggle.isOn)
        {
            AudioManager.Instance.AudioMixerObj.SetFloat("SFX",0);
            //AudioManager.Instance.sfxToggleBool = true;
        }else 
        {
            AudioManager.Instance.AudioMixerObj.SetFloat("SFX",-80);
            //AudioManager.Instance.sfxToggleBool = false;
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
