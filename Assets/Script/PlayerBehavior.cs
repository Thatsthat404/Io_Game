using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

// �⺻���� / ��ź�� ������ ���� / ����Ŭ���� �ñ�� ���� is~~�� �̿��ؼ� ���� � �ൿ�� �ϰ� �ִ��� �����ؾ���
// ����� ������ �߻縸 �Ǿ� ����

public class PlayerBehavior : MonoBehaviour
{
    public int hp;
    public int dmg; // ���ݷ�
    public bool isDamage; // �������� �԰� �ִ���
    public bool doDie; // �׾����� ��Ҵ���

    public float moveSpeed = 5f;  // �̵� ���ǵ�
    public float jumpForce = 5f;  // ������
    public int bulletNumber; // ���� �߻��ϴ� �Ѿ��� ��ȣ��(�Ѿ� ����)
    public float attackSpeed; // ���� ���ǵ�
    public int gemPoint;
    private float attackTimer = 0f;
    
    public float moveX;  // �¿� Ű �Է�
    public bool movedown;  // �Ʒ�����Ű �Է�
    public bool keyJump; // ���� Ű �Է�
    public bool keySkilla; // 1�� ��ų �Է�
    public bool keySkillb; // 2�� ��ų �Է�
    public bool keySkillc; // 3�� ��ų �Է�

    public bool isJump; // ����������
    private bool isAttack; // ���� ���� ������
    public bool isRope; // ������ Ÿ�� �ִ���
    public bool isUnderJump; // ���� ���� �ϰ� �ִ���
    public bool positionUpDown; // ���� ��ܿ� �ִ��� �ϴܿ� �ִ���

    private bool isSkillb = false; // 2�� ��ų ��������� �ƴ���
    private bool isSkillc = false; // 3�� ��ų ��������� �ƴ���

    private Rigidbody2D rigid;
    public Transform bulletPosition;
    public GameObject[] bullet; // �Ѿ� ������ ���� �迭
    public GameObject gemBomb; // ��ų 1���� ���� ������
    public GameObject laserTrajectory; // ��ų 2���� ���� ������
    public GameObject laserBeam; // ��ų 2���� ���� ������
    public BoxCollider2D laserCollider; // ��ų 2���� ���� ������
    public GameObject whiteScreen; // ��ų 3���� ���� ������
    public GameObject whiteScreenDmg; // ��ų 3���� ���� ������
    public float fadeInDuration; // ���İ��� �ִ�� ���ϴ� �ð� (���̵� ��)
    public float fadeOutDuration; // ���İ��� �ٽ� ���� ������ ���ƿ��� �ð� (���̵� �ƿ�)
    private Coroutine fadeCoroutine;

    public GameManager gameManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        whiteScreen.SetActive(false);
    }

    private void Update()
    {
        KeyInput();

        if(!isSkillb && !isSkillc)
        {
            Move();
        }
        UpdateLayer();

        if(hp < 0)
        {
            StartCoroutine(Die());
        }

        if (keySkilla && !isSkillb && !isSkillc) // ��ų 1��
        {
            if(gemPoint >= 1)
            {
                gemPoint--;
                gameManager.GemUpdate(gemPoint);
                ThrowGemBomb();
            }
            
        }

        if(keySkillb && !isJump && !isSkillb && !isSkillc)  // ����, ��ų2, ��ų3 ��� �߿��� ��� �Ұ�
        {
            if (gemPoint >= 3)
            {
                gemPoint -= 3;
                gameManager.GemUpdate(gemPoint);
                StartCoroutine(LaserThrow());
            }
        }

        if (keySkillc && !isJump && !isSkillb && !isSkillc) // ����, ��ų2, ��ų3 ��� �߿��� ��� �Ұ�
        {
            if (gemPoint >= 5)
            {
                gemPoint -= 5;
                gameManager.GemUpdate(gemPoint);
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(WhiteScreenFade());
            }
                
        }
    }

    private void FixedUpdate()
    {
        attackTimer += Time.fixedDeltaTime;
        if (attackTimer >= attackSpeed && isAttack)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    private void KeyInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        movedown = Input.GetKey(KeyCode.DownArrow);
        keyJump   = Input.GetButtonDown("Jump");
        isAttack = Input.GetButton("Attack");
        keySkilla = Input.GetKeyDown(KeyCode.Z);
        keySkillb = Input.GetKeyDown(KeyCode.X);
        keySkillc = Input.GetKeyDown(KeyCode.C);
    }

    private void Move()
    {
        if(!isRope)
        {
            Vector3 movement = new Vector3(moveX, 0f, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);

            // �̵� ���⿡ ���� ������Ʈ�� ����
            if (movement.x < 0) // �������� �̵� ���� ��
            {
                transform.localScale = new Vector3(-1.8f, 1.8f, 1f); // ������Ʈ ũ�Ⱑ ������
            }
            else if (movement.x > 0) // ���������� �̵� ���� ��
            {
                transform.localScale = new Vector3(1.8f, 1.8f, 1f);
            }

            if (keyJump && !isJump)
            {
                isJump = true;

                if (movedown)
                {
                    isUnderJump = true;
                    rigid.AddForce(Vector3.up * jumpForce / 2, ForceMode2D.Impulse);
                }
                else
                {
                    rigid.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                rigid.AddForce(Vector3.down * 2.33f, ForceMode2D.Force);
            }
        }
    }

    void Attack()
    {
        GameObject shotBullet = Instantiate(bullet[bulletNumber], bulletPosition.position, Quaternion.identity);
        Rigidbody2D rb = shotBullet.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            rb.velocity = new Vector2(40f * direction, 0f);

            if (direction < 0) // ���� ������ ��� �������� ������Ŵ
            {
                shotBullet.transform.localScale = new Vector3(-3f, 3f, 1f);  // ������ ���·� �߻�Ǵ� �Ѿ��� ũ�� ����
            }
        }
    }

    void UpdateLayer()
    {
        if (isRope || isUnderJump)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerRope");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }

        if(isDamage && !isRope)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerOnDamage");
        }
    }

    public void DecreaseHp(int amount) // ������ �Դ� �Լ�
    {
        if (!isDamage)
        {
            StartCoroutine(OnDamage());
            hp -= amount;
            if (hp < 0)
            {
                hp = 0;
            }
        }
    }

    IEnumerator OnDamage()
    {
        isDamage = true;

        Renderer renderer = GetComponent<Renderer>();
        Material originalMaterial = renderer.material;
        Material blinkMaterial = new Material(originalMaterial);
        blinkMaterial.color = Color.red;

        float blinkDuration = 0.1f; // ������ ���� (��)
        int blinkCount = 5; // ������ Ƚ��

        for (int i = 0; i < blinkCount; i++)
        {
            renderer.material = blinkMaterial;
            yield return new WaitForSeconds(blinkDuration);

            renderer.material = originalMaterial;
            yield return new WaitForSeconds(blinkDuration);
        }

        isDamage = false;
        renderer.material = originalMaterial;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0f);
        Destroy(gameObject);
    }

    private void ThrowGemBomb() // Skill 1��
    {
        GameObject shotBullet = Instantiate(gemBomb, bulletPosition.position, Quaternion.identity);
        Rigidbody2D rb = shotBullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            rb.velocity = new Vector2(40f * direction, 0f);

            if (direction < 0) // ���� ������ ��� �������� ������Ŵ
            {
                shotBullet.transform.localScale = new Vector3(-1f, 1f, 1f);  // ������ ���·� �߻�Ǵ� �Ѿ��� ũ�� ����
            }
        }
    }


    private IEnumerator LaserThrow()
    {
        isSkillb = true;
        isDamage = true; // ��ų ��� �� ����

        // 1�� ������Ʈ Ȱ��ȭ
        laserTrajectory.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        laserTrajectory.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        laserBeam.SetActive(true);

        float elapsedTime = 0f; // Ÿ�̸� ���� �ʱ�ȭ
        while (elapsedTime < 2f) // 2�ʱ����� �ݺ�  @@ �� ���ڶ� �Ʒ��� ��� �ð� ������Ʈ�� �ð� �¾ƾ� ��
        {
            laserCollider.enabled = true; // Collider�� Ȱ��ȭ
            yield return new WaitForSeconds(0.08f); 
            laserCollider.enabled = false; // Collider�� ��Ȱ��ȭ
            yield return new WaitForSeconds(0.08f); 

            elapsedTime += 0.1f; // ��� �ð� ������Ʈ
        }

        laserBeam.SetActive(false);
        isSkillb = false;
        isDamage = false;  // ���� ����
    } 


    private IEnumerator WhiteScreenFade()
    {
        isSkillc = true;
        isDamage = true;  // ��ų ��� �� ����

        // ���̵� �� (ȭ���� ������ �����)
        whiteScreen.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            SetWhiteScreenAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���İ��� �ִ밡 �� �� BoxCollider2D Ȱ��ȭ
        whiteScreenDmg.SetActive(true);

        // 0.1�� ������ �� BoxCollider2D ��Ȱ��ȭ
        yield return new WaitForSeconds(0.1f);
        whiteScreenDmg.SetActive(false);

        isSkillc = false;
        isDamage = false; // ���� ����

        // ���̵� �ƿ� (ȭ���� ������ ��ο���)
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            SetWhiteScreenAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���̵尡 ���� �� ȭ�� ������ ��Ȱ��ȭ
        whiteScreen.SetActive(false);
        
    }

    private void SetWhiteScreenAlpha(float alpha)
    {
        Renderer renderer = whiteScreen.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.a = alpha;
        renderer.material.color = color;
    }






    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gem"))
        {
            if (gemPoint < 6) // ���� 6�� �����̸�
            {
                gemPoint++;
                gameManager.GemUpdate(gemPoint);
            }

        }
    }


}
