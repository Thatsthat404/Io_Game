using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownFloor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� ��ũ��Ʈ ���� ��������
        PlayerBehavior playerMovement = collision.gameObject.GetComponent<PlayerBehavior>();

        if (playerMovement != null)
        {
            // isJump ���¸� false�� ����
            playerMovement.isUnderJump = false;
        }
    }

}
