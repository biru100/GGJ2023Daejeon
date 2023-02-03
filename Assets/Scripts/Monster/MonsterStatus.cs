using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterData", menuName = "Monster/MonsterData", order = 0)]
public class MonsterStatus : ScriptableObject
{
    public MonsterType m_monsterType;

    [Space(10)]
    public Sprite m_texture = null;
    public Sprite m_reverseTexutre = null;

    [Space(10)]
    public int m_hp = 0;
    public int m_damage = 0;

    [Space(10)]
    public float m_searchRange = .0f;
    public float m_attackRange = .0f;

    [Space(10)]
    public float m_moveSpeed = .0f;
    public float m_attackSpeed = .0f;

    [Space(10)]
    public bool m_monsterLife = false; 
}
