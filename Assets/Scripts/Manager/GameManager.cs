using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject monsterSpawnerObject;
    public GameObject player1;
    public GameObject player2;

    public void OnGameStart()
    {
        monsterSpawnerObject.SetActive(true);
        player1.SetActive(true);
        player2.SetActive(true);
    }
}
