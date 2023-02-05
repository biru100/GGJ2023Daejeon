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
    [SerializeField]
    private Sprite idleSprite;
    [SerializeField]
    private Sprite[] walkSprite;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private string playerHorizontal;
    private string playerVertical;
    private int walkIndex = 0;
    private float walkOldTime;
    private Vector2 dir;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerHorizontal = playerState == PlayerState.INSAM ? "InsamHorizontal" : "ZombieHorizontal";
        playerVertical = playerState == PlayerState.INSAM ? "InsamVertical" : "ZombieVertical";
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        dir = new Vector2(Input.GetAxisRaw(playerHorizontal), Input.GetAxisRaw(playerVertical)).normalized;
        if (dir.magnitude == 0)
        {
            spriteRenderer.sprite = idleSprite;
        }
        else if (dir.magnitude > 0 && Time.time - walkOldTime > 0.2f)
        {
            walkOldTime = Time.time;
            spriteRenderer.sprite = walkSprite[walkIndex];
            walkIndex++;
            walkIndex %= walkSprite.Length;
            spriteRenderer.flipX = dir.x > 0;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rigid.velocity = dir * speed * Time.deltaTime;
    }
}
