using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum ItemState
{
    Idle,   // 대기
    Trace   // 추적
}
public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;
    private ItemState _itemState = ItemState.Idle;

    public Transform _target;
    public float ExplosionRadius = 3;
    public float FindDistance = 7;
    public float MoveSpeed = 50f;

    private Vector3 _itemStartPosition;
    private Vector3 _itemEndPosition;
    private const float item_DURATION = 0.2f;
    private float _knockbackProgress = 0f;
    

    private CharacterController _characterController;
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        int layer = LayerMask.GetMask("Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, layer);

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

    private void Idle()
    {
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            _itemState = ItemState.Trace;
        }
    }
    private void Trace()
    {
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.Normalize();

        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        transform.LookAt(_target);

    }
    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            float distance = Vector3.Distance(other.transform.position, transform.position);
            Debug.Log(distance);
            ItemManager.Instance.AddItem(ItemType);
            ItemManager.Instance.RefreshUI();

            gameObject.SetActive(false);

        }


    }

}


