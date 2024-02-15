using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    public float destroyTime = 1.5f;
    private float _timer = 0;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= destroyTime)
        {
            Destroy(this.gameObject);
        }
    }

}
