using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public float fadeDuration;

    private SpriteRenderer spriteRenderer;
    private Color startColor;
    private bool fading = false;

    // ������ �κ�: ���� �� �ٷ� FadeAndDestroyRoutine()�� ȣ���Ͽ� ���̵�ƿ� ����
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;

        // �������ڸ��� ���̵�ƿ� �� ���� ��ƾ ����
        StartCoroutine(FadeAndDestroyRoutine());
    }

    private IEnumerator FadeAndDestroyRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        fading = true;
        float elapsedTime = 0f;
        float startAlpha = spriteRenderer.color.a; // ���� ���İ� ����

        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, 0f, normalizedTime); // ���� ���İ����κ��� ���̵�ƿ� ����
            alpha = Mathf.Max(alpha, 0f); // ���İ��� 0 �̸��� ���� �ʵ��� ����

            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            spriteRenderer.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }




    private void Update()
    {
        // ȭ���� ���� ��� ����
        if (!fading && !spriteRenderer.isVisible)
        {
            Destroy(gameObject);
        }
    }
}