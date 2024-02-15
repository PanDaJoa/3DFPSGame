using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // 목표: 마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
    // 필요 속성
    // - 수류탄 프라팹
    public GameObject BombPrefeb;
    // - 수류탄 던지는 위치
    public Transform FirePosition;
    // - 수류탄 던지는 파워
    public float ThrowPower = 15f;

    [Header("Bomb카운터 UI")]
    public Text BombScoreTextUI;
    public int BombScore;
    public const int MaxBombScore = 3;


    private void Start()
    {
        BombScore = MaxBombScore;

        RefreshUI();
    }

    private void RefreshUI()
    {
        // UI 위에 Text로 표시하기;
        BombScoreTextUI.text = $"{BombScore + " / " + MaxBombScore}";
    }

    private void Update()
    {
        /* 수류탄 투척 */
        // 1. 마우스 오른쪽 버튼을 감지
        if (Input.GetMouseButtonDown(1))
        {
            // 수류탄 개수가 0보다 큰 경우에만 던질 수 있음
            if (BombScore > 0)
            {

                // 수류탄 개수 감소
                BombScore--;

                RefreshUI();

                // 2. 수류탄 던지는 위치에다가 수류탄 생성
                GameObject bomb = Instantiate(BombPrefeb);
                bomb.transform.position = FirePosition.position;

                // 3. 시선이 바라보는 방향(카메라가 바라보는 방향 = 카라의 전방) 으로 수류탄 투척
                Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
                rigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
            }
        }


       
    }
}
