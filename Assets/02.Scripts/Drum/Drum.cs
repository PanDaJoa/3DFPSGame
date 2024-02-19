using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drum : MonoBehaviour, IHitable
{
    private int hitCount = 0;

    public void Hit(int damage)
    {
        hitCount += 1;
        if (hitCount >= 3)
        {
            Destroy(gameObject);
        }
    }
}
