using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] UnityEngine.UI.Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;

    [SerializeField] Color color;

    const string GAMEPLAY = "GamePlayScene";
    const string GAME_START = "GameStartScene";
    const string SCORING = "Scoring";

    IEnumerator LoadingCoroutine(string sceneName)
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        transitionImage.gameObject.SetActive(true);

        while(color.a < 1f)
        {
           color.a = Mathf.Clamp01(color.a +=Time.unscaledDeltaTime / fadeTime);
           transitionImage.color = color;

           yield return null;
        }

        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);

        loadingOperation.allowSceneActivation = true;

                while(color.a > 0f)
        {
           color.a = Mathf.Clamp01(color.a -=Time.unscaledDeltaTime / fadeTime);
           transitionImage.color = color;

           yield return null;
        }
        transitionImage.gameObject.SetActive(false);
    }

    public void LoadGameplayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadGameStartScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAME_START));
    }
    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }
}
