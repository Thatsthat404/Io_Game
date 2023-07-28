using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int dmg = 5;
    public BoxCollider2D dmgCollider;

    private void Start()
    {
        StartCoroutine(ActivateDmgCollider());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the "Monster" tag
        if (other.CompareTag("Monster"))
        {
            // Get the Monster script component from the colliding object
            Monster monsterScript = other.GetComponent<Monster>();

            // Check if the Monster script component is not null
            if (monsterScript != null)
            {
                monsterScript.StartCoroutine("OnDamage");
                monsterScript.hp -= dmg;
            }
        }
    }

    private IEnumerator ActivateDmgCollider()
    {
        while (true) // ���� ����
        {
            dmgCollider.enabled = true; // Collider�� Ȱ��ȭ
            yield return new WaitForSeconds(0.2f); // 0.2�� ���
            dmgCollider.enabled = false; // Collider�� ��Ȱ��ȭ
            yield return new WaitForSeconds(0.2f); // 0.2�� ���
        }
    }
}
