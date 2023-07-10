using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp;
    public int dmg;
    public float speed = 5f;

    public bool id; // �¿� ���� ������ �ӽ÷� ������� 


    void OnEnable()
    {
        if(id) // ��������Ʈ�� �ݴ�� �Ǿ� �ִ� �͵� ������ ��
        {
            // ������Ʈ�� X �������� ������Ŵ
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) // �Ѿ˿� ������ ����
        {
            Destroy(gameObject);
        }
    }
}
