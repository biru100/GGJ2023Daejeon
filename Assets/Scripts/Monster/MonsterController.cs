using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class MonsterController : MonoBehaviour
{
    public MonsterStatus monsterStatus;

    public SpriteRenderer m_render = null;

    private void Awake()
    {
        if(monsterStatus == null)
        {
            Debug.LogError($"몬스터 스테이터스 정보가 존재하지 않습니다. ");
            Debug.Break();
        } else
        {
            if(m_render == null)
            {
                Debug.LogError($"Sprite Render가 존재하지 않습니다.");
                Debug.Break();
            } else
            {
                // Texture 적용
                m_render.sprite = monsterStatus.m_texture;
            }
        }
    }

    private void Move()
    {

    }

    private void Death()
    {

    }

    private void Attack()
    {

    }
}
