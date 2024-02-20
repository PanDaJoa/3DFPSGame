using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{

    

    // 무기 인벤토리
    public List<Gun> GunInventory;
   

    
    public Gun CurrentGun;       // 현재 들고있는 총
    private int currentGunIndex; //현재 들고있는 총의 순서
    
    // 무기 이미지 UI
    public Image GunImageUI; // 총 이미지

    // 목표: 마우스 왼족 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;

    public float Timer = 0;





    // 총알집
    public int BulletBox;
    // - 총알 개수 텍스트 UI
    public Text bulletUI;
    public Text ReloadUI;




    public bool _isReloading = false; // 재장전 중이냐?

    private void Start()
    {
        RifreshGun();
    }


    private void Update()
    {
        Timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftBracket)) // '['
        {
            // 뒤로가기 
            currentGunIndex--;
            if (currentGunIndex < 0)
            {
                currentGunIndex = GunInventory.Count - 1;
          
            }
            CurrentGun = GunInventory[currentGunIndex];
            RifreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) // ']'
        {
            // 앞으로 가기
            currentGunIndex++;
            if (currentGunIndex >= GunInventory.Count)
            {
                currentGunIndex = 0;
                
            }
            CurrentGun = GunInventory[currentGunIndex];
            RifreshGun();
            RefreshUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGunIndex = 0;
            CurrentGun = GunInventory[0];
            RifreshGun();
            RefreshUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentGunIndex = 1;
            CurrentGun = GunInventory[1];
            RifreshGun();
            RefreshUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentGunIndex = 2;
            CurrentGun = GunInventory[2];
            RifreshGun();
            RefreshUI();
        }
        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 총알 쿨타임 && 총알이 0보다 클 때 발사
        if (Input.GetMouseButton(0) && Timer >= CurrentGun.FireCoolTime && CurrentGun.BulletRemainCount > 0)
        {
            // 재장전 중이라면 재장전 코루틴을 중지
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
                ReloadUI.text = "";
            }
            CurrentGun.BulletRemainCount--;
            Timer = 0;

            // 2. 레이(광선)을 생성하고,위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딛힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)
                {
                    hitObject.Hit(CurrentGun.Damage);
                }

                // 실습 과제 18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                Monster monster = GetComponent<Monster>();

                // 5. 부딛힌 위치에 (총알이 튀는) 이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딛힌 위치에 법선 벡터로 한다.반동
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
            RefreshUI();
        }
        // R 키를 누르면 재장전을 시작
        if (Input.GetKeyDown(KeyCode.R) && CurrentGun.BulletMaxCount > 0)
        {
            _isReloading = true;
            StartCoroutine(Reload_Coroutine(CurrentGun.Relode));
            ReloadingUI();
        }

    }

    private IEnumerator Reload_Coroutine(float delayTime) // 재장전 코루틴
    {

        yield return new WaitForSeconds(delayTime);

        if (_isReloading)
        {
            CurrentGun.BulletRemainCount = CurrentGun.BulletMaxCount; // 총알 장전
            RefreshUI(); // 총알 장전 UI
            ReloadUI.text = ""; // 1.5초 후 공백으로 만듬
            _isReloading = false; // false는 재장전이 아닐 때를 의미함
        }
    }

    private void RefreshUI()
    {
        GunImageUI.sprite = CurrentGun.ProfileImage;
        bulletUI.text = $"{CurrentGun.BulletRemainCount}/{CurrentGun.BulletMaxCount}";
    }
    private void ReloadingUI()
    {
        ReloadUI.text = $"{"재장전 중..."}";
    }

    public void RifreshGun()
    {
        foreach (Gun g in GunInventory)
        {
            if (g == CurrentGun)
            {
                g.gameObject.SetActive(true);
            }
            else
            {
                g.gameObject.SetActive(false);
            }

        }
    }

}
