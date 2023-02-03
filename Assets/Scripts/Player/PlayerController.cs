using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerState
{
    INSAM,
    ZOMBIE
}
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerState playerState;
    [SerializeField]
    private float speed;

    private Rigidbody2D rigid;
    private string playerHorizontal;
    private string playerVertical;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerHorizontal = playerState == PlayerState.INSAM ? "InsamHorizontal" : "ZombieHorizontal";
        playerVertical = playerState == PlayerState.INSAM ? "InsamVertical" : "ZombieVertical";
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw(playerHorizontal), Input.GetAxisRaw(playerVertical)).normalized;

        rigid.velocity = dir * speed * Time.deltaTime;
    }
}
