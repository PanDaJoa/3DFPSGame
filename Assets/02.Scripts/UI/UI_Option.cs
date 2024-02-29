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
    }
}
