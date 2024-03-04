using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Option : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnOptionButtonClickedAgain()
    {
        Debug.Log("계속하기.");

        GameManager.instance.Continue();

        Close();
    }
    public void OnOptionButtonClickedReplay()
    {
        Debug.Log("다시하기.");
    }
    public void OnOptionButtonClickedEnd()
    {
        Debug.Log("게임종료.");

        // 빌드 후 실행했을 경우 종료하는 방법
        Application.Quit();

#if UNITY_EDITOR
        // 유니티 에디터에서 실행했을 경우 종료하는 방법
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
