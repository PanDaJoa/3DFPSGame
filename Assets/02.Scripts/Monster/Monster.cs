using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterState // 몬스터의 상태
{
    Idle,           // 대기
    Trace,          // 추적
    Attack,         // 공격
    ComeBack,         // 복귀
    Damaged,        // 공격 당함
    Die             // 사망
}

public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    /***********************************************************************/

    private CharacterController _characterController;

    private Transform _target;         // 플레이어
    public float FindDistance = 5f;  // 감지 거리
    public float AttackDistance = 2f;  // 공격 범위 
    public float MoveSpeed = 4f;  // 이동 상태
    public Vector3 StartPoisition;     // 시작 위치
    public float MoveDistance = 40f; // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f;
    public int Damage = 10;

    private float AttackTimer  = 0; // 공격타임
    public float Attackdelay  = 1f; // 공격딜레이

    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.2f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 0.5f;

    private MonsterState _currentState = MonsterState.Idle;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        StartPoisition = transform.position;

        Init();
        
    }

    public void Init()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth;  // 0 ~ 1

        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴 
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다.

        switch (_currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Trace:
                Trace();
                break;

            case MonsterState.ComeBack:
                Comeback(); 
                break;

            case MonsterState.Attack:
                Attack();
                break;

            case MonsterState.Damaged:
                Damaged();
                break;
        }
        
    }

    private void Idle()
    {
        // todo: 몬스터의 Idle 애니메이션 재생
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _currentState = MonsterState.Trace;
        }
    }

    private void Trace()
    {
        // Trace 상태일때의 행동 코드를 작성

        // 플레이어게 다가간다.
        // 1. 방향을 구한다. (target - me)
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        // 3. 쳐다본다.
        transform.forward = dir; //(_target) 
        if (Vector3.Distance(transform.position, StartPoisition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> ComeBack");
            _currentState = MonsterState.ComeBack;
        }
        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _currentState = MonsterState.Attack;
        }
    }
    private void Attack()
    {
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            AttackTimer = 0f;
            Debug.Log("상태 전환: Attack -> Trace");
            _currentState = MonsterState.Trace;
        }

        AttackTimer += Time.deltaTime;
        if (AttackTimer >= Attackdelay)
        {
            IHitable playerHitable = _target.GetComponent<IHitable>();
            if (playerHitable != null)
            {
                AttackTimer = 0;
                Debug.Log("때렸다!");
                playerHitable.Hit(Damage);
            }
        }

    }
    private void Comeback()
    {
        Vector3 dir = StartPoisition - this.transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

        // 3. 쳐다본다.
        transform.forward = dir;

        // 몬스터가 시작 위치에 가까워졌을 때 Idle 상태로 전환
        if (Vector3.Distance(transform.position, StartPoisition) <= TOLERANCE)
        { 
            Debug.Log("상태 전환: Comeback -> idle");
            _currentState = MonsterState.Idle;
        }
    }
    private void Damaged()
    {
        // 1. Damaged 애니메이션 실행(0.5초)
        // 2. 넉백(lerp -> 0.5초)
        // 2-1. 넉백 시작/최종 위치를 구한다.
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();
            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;
        // 2-2. Lerp를 이용해 
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0;
            // 3. 다시 Trace로 전환
            Debug.Log("상태 전환: Damaged -> Trace");
            _currentState = MonsterState.Trace;
        }
  
    }
    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("상태 전환: Any -> Damaged");
            _currentState = MonsterState.Damaged;
        }
    }

    private void Die()
    {

        // 죽을때 아이템 생성
        ItemObjectFactory.Instance.MakePercent(transform.position);

        Destroy(gameObject);
    }









}









