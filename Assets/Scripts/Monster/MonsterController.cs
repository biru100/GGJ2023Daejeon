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

    private MonsterType m_monsterType;
    private int hp;
    private int animIndex = 0;
    private float animOldTime;
    private float attackOldTime;
    private bool isAttackPlayer;

    private Sprite[] textureList;

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
                m_monsterType = monsterStatus.m_monsterType;
                if (m_monsterType == MonsterType.INSAM)
                {
                    textureList = monsterStatus.m_texture;
                    m_render.sprite = monsterStatus.m_texture[0];
                    m_render.flipX = true;
                }
                else if (m_monsterType == MonsterType.ZOMBIE)
                {
                    textureList = monsterStatus.m_reverseTexutre;
                    m_render.sprite = monsterStatus.m_reverseTexutre[0];
                }

                // Hit Box ���� -> ũ�� ����
                m_hitBox.SetRadius(monsterStatus.m_attackRange);

                // ���� ���� �ο�
                monsterStatus.m_monsterLife = true;

                // ���� ���� �ο�
                m_monsterStateType = MonsterStateType.IDLE;

                hp = monsterStatus.m_hp;
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

            if(m_monsterStateType == MonsterStateType.ATTACK)
            {
                Attack();
            }
        }
    }

    private void LateUpdate()
    {
        DeathCheck();
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
            if (m_monsterType == MonsterType.INSAM)
                m_rig.velocity = Vector2.right * monsterStatus.m_moveSpeed;
            if (m_monsterType == MonsterType.ZOMBIE)
                m_rig.velocity = Vector2.left * monsterStatus.m_moveSpeed;
        }
        else if (specialMove && m_targetObj != null)
        {
            // m_target������ ������Ʈ�� ��������, ����ȭ ���� ��
            var dir = (m_targetObj.transform.position - transform.position).normalized;
            // velocity���� �ش� �������� ���ǵ带 �����ְ�
            m_rig.velocity = dir * monsterStatus.m_moveSpeed;


            if ((m_targetObj.transform.position - transform.position).magnitude < monsterStatus.m_attackRange)
            {
                m_monsterStateType = MonsterStateType.ATTACK;
                m_rig.velocity = Vector2.zero;

            }
        }
        if(Time.time - animOldTime > monsterStatus.m_animTime)
        {
            animOldTime = Time.time;
            m_render.sprite = textureList[animIndex];
            animIndex++;
            animIndex %= textureList.Length;
        }
    }

    private void Hit(int value, bool isPlayerAttack)
    {
        hp -= value;
        isAttackPlayer = isPlayerAttack;
    }

    private void DeathCheck()
    {
        if (hp <= 0)
        {
            if (isAttackPlayer)
            {
                ChangeTeam();
            }
            else
            {
                Death();
            }
        }
        isAttackPlayer = false;
    }
   
    private void Death()
    {
        // �����Ų ��ü �Ǻ�
        Destroy(gameObject);
    }

    private void ChangeTeam()
    {
        if(m_monsterType == MonsterType.INSAM)
        {
            m_monsterType = MonsterType.ZOMBIE;
        }
        else
        {
            m_monsterType = MonsterType.INSAM;
        }
    }

    private void Attack()
    {
        if(Time.time - attackOldTime < monsterStatus.m_attackDelay)
        {
            return;
        }
        attackOldTime = Time.time;
        if (m_targetObj)
        {
            if (m_targetObj.TryGetComponent<MonsterController>(out var m_monster))
            {
                m_monster.Hit(monsterStatus.m_damage, false);
            }
        }
        // �Ϲ� ���� (����)

        // �Ϲ� ���� (����)


        // Ư�� ������ ������.

        // Ư������1.Attack()
        // Ư������2.Attack()
    }

    private void Search()
    { // SearchRange���� ���� ����� ������ ���� ã�� �޼���.

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, monsterStatus.m_searchRange, LayerMask.GetMask("Enemy"));

        foreach (var coll in colls)
        {
            if (gameObject != coll.gameObject)
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
