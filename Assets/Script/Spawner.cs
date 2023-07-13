using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnSpeed; // �����ֱ�
    private float spawnTimer = 0f;


    public GameObject[] Monsters;
    public Transform[] spawnPositionUpDown;


    private void FixedUpdate()
    {
        spawnTimer += Time.fixedDeltaTime;
        if (spawnTimer >= spawnSpeed)
        {
            SpawnMonster();
            spawnTimer = 0f;
        }
    }

    private void SpawnMonster()
    {
        int randomPosition = Random.Range(0, 2);
        int randomMonster = Random.Range(0, 4);

        Transform spawnPosition = spawnPositionUpDown[randomPosition];

        if (randomMonster == 1)
        {
            Vector3 newPosition = spawnPosition.position + new Vector3(0f, 1f, 0f); // Y ��ǥ�� 1��ŭ ������Ŵ
            GameObject monster = Instantiate(Monsters[randomMonster], newPosition, Quaternion.identity);
        }
        else
        {
            GameObject monster = Instantiate(Monsters[randomMonster], spawnPosition.position, Quaternion.identity);
        }
    }

}
