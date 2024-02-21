using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType 
{ 
    Health,   // 체력이 꽉찬다.
    Stamina,  // 스태미나 꽉찬다.
    Bullet    // 현재 들고있는 총의 총알이 꽉찬다.
}

public class Item
{
    public ItemType ItemType;
    public int Count;

    // 기본생성자
    public Item(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }


    public bool TryUse() // Try 형식은 불형식을 반영한다.
    {
        switch (ItemType) 
        {
            case ItemType.Health:
            {
                if (Count == 0)
                {
                    return false;
                }
                Count -= 1;
                // Todo: 플레이어 체력 꽉차기
                break;
            }
            case ItemType.Stamina:
            {
                // Todo: 플레이어 스태미너 꽉차기
                break;
            }
            case ItemType.Bullet:
            {
                // Todo: 플레이어가 현재 들고있는 총의 총알이 꽉찬다.
                break;
            }
        }

        return true;
    }
    
}
