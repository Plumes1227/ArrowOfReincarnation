using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadUi : MonoBehaviour
{
    [SerializeField] Image imageHpBar;
    [SerializeField] Image imageMpBar;
    [SerializeField] GameObject phiz;
    float currentFillAmount;

    public void InitializeHpBar(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        imageHpBar.fillAmount = currentFillAmount;
    }
    public void InitializeMpBar(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        imageMpBar.fillAmount = currentFillAmount;
    }
    public void ShowPhiz()
    {
        if(phiz.activeSelf) return;
        phiz.SetActive(true);
    }
}
