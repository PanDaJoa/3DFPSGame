using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]
    public float Health;
    public float MaxHealth = 100;
    public Slider HealthSliderUI;

    public GameObject ItemPrefabHealth;
    public GameObject ItemPrefabStamina;
    public GameObject ItemPrefabBullet;

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0) 
        {
            Destroy(gameObject);
            MakeItem();
        }
    }
    public void Init()
    {
        Health = MaxHealth;   
    }
    void Start()
    {
        Init();
        
    }

    void Update()
    {
        HealthSliderUI.value = Health / MaxHealth;
    }
    public void MakeItem()
    {
        int randomvalue = Random.Range(0, 10);
        Debug.Log(randomvalue);
        // 목표: 50% 확률로 체력 올려주는 아이템, 50% 확률로 이동속도 올려주는 아이템
        if (randomvalue < 2)
        {
            // - 체력을 올려주는 아이템만들기
            GameObject item = Instantiate(ItemPrefabHealth);
            // - 위치를 나의 위치로 수정
            item.transform.position = this.transform.position;
        }
        else if(randomvalue < 4)
        {
            // - 스태미나를 올려주는 아이템만들기
            GameObject item = Instantiate(ItemPrefabStamina);
            // - 위치를 나의 위치로 수정
            item.transform.position = this.transform.position;
        }
        else if (randomvalue < 5)
        {
            // - 총알을 올려주는 아이템만들기
            GameObject item = Instantiate(ItemPrefabBullet);
            // - 위치를 나의 위치로 수정
            item.transform.position = this.transform.position;
        }
    }
}
