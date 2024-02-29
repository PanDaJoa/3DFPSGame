using UnityEngine;
using System.Collections.Generic;

public class BloodFactory : MonoBehaviour
{
    public static BloodFactory Instance { get; private set; }

    [Header("피 효과 프리팹")]
    public GameObject BloodPrefab;

    private List<GameObject> _bloodpool;
    public int PoolSize = 10;


    // Todo. 오브젝트 풀링 적용해보세요.

    private void Awake()
    {
        Instance = this;

        _bloodpool = new List<GameObject>();

        for (int i = 0; i < PoolSize; ++i)
        {
            GameObject bloodObject = Instantiate(BloodPrefab);
            _bloodpool.Add(bloodObject);
            bloodObject.SetActive(false);
        }
    }

    public void Make(Vector3 position, Vector3 normal)
    {
        foreach (GameObject bloodObject in _bloodpool)
        {
            if (bloodObject.activeInHierarchy == false)
            {
                bloodObject.GetComponent<DestroyTime>()?.Init();
                bloodObject.transform.position = position;
                bloodObject.transform.forward = normal;
                bloodObject.SetActive(true);
                break;
            }
        }
    }
}
