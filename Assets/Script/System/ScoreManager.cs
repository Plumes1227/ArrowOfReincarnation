using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region 遊玩計分
    public int KillAmount => killAmount;
    int killAmount;
    public string SurvivetimeText => survivetimeText;
    string survivetimeText;
    int surviveTotalTime;

    public void killAmountAdd()
    {
        killAmount++;
        ScoreCounterUi.UpdateUi(killAmount);
    }
    public void SurviveTotalTimeSave(int time, string timeText)
    {
        surviveTotalTime = time;
        survivetimeText = timeText;
    }

    public void SaveScoreData()
    {
        //之後做數據存檔-高分榜
        killAmount = 0;     //存檔完後清零
    }
    #endregion
    
    #region 高分存檔系統
    [System.Serializable] public class PlayerScore
    {
        public int killScore;
        public int totalTime;
        public string surviveTimeScore;

        public PlayerScore(int score, int time, string timeText)
        {
            this.killScore = score;
            this.totalTime = time;
            this.surviveTimeScore = timeText;
        }
    }
    
    [System.Serializable] public class PlayerScoreData
    {
        public List<PlayerScore> list = new List<PlayerScore>();
    }

    readonly string SaveFileName = "player_score.json";
    //public bool HasNewHighSurviveTime => surviveTotalTime > LoadPlayerScoreData().list[5].totalTime; //時間排行
    //public bool HasNewKillScore => killAmount > LoadPlayerScoreData().list[5].killScore;        //殺敵數排行

    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();

        playerScoreData.list.Add(new PlayerScore(killAmount, surviveTotalTime, survivetimeText));
        playerScoreData.list.Sort((x, y) => y.totalTime.CompareTo(x.totalTime));        //時間排列
        //playerScoreData.list.Sort((x, y) => y.killScore.CompareTo(x.killScore));        //殺敵數排列

        SaveSystem.Save(SaveFileName, playerScoreData);
    }

    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();

        //playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        if(SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.list.Count < 5)
            {
                playerScoreData.list.Add(new PlayerScore(0, 0, "00:00"));
            }

            SaveSystem.Save(SaveFileName, playerScoreData);
        }

        return playerScoreData;
    }

    #endregion
}
