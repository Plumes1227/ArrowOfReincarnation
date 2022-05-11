using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetting : MonoBehaviour
{
    [SerializeField] Text cardText;
    [SerializeField] Image cardImage;
    [SerializeField] SkillCard thisSkillCards;

    void OnEnable()
    {
        thisSkillCards.DrawCard();
        cardText.text = thisSkillCards.CardText;
        cardImage.sprite = thisSkillCards.CardImage;
    }

    public void ActivateCard()
    {
        thisSkillCards.UseCard();
    }
}
