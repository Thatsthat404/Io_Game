using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� ��ũ��Ʈ ���� ��������
        PlayerBehavior playerMovement = collision.gameObject.GetComponent<PlayerBehavior>();

        if (playerMovement != null)
        {
            // isJump ���¸� false�� ����
            playerMovement.isJump = false; 
        }

       

        JumpMonster jumpMonster = collision.gameObject.GetComponent<JumpMonster>();
        if (jumpMonster != null)
        {
            jumpMonster.scan = false;
        }

    }
}
