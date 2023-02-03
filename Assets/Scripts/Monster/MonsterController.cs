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

    [SerializeField]
    private GameObject m_targetObj = null;

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
            Search();

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, monsterStatus.m_searchRange);
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
            if (m_targetObj != null)
            {
                // 가까운 곳으로 잡혔다는 가정 중.

            }
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
    { // SearchRange에서 가장 가까운 범위의 적을 찾는 메서드.

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, monsterStatus.m_searchRange);

        foreach (var coll in colls)
        {
            if (gameObject.name != coll.gameObject.name)
            {
                // 순회하면서 가장 가까운 collider 찾는다.
                if (m_targetObj == null)
                    m_targetObj = coll.gameObject;

                // 가장 작은 Short를 찾는다.
                var shortObj = GetShortCollider(coll);

                // 받은 값이 null 값인 경우, m_targetObj를 쓰고 아니면, shortObj를 넣는다.
                m_targetObj = (shortObj == null) ? m_targetObj : shortObj;

                // 이동 방법을 스페셜하게 변경한다.
                specialMove = true;
            }
            else if (gameObject.name == coll.gameObject.name && colls.Length <= 1)
            {
                specialMove = false;
            }
        }
    }

    private GameObject GetShortCollider(Collider2D coll)
    {
        if (m_targetObj != null)
        {
            if (Vector2.Distance(transform.position, m_targetObj.transform.position) > Vector2.Distance(transform.position, coll.gameObject.transform.position))
                return coll.gameObject;
        }

        return null;
    }
}
