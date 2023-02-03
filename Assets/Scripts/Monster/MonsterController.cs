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
            Debug.LogError($"���� �������ͽ� ������ �������� �ʽ��ϴ�. ");
            Debug.Break();
        } else
        {
            if(m_render == null)
            {
                Debug.LogError($"Sprite Render�� �������� �ʽ��ϴ�.");
                Debug.Break();
            } else
            {
                // Texture ����
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
