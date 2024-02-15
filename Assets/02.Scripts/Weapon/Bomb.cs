using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject BombEffectPrefab;
    // 풀 사이즈
    public int PoolSize = 10;

    //
    private List<GameObject> _bombPool;

    private void Awake()
    {
        _bombPool = new List<GameObject>();

        // 3. 총알 프리팹으로부터 총알을 풀 사이즈만큼 생성해준다.
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bomb = Instantiate(BombEffectPrefab);


            
            _bombPool.Add(bomb);

            bomb.SetActive(false);
        }
    }

    //플페이어를 제외하고 물체에 닿으면(=충돌이 일어나면)
    //자기 자신을 사라지게 하는 코드 작성
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Player")
        {
            Instantiate(BombEffectPrefab, transform.position, transform.rotation);
            
            /*GameObject effect = Instantiate(BombEffectPrefab);
            effect.transform.position = this.gameObject.transform.position;*/

            Destroy(this.gameObject);
        }
    }
}
