using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GunTpye 
{
    Rifle,  // 따발총
    Sniper, // 스나이퍼
    Pistol,
}

public class Gun : MonoBehaviour
{
    public GunTpye GTpye;

    public Sprite ProfileImage;
    public Sprite ZoomImage;
    
    

    // 공격력
    public int Damage = 10;

    // 발사 쿨타임
    public float FireCoolTime = 0.2f;

    // 총알 개수
    public int BulletRemainCount = 30;
    public int BulletMaxCount = 30;

    // 재장전 시간
    public float Relode = 1.5f;

    private void Start()
    {
        BulletRemainCount = BulletMaxCount;
        
    }
}
