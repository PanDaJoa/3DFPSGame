using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;
    private float eatDistance = 10.0f;  // 아이템을 먹을 수 있는 최소 거리
    private GameObject player;  // 플레이어 객체
    private bool isFollowingPlayer = false;

    private void Start()
    {
 
        player = GameObject.FindGameObjectWithTag("Player");  // 플레이어 객체를 가져옵니다.
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= eatDistance && !isFollowingPlayer)  // 거리가 eatDistance 이하이며, 아직 플레이어를 따라가지 않았으면,
        {
            StartCoroutine(MoveToPlayer());
            isFollowingPlayer = true;  // 아이템이 플레이어를 따라가기 시작했음을 표시합니다.
        }
    }

    private void EatItem()
    {
        if (ItemType == ItemType.Health)
        {
            ItemManager.Instance.AddItem(ItemType.Health);
            ItemManager.Instance.RefreshUI();
        }
        else if (ItemType == ItemType.Stamina)
        {
            ItemManager.Instance.AddItem(ItemType.Stamina);
            ItemManager.Instance.RefreshUI();
        }
        else if (ItemType == ItemType.Bullet)
        {
            ItemManager.Instance.AddItem(ItemType.Bullet);
            ItemManager.Instance.RefreshUI();
        }
        gameObject.SetActive(false); // 아이템 객체를 파괴합니다.
    }
    private Vector3 BezierLerp(Vector3 start, Vector3 center, Vector3 end, float progress)
    {
        Vector3 m0 = Vector3.Lerp(start, center, progress);
        Vector3 m1 = Vector3.Lerp(center, end, progress);
        return Vector3.Lerp(m0, m1, progress);
    }
    IEnumerator MoveToPlayer()
    {
        while (true)  // 무한 루프를 통해 아이템이 플레이어를 계속 따라가도록 합니다.
        {
            Vector3 start = transform.position;
            Vector3 end = player.transform.position;
            Vector3 center = (start + end) / 2 + new Vector3(0, 1, 0);

            for (float t = 0; t <= 1; t += Time.deltaTime)
            {
                transform.position = BezierLerp(start, center, end, t);
                yield return null;
            }
        }


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")  // 충돌한 객체의 태그가 "Player"일 때,
        {
            EatItem();  // 아이템을 먹습니다.
            StopCoroutine(MoveToPlayer()); // 아이템이 사라질 때 코루틴을 중지합니다
        }
    }
}
