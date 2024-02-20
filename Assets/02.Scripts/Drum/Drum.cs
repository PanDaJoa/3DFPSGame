using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Drum : MonoBehaviour, IHitable
{
    public GameObject DestroyEffect;
    public float UpPower = 50f;
    public float RotationPower = 10f;

    public int hitCount = 0;
    public int Damage = 70;
    public float ExplosionRadius = 5; //데미지 범위

    private bool isExploding = false;
    public void Hit(int damage)
    {
        hitCount += 1;
        if (hitCount >= 3)
        {
            Explosion();
        }
    }
    private void Explosion()
    {
        if (isExploding)
        {
            return;
        }

        isExploding = true;

        GameObject destroyEffect = Instantiate(DestroyEffect);
        destroyEffect.transform.position = this.transform.position;

        //Instantiate(DestroyEffect, transform.position, transform.rotation);

        //실습 과제 22. 드럼통 폭발할 때 주변 Hitable한 Monster와 Player에게 데미지 70
        // 1. 폭발 범위 내 콜라이더 찾기
        int findlayer = LayerMask.GetMask("Monster") | LayerMask.GetMask("Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, findlayer);

        // 2. 콜라이더 내에서 Hitable 찾기
        foreach (Collider collider in colliders)
        {
            IHitable hitable = collider.gameObject.GetComponent<IHitable>();
            if (hitable != null)
            {
                // 3. 데미지 주기
                hitable.Hit(Damage);
            }
            
        }
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.AddForce(Vector3.up * UpPower, ForceMode.Impulse);
            rigidbody.AddTorque(Vector3.right * RotationPower, ForceMode.Impulse);
        }

        // Physics.OverlapSphere = 일정반경내 콜라이더를 찾는다.
        // 실습 과제 23.드럼통 폭발할 때 주변 드럼통도 같이 폭발되게 구현
        int environmentlayer = LayerMask.GetMask("Environment");
        Collider[] environmentcolliders = Physics.OverlapSphere(transform.position, ExplosionRadius, environmentlayer);
        foreach (Collider c in environmentcolliders)
        {
            Drum drum = null;
            if (c.TryGetComponent<Drum>(out drum))
            {
                drum.Explosion();
            }
        }


        StartCoroutine(DestroyAfterSeconds(3));
    }

    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
       
        Destroy(gameObject);
    }   
}
