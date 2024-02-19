using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [Range(0, 100)]
    public float Health;
    public float MaxHealth = 100;
    public Slider HealthSliderUI;
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
}
