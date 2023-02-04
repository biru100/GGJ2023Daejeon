using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GoalType
{
    Insam,
    Zombie
}
public class Goal : MonoBehaviour
{
    [SerializeField]
    private GoalType type;

    [SerializeField]
    GameObject winInsam;
    [SerializeField]
    GameObject winZombie;
    [SerializeField]
    GameObject overPanel;

    private void Awake()
    {
        winInsam.SetActive(false);
        winZombie.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            Time.timeScale = 0;
            winInsam.SetActive(true);
            overPanel.SetActive(true);
        }
        else if(collision.gameObject.layer == 7)
        {
            Time.timeScale = 0;
            winZombie.SetActive(true);
            overPanel.SetActive(true);
        }
    }
}
