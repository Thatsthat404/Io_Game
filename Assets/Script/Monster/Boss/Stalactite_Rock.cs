using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Stalactite_Rock : MonoBehaviour
{
    public int dmg;
    public float downSpeed;


    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // �Ʒ� �������� ���� �ִ� ���� �߰�
        if (gameObject.activeSelf)
        {
            rb.AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse); // �Ʒ��� ���� ���ϵ��� ���� ����
        }
    }


    private void StalactiteBomb()
    {

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

}
