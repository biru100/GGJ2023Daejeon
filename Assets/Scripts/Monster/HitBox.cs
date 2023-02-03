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
        // �ʹݿ� Radius ����
        m_collider.radius = radius;
    }

    #endregion

    #region PrivateFunc

    public void OnDrawGizmos()
    {
        // Collider �ð�ȭ 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_collider.radius);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    #endregion

}
