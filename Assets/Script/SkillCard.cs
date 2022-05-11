using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillCard
{
    [SerializeField] public int CardNumber;
    [SerializeField] public string CardText;
    public Sprite CardImage;
    [SerializeField] public Sprite[] CardIcon;
    [SerializeField] AudioData[] SkillSFX;

    //抽卡片名稱
    public void DrawCard()
    {
        CardNumber = Random.Range(0,7);
        CardImage = CardIcon[CardNumber];
        switch(CardNumber)
        {
            case 0:
            CardText = "生命\n全恢復";
            break;
            case 1:
            CardText = "無敵\n五秒鐘";
            break;
            case 2:
            CardText = "敵人\n全凍結";
            break;
            case 3:
            CardText = "箭矢\n全回收";
            break;
            case 4:
            CardText = "箭矢\n超加速";
            break;
            case 5:
            CardText = "全集中\n呼吸";
            break;
            case 6:
            CardText = "魔力\n全恢復";
            break;
        }
    }

    //使用卡片效果(實現在各腳本中)
    public void UseCard()
    {
        switch(CardNumber)
        {
            case 0:
            Player.player.HealingLife();
            AudioManager.Instance.PlaySFX(SkillSFX[0]);
            break;
            case 1:
            Player.player.Invincible(5);
            //AudioManager.Instance.PlaySFX(SkillSFX[1]);
            break;
            case 2:
            EnemyManager.Instance.FreezeAllEnemy();
            AudioManager.Instance.PlaySFX(SkillSFX[2]);
            break;
            case 3:
            Player.player.RemoveAllArrow();
            //AudioManager.Instance.PlaySFX(SkillSFX[3]);
            break;
            case 4:
            Player.player.ArrowSpeedUp();
            //AudioManager.Instance.PlaySFX(SkillSFX[4]);
            break;
            case 5:
            TimeController.Instance.BulletTime(0.5f,0.5f,1.5f);    //時間緩速
            EnemyManager.Instance.RemoveAllEnemy();     //殺死所有敵人
            //撥放一個帥氣的箭矢鎖定場上所有敵人？
            AudioManager.Instance.PlaySFX(SkillSFX[5]);
            break;
            case 6:
            Player.player.FullMagicPoint();
            AudioManager.Instance.PlaySFX(SkillSFX[6]);
            break;
        }
    }
}
