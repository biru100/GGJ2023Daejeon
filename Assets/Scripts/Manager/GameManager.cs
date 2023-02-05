using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // ΩÃ±€≈Ê √≥∏Æ
    public GameObject monsterSpawnerObject;
    public GameObject player1;
    public GameObject player2;
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void OnGameStart()
    {
        monsterSpawnerObject.SetActive(true);
        player1.SetActive(true);
        player2.SetActive(true);
    }

    public void Retry()
    {
        Destroy(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
