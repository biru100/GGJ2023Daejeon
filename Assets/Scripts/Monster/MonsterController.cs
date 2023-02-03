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
            Debug.LogError($"몬스터 스테이터스 정보가 존재하지 않습니다. ");
            Debug.Break();
        }
        else
        {
            if (m_render == null)
            {
                Debug.LogError($"Sprite Render가 존재하지 않습니다.");
                Debug.Break();
            }
            else
            {
                // Texture 적용 
                // Insam = Normal, Zombie = Reverse
                if (monsterStatus.m_monsterType == MonsterType.INSAM)
                    m_render.sprite = monsterStatus.m_texture;
                else if (monsterStatus.m_monsterType == MonsterType.ZOMBIE)
                    m_render.sprite = monsterStatus.m_reverseTexutre;

                // Hit Box 생성 -> 크기 설정
                m_hitBox.SetRadius(monsterStatus.m_attackRange);

                // 캐릭터 생명 부여
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
        { // 일반 이동
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
        // 사망시킨 객체 판별
    }

    private void Attack()
    {
        // 특수 공격이 존재함.
        // 특수공격1.Attack()
        // 특수공격2.Attack()

    }

    private void Search()
    {

    }
}
