using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerAttackEvent : MonoBehaviour
{
    private PlayerBombFire ThrowEvent;

    public void Start()
    {
        ThrowEvent = GetComponentInParent<PlayerBombFire>();
    }
    public void AttackEvent()
    {
        ThrowEvent.Throw();
    }
}

