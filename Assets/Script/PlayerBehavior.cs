using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    private float attackTimer = 0f;

    public float moveX;  // �¿� Ű �Է�
    public bool movedown;  // �Ʒ�����Ű �Է�
    public bool keyJump; // ���� Ű �Է�
    public bool isJump; // ����������
    private bool isAttack; // ���� ���� ������
    public bool isRope; // ������ Ÿ�� �ִ���
    public bool isUnderJump; // ���� ���� �ϰ� �ִ���
    public bool positionUpDown; // ���� ��ܿ� �ִ��� �ϴܿ� �ִ���


    private Rigidbody2D rigid;
    public Transform bulletPosition;
    public GameObject[] bullet; // �Ѿ� ������ ���� �迭

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        KeyInput();
        Move();
        UpdateLayer();

        if(hp < 0)
        {
            StartCoroutine(Die());
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
        keyJump = Input.GetButtonDown("Jump");
        isAttack = Input.GetButton("Attack");
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
}






