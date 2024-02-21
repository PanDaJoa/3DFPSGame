using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider slider;
    public PlayerMoveAbility playerMove; // PlayerMove 스크립트에 접근하기 위한 변수

    void Start()
    {
        // PlayerMove 스크립트가 있는 게임 오브젝트를 찾아서 playerMove 변수에 할당
        playerMove = FindObjectOfType<PlayerMoveAbility>();
        // 슬라이더의 값을 플레이어의 현재 스태미너에 비례하도록 설정
        slider.value = playerMove.Stamina / 100;
    }

    void Update()
    {
        // 플레이어의 스태미너가 변경될 때마다 슬라이더의 값을 업데이트
        slider.value = playerMove.Stamina / 100;
    }
}