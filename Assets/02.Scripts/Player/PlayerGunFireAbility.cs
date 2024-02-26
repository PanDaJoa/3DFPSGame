using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFireAbility : MonoBehaviour
{
    public Gun CurrentGun;        // 현재 플레이어가 들고 있는 총
    private int _currentGunIndex; // 현재 플레이어가 들고 있는 총의 인벤토리 내 인덱스

    private float _timer; // 시간 측정을 위한 타이머

    private const int DefaultFOV = 60; // 기본 시야 각도
    private const int ZoomFOV = 20; // 줌 모드일 때의 시야 각도
    private bool _isZoomMode = false;  // 현재 줌 모드인지 아닌지 표시
    private const float ZoomInDuration = 0.3f;
    private const float ZoomOutDuration = 0.2f;
    private float _zoomProgress; // 0 ~ 1

    public GameObject CrosshairUI; // 기본 조준점 UI 
    public GameObject CrosshairZoomUI; // 줌 모드일 때의 조준점 UI 

    public List<Gun> GunInventory; // 플레이어가 가지고 있는 총들의 인벤토리

    public ParticleSystem HitEffect; // 총알이 목표물에 맞았을 때의 이펙트

    public TextMeshProUGUI BulletTextUI; // 총알 개수를 표시하는 UI

    private bool _isReloading = false;      // 현재 재장전 중인지 표시
    public TextMeshProUGUI ReloadTextObject; // 재장전 중임을 표시하는 UI

    public Image GunImageUI; // 현재 총의 이미지를 표시하는 UI

    

    private void Start()
    {
        _currentGunIndex = 0;

        // 총알 개수 초기화
        RefreshUI();
        RefreshGun();
    }

    public void RefreshUI()
    {
        GunImageUI.sprite = CurrentGun.ProfileImage;
        BulletTextUI.text = $"{CurrentGun.BulletRemainCount}/{CurrentGun.BulletMaxCount}"; // 불렛 총알 수 이미지

        CrosshairUI.SetActive(!_isZoomMode);
        CrosshairZoomUI.SetActive(_isZoomMode);
    }

    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;

        _isZoomMode = false;
        RefreshZoomMode();
        RefreshUI();

        // R키 누르면 1.5초 후 재장전, (중간에 총 쏘는 행위를 하면 재장전 취소)
        yield return new WaitForSeconds(CurrentGun.ReloadTime);
        CurrentGun.BulletRemainCount = CurrentGun.BulletMaxCount;
        RefreshUI();

        _isReloading = false;
        
    }

    // 줌 모드에 따라 카메라 FOV 수정해주는 메서드
    private void RefreshZoomMode()
    {
        if (!_isZoomMode) // 줌이 아닐때
        {
            Camera.main.fieldOfView = DefaultFOV; // 기본 설정
        }
        else
        {
            Camera.main.fieldOfView = ZoomFOV;  // 줌 설정
        }
    }
    private void Update()
    {
        if (GameManager.instance.State != GameState.Go)
        {
            return;
        }
        // 마우스 휠 버튼 눌렀을 때 && 현재 총이 스나이퍼
        if (Input.GetMouseButtonDown(1) && CurrentGun.GType == GunType.Sniper)
        {
            _isZoomMode = !_isZoomMode; // 줌 모드 뒤집기
            _zoomProgress = 0f;
            RefreshUI();
            
        }

        if (CurrentGun.GType == GunType.Sniper && _zoomProgress < 1)
        {
            // Lerp
            if (_isZoomMode) // 줌인
            {
                _zoomProgress += Time.deltaTime / ZoomInDuration;
                Camera.main.fieldOfView = Mathf.Lerp(DefaultFOV, ZoomFOV, _zoomProgress);
            }
            else
            {
                _zoomProgress += Time.deltaTime / ZoomOutDuration;
                Camera.main.fieldOfView = Mathf.Lerp(ZoomFOV, DefaultFOV, _zoomProgress);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket)) // '['
        {
            // 뒤로가기 
            _currentGunIndex--;
            if (_currentGunIndex < 0)
            {
                _currentGunIndex = GunInventory.Count - 1;
            }
            CurrentGun = GunInventory[_currentGunIndex];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
            
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) // ']'
        {
            // 앞으로 가기
            _currentGunIndex++;
            if (_currentGunIndex >= GunInventory.Count)
            {
                _currentGunIndex = 0;
            }
            CurrentGun = GunInventory[_currentGunIndex];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentGunIndex = 0;
            CurrentGun = GunInventory[0];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentGunIndex = 1;
            CurrentGun = GunInventory[1];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentGunIndex = 2;
            CurrentGun = GunInventory[2];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
            
        }

        if (Input.GetKeyDown(KeyCode.R) && CurrentGun.BulletRemainCount < CurrentGun.BulletMaxCount)
        {
            if (!_isReloading)
            {
                StartCoroutine(Reload_Coroutine());
            }
        }

        ReloadTextObject.gameObject.SetActive(_isReloading);


        _timer += Time.deltaTime;

        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태
        if (Input.GetMouseButton(0) && _timer >= CurrentGun.FireCooltime)
        {
            // 총알이 없을 경우 총을 쏘면 재장전
            if (CurrentGun.BulletRemainCount <= 0)
            {
                if (!_isReloading)
                {
                    StartCoroutine(Reload_Coroutine());
                }
                return;
            }
            // 재장전 취소
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
            }

            CurrentGun.BulletRemainCount -= 1;
            RefreshUI();

            _timer = 0;

            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딛힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                //실습 과제 18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)  // 때릴 수 있는 친구인가요?
                {
                    hitObject.Hit(CurrentGun.Damage);
                }


                // 5. 부딛힌 위치에 (총알이 튀는)이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딛힌 위치의 법선 벡터로 한다.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
        }

    }

    private void RefreshGun()
    {
        foreach (Gun gun in GunInventory)
        {
            /**if (gun == CurrentGun)
            {
                gun.gameObject.SetActive(true);
            }
            else
            {
                gun.gameObject.SetActive(false);
            }**/
            gun.gameObject.SetActive(gun == CurrentGun);
        }
    }
}
