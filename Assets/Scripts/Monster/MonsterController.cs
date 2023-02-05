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
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private float scale;

    private bool specialMove = false;

    [SerializeField]
    private MonsterStateType m_monsterStateType;

    private MonsterType m_monsterType;

    private int hp;
    private int animIndex = 0;

    private float animOldTime;
    private float attackOldTime;
    private bool isAttackPlayer;

    [Space(10)]
    [SerializeField]
    private int m_attackType = 0;

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

    private void FixedUpdate()
    {
        if (monsterStatus.m_monsterLife)
        {
            if (m_monsterStateType == MonsterStateType.MOVE)
            {
                Move();
            }
        }
    }

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
                Search();
            }
            else if (m_monsterStateType == MonsterStateType.ATTACK)
            {
                Search();
                Attack();
            }
        }

        m_render.color = Color.Lerp(m_render.color, Color.white, Time.deltaTime * 10);
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

    #region Public
    public MonsterStatus GetStatus()
    {
        return monsterStatus;
    }
    #endregion

    #region Private

    private void Move()
    {
        if (!specialMove)
        { // �Ϲ� �̵�
            if (m_monsterType == MonsterType.INSAM)
            {
                transform.Translate(Vector2.right * monsterStatus.m_moveSpeed * Time.deltaTime);
                m_render.flipX = true;
            }
            if (m_monsterType == MonsterType.ZOMBIE)
            {
                transform.Translate(Vector2.left * monsterStatus.m_moveSpeed * Time.deltaTime);
                m_render.flipX = false;
            }
        }
        else if (specialMove && m_targetObj != null)
        {
            // m_target������ ������Ʈ�� ��������, ����ȭ ���� ��
            var dir = (m_targetObj.transform.position - transform.position).normalized;

            // velocity���� �ش� �������� ���ǵ带 �����ְ�
            transform.Translate(dir * monsterStatus.m_moveSpeed * Time.deltaTime);
            m_render.flipX = dir.x > 0;

            if ((m_targetObj.transform.position - transform.position).magnitude < monsterStatus.m_attackRange)
            {
                m_monsterStateType = MonsterStateType.ATTACK;

                m_rig.constraints = RigidbodyConstraints2D.FreezeAll;

                m_rig.velocity = Vector2.zero;
                attackOldTime = Time.time;
            }
        }
        if (Time.time - animOldTime > monsterStatus.m_animTime)
        {
            animOldTime = Time.time;
            m_render.sprite = textureList[animIndex];
            animIndex++;
            animIndex %= textureList.Length;
        }
    }

    public void Hit(int value, bool isPlayerAttack)
    {
        if (m_render)
            m_render.color = Color.red;
        Instantiate(hitEffect, transform.position, transform.rotation).transform.localScale = Vector3.one * scale;
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
        if (m_monsterType == MonsterType.INSAM)
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
        string targetString = (m_monsterType == MonsterType.INSAM) ? "Zombie" : "Insam";

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, monsterStatus.m_searchRange, LayerMask.GetMask(targetString));

        if (colls.Length <= 0)
        {
            m_monsterStateType = MonsterStateType.MOVE;
            specialMove = false;
            m_rig.constraints = RigidbodyConstraints2D.FreezeRotation;

            return;
        }

        if (Time.time - attackOldTime < monsterStatus.m_attackDelay)
        {
            return;
        }

        attackOldTime = Time.time;

        if (m_targetObj)
        {
            if (m_attackType == 0) // Normal  
            {
                if (m_targetObj.TryGetComponent<MonsterController>(out var m_monster))
                    m_monster.Hit(monsterStatus.m_damage, false);
            }
            else if (m_attackType == 1)
            {
                for (var i = 0; i < m_hitBox.GetMonsterList().Count; ++i)
                {
                    m_hitBox.GetMonsterList()[i].Hit(m_hitBox.GetMonsterList()[i].monsterStatus.m_damage, false);
                }
            }
        }
    }

    private void Search()
    { // SearchRange���� ���� ����� ������ ���� ã�� �޼���.

        string targetString = "";
        if (m_monsterType == MonsterType.INSAM)
        {
            targetString = "Zombie";
        }
        else
        {
            targetString = "Insam";
        }
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, monsterStatus.m_searchRange, LayerMask.GetMask(targetString));

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

    public int GetCost()
    {
        return monsterStatus.m_cost;
    }
}
