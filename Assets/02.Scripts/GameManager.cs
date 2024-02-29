using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 역할: 게임 관리자
// -> 게임 전체의 상태를 알리고, 시작과 끝을 텍스트로 나타낸다.
public enum GameState 
{
    Ready, // 대기
    Go,    // 시작
    Pause, // 일시정지
    Over,  // 오버
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    // 게임의 상태는 처음에 "준비" 상태
    public GameState State { get; private set; } = GameState.Ready;

    public TextMeshProUGUI StateTextUI;

    public UI_Option OptionUI;

    //private bool isGameActive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        StartCoroutine(Start_Coroutine());
        Refresh();
    }

    private IEnumerator Start_Coroutine()
    {
        // 게임 상태
        // 1. 게임 준비 상태 (Ready...)
        State = GameState.Ready;
        StateTextUI.gameObject.SetActive(true);
        // 2. 1.6초 후에 게임 시작 상태 (Go!)
        yield return new WaitForSeconds(1.6f);
        State = GameState.Go;
        Refresh();

        // 3. 0.4초 후에 텍스트 사라지고...
        yield return new WaitForSeconds(0.4f);
        StateTextUI.gameObject.SetActive(false);
    }

    //4. 플레이를 하다가 
    public PlayerMoveAbility Player;

    public void GameOver()
    {
        State = GameState.Over;
        StateTextUI.gameObject.SetActive(true);
        Refresh();
    }


    public void Refresh()
    {
        switch (State)
        {
            case GameState.Ready:
            {
                StateTextUI.text = "Ready...";
                StateTextUI.color = new Color32(0, 255, 181, 255);
                break;
            }

            case GameState.Go:
            {
                StateTextUI.text = "GO...";
                break;
            }

            case GameState.Over:
            {
                StateTextUI.text = "Over...";
                break;
            }
        }

    }
    public void Pause()
    {
        State = GameState.Pause;
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        State = GameState.Go;
        Time.timeScale = 1f;
    }

    public void OnOptionButtonClicked()
    {
        Debug.Log("옵션버튼을 클릭했습니다.");

        Pause();

        OptionUI.Open();
    }


}
