using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // ΩÃ±€≈Ê √≥∏Æ
    [SerializeField]
    private Image backgroundInsam;
    [SerializeField]
    private Image backgroundZombie;

    float currentInsam;
    float currentZombie;

    private void Start()
    {
        Time.timeScale = 1;
        SetBackGround(0, 0);
    }

    public void SetBackGround(float insam, float zombie)
    {
        backgroundInsam.fillAmount = (Mathf.Max(currentInsam, insam) + 27) / 54;
        backgroundZombie.fillAmount = (Mathf.Max(currentZombie, insam) + 27) / 54;
    }
}
