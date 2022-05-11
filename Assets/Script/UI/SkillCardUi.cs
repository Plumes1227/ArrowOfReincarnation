using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCardUi : Singleton<SkillCardUi>
{
    [SerializeField] AudioData sFX_DrawCard;
    [SerializeField] List<GameObject> skillCardLest;
    [SerializeField] int skillCardConsumeNumber;
    [SerializeField] int skillCardAddNumber;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            skillCardLest.Add(transform.GetChild(i).gameObject);
        }
    }

    public void ConsumeSkillCard()
    {
        if(skillCardConsumeNumber == transform.childCount) skillCardConsumeNumber = 0;
        skillCardLest[skillCardConsumeNumber].SetActive(false);
        skillCardConsumeNumber ++;
    }

    public void ReplenishSkillCard()
    {
        AudioManager.Instance.PlaySFX(sFX_DrawCard);
        if(skillCardAddNumber == transform.childCount) skillCardAddNumber = 0;
        skillCardLest[skillCardAddNumber].SetActive(true);
        skillCardAddNumber ++;
    }

}
