using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform player1Spawner;
    [SerializeField]
    private Transform player2Spawner;

    [SerializeField]
    GameObject[] monsterList;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Spawn(0, 0, true);
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            Spawn(1, 0, false);
        }    
    }

    void Spawn(int index, int gold, bool isInsam)
    {
        Vector3 pos = isInsam ? player1Spawner.position : player2Spawner.position;
        Instantiate(monsterList[index], pos, Quaternion.identity);
    }
}
