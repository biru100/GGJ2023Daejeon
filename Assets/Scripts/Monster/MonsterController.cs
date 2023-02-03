using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterStatus monsterStatus;
    [SerializeField]
    private SpriteRenderer m_render = null;

    [SerializeField]
    private Rigidbody2D m_rig = null;

    [SerializeField]
    private HitBox m_hitBox;

    private bool specialMove = false;

    private void Awake()
    {
        if (monsterStatus == null)
        {
            Debug.LogError($"���� �������ͽ� ������ �������� �ʽ��ϴ�. ");
            Debug.Break();
        }
        else
        {
            if (m_render == null)
            {
                Debug.LogError($"Sprite Render�� �������� �ʽ��ϴ�.");
                Debug.Break();
            }
            else
            {
                // Texture ���� 
                // Insam = Normal, Zombie = Reverse
                if (monsterStatus.m_monsterType == MonsterType.INSAM)
                    m_render.sprite = monsterStatus.m_texture;
                else if (monsterStatus.m_monsterType == MonsterType.ZOMBIE)
                    m_render.sprite = monsterStatus.m_reverseTexutre;

                // Hit Box ���� -> ũ�� ����
                m_hitBox.SetRadius(monsterStatus.m_attackRange);

                // ĳ���� ���� �ο�
                monsterStatus.m_monsterLife = true;

            }
        }
    }

    private void Update()
    {
        if (monsterStatus.m_monsterLife)
        {
            Move();
        }
    }

    private void Move()
    {
        if (!specialMove)
        { // �Ϲ� �̵�
            if (monsterStatus.m_monsterType == MonsterType.INSAM)
                m_rig.velocity = Vector2.right * monsterStatus.m_moveSpeed;
            if (monsterStatus.m_monsterType == MonsterType.ZOMBIE)
                m_rig.velocity = Vector2.left * monsterStatus.m_moveSpeed;
        }
        else if (specialMove)
        { 

        }
    }

    private void Death()
    {
        // �����Ų ��ü �Ǻ�
    }

    private void Attack()
    {
        // Ư�� ������ ������.
        // Ư������1.Attack()
        // Ư������2.Attack()

    }

    private void Search()
    {

    }
}
