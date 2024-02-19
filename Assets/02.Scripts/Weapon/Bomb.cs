using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject BombEffectPrefab;
    internal bool activeInHierarchy;

    public float ExplosionRadius;
    // 구현 순서:
    // 1. 터질 때
    // 2. 범위안에 있는 모든 콜라이더를 찾는다.
    // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾는다.
    // 4. Hit() 한다.

    public int Damage = 60;
    //플페이어를 제외하고 물체에 닿으면(=충돌이 일어나면)
    //자기 자신을 사라지게 하는 코드 작성
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Player")
        {
            Instantiate(BombEffectPrefab, transform.position, transform.rotation);

            IHitable hitable = collision.collider.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.Hit(Damage);
            }
            

            /*GameObject effect = Instantiate(BombEffectPrefab);
            effect.transform.position = this.gameObject.transform.position;*/

            this.gameObject.SetActive(false); // 창고에 넣는다.
        }
    }
}
