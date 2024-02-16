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
    public bool _isReloading;

    private IEnumerator Reload_Coroutine(float delayTime)
    {
        
        yield return new WaitForSeconds(delayTime);

        if (_isReloading)
        {
            BulletRemainCount = BulletMaxCount;
            RefreshUI();
            ReloadUI.text = "";
            _isReloading = false; // 재장전이 완료되었으므로 false로 설정
        }
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태
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
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                // 5. 부딛힌 위치에 (총알이 튀는) 이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딛힌 위치에 법선 벡터로 한다.반동
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
            RefreshUI();
        }
        
        if (Input.GetKeyDown(KeyCode.R) && BulletMaxCount > 0)
        {
            // BulletRemainCount = BulletMaxCount;
            // RefreshUI();
            _isReloading = true;
            StartCoroutine(Reload_Coroutine(1.5f));
            ReloadingUI();
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
