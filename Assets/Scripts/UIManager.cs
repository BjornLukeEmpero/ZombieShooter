using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public AudioSource audioSource;
    //오디오 배경음악

    public void GoTitle()
    {
        SceneManager.LoadScene("_Title");
    }
    //타이틀 화면으로 돌아가기

    public void StartGame()
    {
        SceneManager.LoadScene("Ingame");
    }
    //게임 시작
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying
            = false;
#else
        Application.Quit();
#endif
    }
    //게임 종료
}
