using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic : Monster
{
    public bool findAttack;
    public float rayLength = 3f; // ���� ����

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        ScanFront();
        if(!findAttack)
        {
            MoveMonster();
        }
        else 
        {
            Attack();
        }
    }


    private void ScanFront() // ���濡 �÷��̾�, Ȥ�� �Ͽ콺�� �ִ� Ȯ���ϴ� �Լ�
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayLength, LayerMask.GetMask("Player") | LayerMask.GetMask("House"));

        Debug.DrawRay(transform.position, Vector2.left * rayLength, Color.red); // Ray�� �ð����� ǥ�ø� ���� Debug.DrawRay ���

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("House"))
            {
                findAttack = true;  
            }
        }
        else
        {
            findAttack = false;  
        }
    }

    private void MoveMonster() // �������� �̵��ϴ� �Լ�
    {
        anim.SetBool("isWalk", true);
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void Attack()
    {

    }
}
