using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔을 다르게해서 구현)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고 사라진다.
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 나의 거리를 알고 싶다.

        if (other.CompareTag("Player"))
        {



            // 1. 아이템 매니저(인벤토리)에 추가하고,
            if(ItemType == ItemType.Health)
            {
                ItemManager.Instance.AddItem(ItemType.Health);
                ItemManager.Instance.RefreshUI();
                // 플레이어와 나의 거리를 알고 싶다.
                float distance = Vector3.Distance(other.transform.position, transform.position);
                Debug.Log(distance);
            }
            if (ItemType == ItemType.Stamina)
            {
                ItemManager.Instance.AddItem(ItemType.Stamina);
                ItemManager.Instance.RefreshUI();
                // 플레이어와 나의 거리를 알고 싶다.
                float distance = Vector3.Distance(other.transform.position, transform.position);
                Debug.Log(distance);
            }
            if (ItemType == ItemType.Bullet)
            {
                ItemManager.Instance.AddItem(ItemType.Bullet);
                ItemManager.Instance.RefreshUI();
                // 플레이어와 나의 거리를 알고 싶다.
                float distance = Vector3.Distance(other.transform.position, transform.position);
                Debug.Log(distance);
            }


            // 2. 사라진다.
            Destroy(gameObject);
        }
    }

    // 실습 과제 31. 몬스터가 죽으면 아이템이 드랍 (Health: 20% Stamina: 20% Bullet:10%)
    // 실습 과제 32. 일정 거리가 되면 아이템이 Slerp으로 날라오게 하기 (중간점 랜덤)
}
