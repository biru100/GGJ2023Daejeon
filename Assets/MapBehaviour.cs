using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBehaviour : Singleton<MapBehaviour>
{
    [SerializeField]
    private Image[] bgArray; // 0 - Insam, 1 - middle, 2 - zombie

    [SerializeField]
    private float bgRatio; // 4.5 1 4.5

    private readonly float maxRatio = 0.45f;


    protected override void Awake()
    {
        base.Awake();

        bgArray[0].fillAmount = maxRatio;
        bgArray[2].fillAmount = maxRatio;
    }

    public void SetFillRatio(int type, float ratio)
    {
        if (ratio > maxRatio || (type < 0 && type > 2))
            return;
        
        float result = (1 - ratio) - 0.1f;

        switch(type)
        {
            case 0:
                if (result <= 0)
                {
                    bgArray[0].fillAmount = 0.9f;
                    bgArray[2].fillAmount = 0f;
                }
                else if(result > 0)
                {
                    bgArray[0].fillAmount = ratio;
                    bgArray[2].fillAmount = result;
                }
                break;
            case 2:
                if (result <= 0)
                {
                    bgArray[2].fillAmount = 0.9f;
                    bgArray[0].fillAmount = 0f;
                }
                else if (result > 0)
                {
                    bgArray[2].fillAmount = ratio;
                    bgArray[0].fillAmount = result;
                }
                break;
        }
    }

}
