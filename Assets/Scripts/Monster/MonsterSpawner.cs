using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform player1Spawner;
    [SerializeField]
    private Transform player2Spawner;

    [SerializeField]
    private TMP_Text gold1Text;
    [SerializeField]
    private TMP_Text gold2Text;

    [SerializeField]
    private List<MonsterController> insamList = new List<MonsterController>();

    [SerializeField]
    private List<MonsterController> zombieList = new List<MonsterController>();

    [SerializeField]
    GameObject[] monsterList;

    public float gold1 = 0;
    public float gold2 = 0;

    private void Update()
    {
        gold1 += Time.deltaTime;
        gold2 += Time.deltaTime;
        gold1Text.text = gold1.ToString("N0");
        gold2Text.text = gold2.ToString("N0");
        if (Input.GetKeyDown(KeyCode.F))
        {
            Spawn(0, true);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Spawn(1, false);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Spawn(2, true);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Spawn(3, false);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Spawn(4, true);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Spawn(5, false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Spawn(6, true);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Spawn(7, false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Spawn(8, true);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Spawn(9, false);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Spawn(10, true);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Spawn(11, false);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(GetHeadMonster(1).name);
            Debug.Log(GetHeadMonster(2).name);
        }
    }

    void Spawn(int index, bool isInsam)
    {
        int cost = monsterList[index].GetComponent<MonsterController>().GetCost();

        if (isInsam)
        {
            if (gold1 - cost < 0)
                return;

            gold1 -= cost;
        }
        else
        {
            if (gold2 - cost < 0)
                return;
            gold2 -= cost;
        }
        gold1Text.text = gold1.ToString("N0");
        gold2Text.text = gold2.ToString("N0");

        Vector3 pos = isInsam ? player1Spawner.position : player2Spawner.position;
        var obj = Instantiate(monsterList[index], pos, Quaternion.identity);


        if (isInsam)
            insamList.Add(obj.GetComponent<MonsterController>());
        else
            zombieList.Add(obj.GetComponent<MonsterController>());

    }

    public Transform GetHeadMonster(int type)
    {
        if (type == 1)
            return ForeachMonsterList(insamList, type).transform;
        else if (type == 2)
            return ForeachMonsterList(zombieList, type).transform;

        return null;
    }

    private GameObject ForeachMonsterList(List<MonsterController> monsterList, int type)
    {
        GameObject resultObject = null;

        if (monsterList.Count <= 0)
            return null;

        for (int i = 0; i < monsterList.Count; ++i)
        {
            if (resultObject == null)
                resultObject = monsterList[i].gameObject;
            else
            {
                if (type == 1)
                {
                    if (resultObject.transform.position.x < monsterList[i].gameObject.transform.position.x)
                        resultObject = monsterList[i].gameObject;
                }
                else if(type == 2)
                {
                    if (resultObject.transform.position.x > monsterList[i].gameObject.transform.position.x)
                        resultObject = monsterList[i].gameObject;
                }
            }
        }
        return resultObject;
    }
}
