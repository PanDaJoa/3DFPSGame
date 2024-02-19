using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 목표: 키보드 방향키(wasd)를 누르면 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다.
    // 속성:
    // - 이동속도
    public float MoveSpeed = 5; // 일반 속도
    public float RunSpeed = 10; // 뛰는 속도

    public float Stamina = 100;             // 스태미나
    public const float MaxStamina = 100;    // 스태미나 최대량
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50;   // 초당 스태미나 충전량

    [Header("스태미나 슬라이더 UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;

    // 목표: 스페이스바를 누르면 캐릭터를 점프하고 싶다.
    // 필요 속성:
    // - 점프 파워 값
    public float JumpPower = 10f;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // 목표: 캐릭터에 중력을 적용하고 싶다.
    // 필요 속성:
    // -중력 값
    private float _gravity = -20;
    // - 누적할 중력 변수: y축 속도
    private float _yVelocity = 0f;
    // 구현 순서:
    // 1. 중력 가속도가 누적된다.
    // 2. 플레이어에게 y축에 있어 중력을 적용한다.


    // 목표: 벽에  닿아 있는 상태에서 스페이스바를 누르면 벽타기를 하고 싶다.
    // 필요 속성:
    // - 벽타기 파워
    public float ClimbingPower = 7f;

    // - 벽타기 스태미너 소모량 팩터
    public float ClimbingStatminaCosumeFactor = 3f;
    
    // - 벽타기 상태
    private bool _isClimbing = false;
    // 구현 순서

    public float Health;
    public float MaxHealth = 100;
    public Slider HealthSliderUI;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
       
    }

    private void Start()
    {
        Stamina = MaxStamina;
        Health = MaxHealth;
    }

    // 구현순서
    // 1. 키 입력 받기
    // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
    // 3. 이동하기
    void Update()
    {
        HealthSliderUI.value = Health / MaxHealth;
        float speed = MoveSpeed; // 5
        // 1. 만약 벽에 닿아 있는데
        if (Stamina > 0 && _characterController.collisionFlags == CollisionFlags.Sides)
        {
            
            // 2. [Spacebar] 버튼을 누르고 있으면
            if (Input.GetKey(KeyCode.Space))
            {
                // 3. 벽을 타겠다.
                
                _isClimbing = true;
                _yVelocity = ClimbingPower;                
            }            
        }


        // 1. 키 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 방향구하기
        Vector3 dir = new Vector3(x: h, y: 0, z: v);          // 로컬 좌표계 (나만의 동서남북)
        dir.Normalize();

        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
        dir = Camera.main.transform.TransformDirection(dir); // 글로벌 좌표계 (세상의 동서남북)

        
        
        if (_isClimbing || Input.GetKey(KeyCode.LeftShift))
        {
            float factor = _isClimbing ? ClimbingStatminaCosumeFactor : 1f;
            Stamina -= StaminaConsumeSpeed * factor * Time.deltaTime; // 초당 33씩 소모
            

            // 클라이밍 상태가 아닐때만 스피드 up!
            if (!_isClimbing && Stamina > 0)
            {
                speed = RunSpeed; // 10
            }
        }
        else
        {
            // - 아니면 스태미나가 소모 되는 속도보다 빠른 속도로 충전된다 (2초)
            Stamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전
        }
        Stamina = Mathf.Clamp(value: Stamina, min: 0, max: 100);

        StaminaSliderUI.value = Stamina / MaxStamina;


        // 구현 순서 : 
        if (_characterController.isGrounded)
        {
            _isJumping = false;
            _isClimbing = false;
            _yVelocity = 0f;
            JumpRemainCount = JumpMaxCount;
        }
/*
        if (_characterController.collisionFlags != CollisionFlags.None)
        {
            
            // 점프를 중단
            _isJumping = false;
            _yVelocity = -1f;
        }*/



        // 점프 구현 
        // 1. 만약에 [Spacebar] 버튼을 누르는 순간 && (땅이거나 or 점프 횟수가 남아있다면)
        if (Input.GetKeyDown(KeyCode.Space) && (_characterController.isGrounded ||(_isJumping && JumpRemainCount > 0))) // 누른 그 순간 땅일때 true
        {
           
                _isJumping = true;
                
                JumpRemainCount--;

                // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다.
                _yVelocity = JumpPower;
        }


 
        // 3.1 중력 적용
        // 1. 중력 가속도가 누적된다.
        _yVelocity += _gravity * Time.deltaTime;
       
        
        // 2. 플레이어에게 y축에 있어 중력을 적용한다.
         dir.y = _yVelocity;

        // 3-2. 이동하기
        //transform.position += speed * dir * Time.deltaTime;
        _characterController.Move(dir * speed * Time.deltaTime);
        

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CameraManager.Instance.SetCameraMode(CameraMode.FPS);
        }
        // 키보드 '0' 키를 누르면 TPS 모드
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraManager.Instance.SetCameraMode(CameraMode.TPS);
        }
    }
}
