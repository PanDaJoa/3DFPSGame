using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{
    // 목표: 마우스 왼족 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;

    public float Timer = 0;
    public float FireCoolTime = 0.2f;

    public float rebound = 2f; //반동 크기
    public float reboundDuration = 0.1f; //반동 지속
    public float reboundTime; //반동시간


    // 총알 개수
    public int BulletRemainCount = 30;
    // 최대총알숫자
    public int BulletMaxCount = 270;
    // 총알집
    public int BulletBox;
    // - 총알 개수 텍스트 UI
    public Text bulletUI;
    public Text ReloadUI;

    private Coroutine _reloadCoroutine;

    private const float Relode = 1.5f; // 재장전 시간
    public bool _isReloading = false; // 재장전 중이냐?

    public int Damage = 1;

    private void Update()
    {
        Timer += Time.deltaTime;
        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 총알 쿨타임 && 총알이 0보다 클 때 발사
        if (Input.GetMouseButton(0) && Timer >= FireCoolTime && BulletRemainCount > 0)
        {
            // 재장전 중이라면 재장전 코루틴을 중지
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
                ReloadUI.text = "";
            }
            BulletRemainCount--;
            Timer = 0;

            // 2. 레이(광선)을 생성하고,위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딛힌 대상의 정보를 받아온다.
            RaycastHit hitInfo; // RaycastHit 맞은 정보가 담겨져 있다.
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                //실습 과제 18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)  // 때릴 수 있는 친구인가요?
                {
                    hitObject.Hit(Damage);
                }
/*                
                 
                if(hitInfo.collider.CompareTag("Monster"))
                {
                    Monster monster = hitInfo.collider.GetComponent<Monster>();
                    monster.Hit(Damage);
                }*/

                // 5. 부딛힌 위치에 (총알이 튀는) 이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딛힌 위치에 법선 벡터로 한다.반동
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
            RefreshUI();
        }
        // R 키를 누르면 재장전을 시작
        if (Input.GetKeyDown(KeyCode.R) && BulletMaxCount > 0)
        {
            _isReloading = true;
            StartCoroutine(Reload_Coroutine(Relode));
            ReloadingUI();
        }
       
    }

    private IEnumerator Reload_Coroutine(float delayTime) // 재장전 코루틴
    {

        yield return new WaitForSeconds(delayTime);

        if (_isReloading)
        {
            BulletRemainCount = BulletMaxCount; // 총알 장전
            RefreshUI(); // 총알 장전 UI
            ReloadUI.text = ""; // 1.5초 후 공백으로 만듬
            _isReloading = false; // false는 재장전이 아닐 때를 의미함
        }
    }

    private void RefreshUI()
    {
        bulletUI.text = $"{BulletRemainCount}/{BulletMaxCount}";
    }
    private void ReloadingUI()
    {
        ReloadUI.text = $"{"재장전 중..."}";
    }


}
