using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public int hp;
    public int touchDmg;
    public float speed = 5f;
    public bool dodie; // �׾����� ��Ҵ���








    public SpawnManager spawnManager; // SpawnManager ��ũ��Ʈ�� ������ ������ ����
   
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (collision.gameObject.CompareTag("Bullet") && !dodie)
        {
            hp -= bullet.dmg;
            if (hp < 0)
            {
                hp = 0;
            }
        }

        PlayerBehavior playerBehavior = collision.gameObject.GetComponent<PlayerBehavior>();
        if (playerBehavior != null)
        {
            playerBehavior.DecreaseHp(touchDmg);
        }
    }
}
