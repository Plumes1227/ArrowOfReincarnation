using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTubeUi : MonoBehaviour
{
    public static ArrowTubeUi arrowTubeUi;
    [SerializeField] List<GameObject> arrowUiLest;
    [SerializeField] int arrowConsumeNumber;
    [SerializeField] int arrowAddNumber;

    void Awake()
    {
        arrowTubeUi = GetComponent<ArrowTubeUi>();
    }
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            arrowUiLest.Add(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < Player.player.ArrowAmount; i++)
        {
            arrowUiLest[i].SetActive(true);
        }
    }

    public void ConsumeArrow()
    {
        if(arrowConsumeNumber == Player.player.MaxArrowAmount) arrowConsumeNumber = 0;
        arrowUiLest[arrowConsumeNumber].SetActive(false);
        arrowConsumeNumber ++;
    }

    public void ReplenishArrow()
    {
        if(arrowAddNumber == Player.player.MaxArrowAmount) arrowAddNumber = 0;
        arrowUiLest[arrowAddNumber].SetActive(true);
        arrowAddNumber ++;
    }
}
