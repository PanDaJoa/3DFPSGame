using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum ItemState
{
    Idle, // 대기(플레이어와의 거리를 체크한다)
    // if(충분히 가까워 지면..)
    Trace // 날라오는 상태 (0.6초에 걸쳣거 Slerp로 플레이어에게 날아온다.)
}
public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;

    // 실습 과제 36. 상태패턴 방식으로 일정 거리가 되면 아이템이 Slerp로 날아오게 하기 (대기 상태, 날아오는 상태)
    private ItemState _itemState = ItemState.Idle;

    public Transform _player;
    public float ExplosionRadius = 3;
    public float EatDistance = 10;
    public float MoveSpeed = 15f;



    public Vector3 _startPoisition;
    private const float TRACE_DURATION = 0.6f;
    private float _progress = 0;

    private CharacterController _characterController;
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _startPoisition = transform.position;
    }

    private void Update()
    {
      //  int layer = LayerMask.GetMask("Player");
       // Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, layer);

        switch (_itemState)
        {
            case ItemState.Idle:
                Idle();
                break;
            case ItemState.Trace:
                Trace();
                break;

        }
    }
    public void Init()
    {
        _progress = 0f;
        _traceCoroutine = null;
    }
    private void Idle()
    {
        float distance = Vector3.Distance(_player.position, transform.position);
        if (distance <= EatDistance)
        {
            _itemState = ItemState.Trace;
        }
        /*if (Vector3.Distance(_player.position, transform.position) <= EatDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            _itemState = ItemState.Trace;
        }*/
    }

    private Coroutine _traceCoroutine;
    private void Trace()
    {

        if (_traceCoroutine == null)
        {
            _traceCoroutine = StartCoroutine(Trace_Coroutine());
        }
        /*
        }*/



        // Slerp( 시작점, 종료점 , 진행도)
        // 진행도를 누적할 시간

        /*Vector3 dir = _player.transform.position - this.transform.position;
        dir.Normalize();

        transform.position = Vector3.Slerp(StartPoisition,_player.transform.position, _flyingProgress);
        _characterController.Move(dir * MoveSpeed *Time.deltaTime);
        transform.LookAt(_player);*/

    }

    private IEnumerator Trace_Coroutine()
    {
        
        while (_progress < 0.6)
        {
            _progress += Time.deltaTime / TRACE_DURATION;
            transform.position = Vector3.Slerp(_startPoisition, _player.position, _progress);
            yield return null;
        }

        // Trace가 완료된 후의 로직 추가
        ItemManager.Instance.AddItem(ItemType);
        // 2. 사라진다.        
        gameObject.SetActive(false);
    }
}


