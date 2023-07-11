using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp;
    public int dmg;
    public float speed = 5f;

    public bool dodie; // �׾����� ��Ҵ���

    public bool id; // �������� ���ʰ� ������ �� ��� ������ �ٶ󺸰� �ִ����� ���� ��������Ʈ�� ������ ���ؼ�

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
        Bullet playerBehavior = collision.gameObject.GetComponent<Bullet>();

        if (collision.gameObject.CompareTag("Bullet") && !dodie) 
        {
            StartCoroutine(OnDamage());
            hp -= playerBehavior.dmg;
            if (hp < 0)
            {
                hp = 0;
            }
        }
    }

    IEnumerator OnDamage()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material originalMaterial = renderer.material;
        Material blinkMaterial = new Material(originalMaterial);
        blinkMaterial.color = Color.yellow;

        float blinkDuration = 0.06f; // ������ ���� (��)
        int blinkCount = 1; // ������ Ƚ��

        for (int i = 0; i < blinkCount; i++)
        {
            renderer.material = blinkMaterial;
            yield return new WaitForSeconds(blinkDuration);

            renderer.material = originalMaterial;
            yield return new WaitForSeconds(blinkDuration);
        }
        yield return new WaitForSeconds(0.2f);
        renderer.material = originalMaterial;
    }
}
