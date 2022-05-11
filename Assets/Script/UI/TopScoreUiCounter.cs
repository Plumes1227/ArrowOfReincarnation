using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopScoreUiCounter : MonoBehaviour
{
    [SerializeField] Text timeScoreText;
    [SerializeField] Text killScoreText;
    [SerializeField] Transform highScoreLeaderboardContainer;

    public void UpdatePlayScoreText()
    {
        timeScoreText.text = ScoreManager.Instance.SurvivetimeText;
        killScoreText.text = ScoreManager.Instance.KillAmount.ToString();
    }
    public void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for (int i =0; i< highScoreLeaderboardContainer.childCount; i++)
        {
            var child = highScoreLeaderboardContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("KillScore").GetComponent<Text>().text = playerScoreList[i].killScore.ToString();
            child.Find("TimeScore").GetComponent<Text>().text = playerScoreList[i].surviveTimeScore;
        }
    }
}
