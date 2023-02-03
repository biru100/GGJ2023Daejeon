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

    [SerializeField]
    private MonsterStateType m_monsterStateType;


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

                // ���� ���� �ο�
                monsterStatus.m_monsterLife = true;

                // ���� ���� �ο�
                m_monsterStateType = MonsterStateType.IDLE;

            }
        }
    }

    #region LifeCycle

    private void Update()
    {
        if (monsterStatus.m_monsterLife)
        {
            if (m_monsterStateType == MonsterStateType.IDLE)
            {
                m_monsterStateType = MonsterStateType.MOVE;
            }

            if (m_monsterStateType == MonsterStateType.MOVE)
            {
                Move();
                Search();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, monsterStatus.m_searchRange);
    }

    #endregion

    #region Private

    private void Move()
    {
        if (!specialMove)
        { // �Ϲ� �̵�
            if (monsterStatus.m_monsterType == MonsterType.INSAM)
                m_rig.velocity = Vector2.right * monsterStatus.m_moveSpeed;
            if (monsterStatus.m_monsterType == MonsterType.ZOMBIE)
                m_rig.velocity = Vector2.left * monsterStatus.m_moveSpeed;
        }
        else if (specialMove && m_targetObj != null)
        {
            // m_target������ ������Ʈ�� ��������, ����ȭ ���� ��
            var dir = (m_targetObj.transform.position - transform.position).normalized;
            // velocity���� �ش� �������� ���ǵ带 �����ְ�
            m_rig.velocity = dir * monsterStatus.m_moveSpeed;
            
            if((m_targetObj.transform.position - transform.position).magnitude > 0.01f)
            {
                Debug.Log("����");
                m_monsterStateType = MonsterStateType.ATTACK;
            }
        }
    }
   
    private void Death()
    {
        // �����Ų ��ü �Ǻ�
    }

    private void Attack()
    {
        // �Ϲ� ���� (����)

        // �Ϲ� ���� (����)


        // Ư�� ������ ������.

        // Ư������1.Attack()
        // Ư������2.Attack()
    }

    private void Search()
    { // SearchRange���� ���� ����� ������ ���� ã�� �޼���.

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, monsterStatus.m_searchRange);

        foreach (var coll in colls)
        {
            if (gameObject.name != coll.gameObject.name)
            {
                // ��ȸ�ϸ鼭 ���� ����� collider ã�´�.
                if (m_targetObj == null)
                    m_targetObj = coll.gameObject;

                // ���� ���� Short�� ã�´�.
                var shortObj = GetShortCollider(coll);

                // ���� ���� null ���� ���, m_targetObj�� ���� �ƴϸ�, shortObj�� �ִ´�.
                m_targetObj = (shortObj == null) ? m_targetObj : shortObj;

                // �̵� ����� ������ϰ� �����Ѵ�.
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

    #endregion

}
