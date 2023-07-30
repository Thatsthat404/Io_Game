using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Basic_test : Monster
{
    public bool findAttack; // ���� ã��
    public bool isAttack; // ���� ��
    public float rayLength = 2f; // ���� ����

    public float xposition;
    public float yposition;

    public BoxCollider2D attackArea; // ���� ����


    private void Awake()
    {
      
    }

    private void Update()
    {
        if (hp <= 0 && !dodie)
        {
            StartCoroutine(Die());
        }
        if (!dodie)
        {
            if (!isAttack)
            {
                if (!findAttack)
                {
                    MoveMonster();
                }
                else
                {
                    StartCoroutine(Attack());
                }
            }
            ScanFront();
        }
    }


    private void MoveMonster() // �������� �̵��ϴ� �Լ�
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }



    private void ScanFront() // ���濡 �÷��̾�, Ȥ�� �Ͽ콺�� �ִ� Ȯ���ϴ� �Լ�
    {
        Vector2 raycastOrigin = new Vector2(transform.position.x - xposition, transform.position.y - yposition);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.left, rayLength, LayerMask.GetMask("Player") | LayerMask.GetMask("House"));

        Debug.DrawRay(raycastOrigin, Vector2.left * rayLength, Color.red);

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



    IEnumerator Attack()
    {
        isAttack = true;

        yield return new WaitForSeconds(1.5f);
        attackArea.enabled = true;

        yield return new WaitForSeconds(0.3f);

        attackArea.enabled = false;

        isAttack = false;
    }






    IEnumerator Die()
    {
        dodie = true;

        // �ݶ��̴� ��Ȱ��ȭ
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Rigidbody2D ������ ��Ȱ��ȭ
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        rigidbody.simulated = false;

        yield return new WaitForSeconds(1.15f);
        Destroy(gameObject);
    }
}




