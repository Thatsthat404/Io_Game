using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // GameManager�� �ν��Ͻ��� ������ ����
    private bool isPaused = false;
    public bool IsPaused { get => isPaused; } // isPaused�� �ܺο��� �б⸸ �����ϵ��� getter�� ����


    public PlayerBehavior player;
    public Image hpBar;
    public GameObject []gemBar;
    public GameObject playOption;
    public GameObject soundPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }


    private void Update()
    {
        // #. �÷��̾� ���� ���� ������Ʈ
        hpBar.fillAmount = (float)player.hp * 0.01f;



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(); 
        }
    }




    public void GemUpdate(int count)
    {
        for(int i = 0; i < 5; i++)
        {
            if(i < count)
            {
                gemBar[i].SetActive(true);
            }
            else
            {
                gemBar[i].SetActive(false);
            }
           
        }
    }













    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // ���� �Ͻ�����
            playOption.SetActive(true);
        }
        else
        {
            if(soundPanel.activeSelf)
            {
                isPaused = true;
                soundPanel.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f; // ���� ����
                playOption.SetActive(false);
            }
        }
    }

    public void SceneToPlay() // Option â�� "�ٽ��ϱ�" ��ư�̶� ����Ǿ� ����
    {
        TogglePause();
        SceneManager.LoadScene("Play");
           
    }

    public void SceneToMainTitle() // Option â�� "����ȭ��" ��ư�̶� ����Ǿ� ����
    {
        TogglePause();
        SceneManager.LoadScene("Menu");
    }

    public void ToggleUIPanel()
    {
        soundPanel.SetActive(!soundPanel.activeSelf);
    }
}
