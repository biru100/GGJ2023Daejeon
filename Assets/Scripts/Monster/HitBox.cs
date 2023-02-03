using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    #region Setting

    [SerializeField]
    private CircleCollider2D m_collider;

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
        
    }

    #endregion

}
