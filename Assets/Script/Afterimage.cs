using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Color color;
    Color originalColor;
    float transparency;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = _spriteRenderer.color;
        color = originalColor;
    }

    void OnEnable()
    {
        transparency = 1;
        color.a = transparency;
        _spriteRenderer.color = color;
        StartCoroutine(nameof(FadeAwayCorCoroutine));
    }

    void OnDisable()
    {
        _spriteRenderer.color = originalColor;
        StopAllCoroutines();
    }

    IEnumerator FadeAwayCorCoroutine()
    {
        while(true)
        {
            transparency -= Time.deltaTime*2;
            color.a = transparency;
            _spriteRenderer.color = color;
            yield return null;
        }
    }
    
}
