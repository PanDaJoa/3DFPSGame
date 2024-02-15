using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject BombEffectPrefab;
    internal bool activeInHierarchy;


    //플페이어를 제외하고 물체에 닿으면(=충돌이 일어나면)
    //자기 자신을 사라지게 하는 코드 작성
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Player")
        {
            Instantiate(BombEffectPrefab, transform.position, transform.rotation);

            /*GameObject effect = Instantiate(BombEffectPrefab);
            effect.transform.position = this.gameObject.transform.position;*/

            this.gameObject.SetActive(false); // 창고에 넣는다.
        }
    }
}
