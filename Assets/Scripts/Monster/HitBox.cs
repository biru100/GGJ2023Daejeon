using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    #region Setting

    [SerializeField]
    private CircleCollider2D m_collider;

    [SerializeField]
    private List<MonsterController> m_monsterControllerList;

    private void Awake()
    {
        m_collider = GetComponent<CircleCollider2D>();
    }

    #endregion

    #region PublicFunc

    public void SetRadius(float radius)
    {
        // 초반에 Radius 세팅
        m_collider.radius = radius;
    }

    public List<MonsterController> GetMonsterList()
    {
        return m_monsterControllerList;
    }

    #endregion

    #region PrivateFunc

    public void OnDrawGizmos()
    {
        // Collider 시각화 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_collider.radius);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            collision.TryGetComponent<MonsterController>(out var monster);
            m_monsterControllerList.Add(monster);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Monster"))
        {
            collision.collider.TryGetComponent<MonsterController>(out var monster);
            m_monsterControllerList.Add(monster);
        }
    }

    #endregion

}
