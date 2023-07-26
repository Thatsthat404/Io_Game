using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class Menu_GameManager : MonoBehaviour
{
    public SoundManager soundManager;
    public GameObject soundPanel;

    public void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(soundPanel.activeSelf)
            {
                soundPanel.SetActive(false);
            }
        }
    }



    public void SceneToPlay() // Main Menu�� ���� ���� ��ư�� ����Ǿ� ����
    {
        soundManager.SaveSoundSettings();
        SceneManager.LoadScene("Play");
    }

    public void ToggleUIPanel()
    {
        soundPanel.SetActive(!soundPanel.activeSelf);
    }

    public void QuitGame() // ���� ���� ��ư Ŭ��(���� ����)
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}

