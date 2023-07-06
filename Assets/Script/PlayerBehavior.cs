using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;  // �̵� ���ǵ�
    public float jumpForce = 5f;  // ������

    public float moveX;  // �¿� Ű �Է�
    public float moveY;  // ���� Ű �Է�
    public bool keyJump; // ���� Ű �Է�

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        KeyInput();
        Move();
    }


    private void KeyInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        keyJump = Input.GetButtonDown("Jump");
    }


    private void Move()
    {
        Vector3 movement = new Vector3(moveX, 0f, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (keyJump)
        {
            rigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
}






