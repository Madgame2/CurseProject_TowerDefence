using System.Collections.Generic;
using UnityEngine;

public class CannonballPool: MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int size = 20;


    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject Get()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        // если закончились — можно расширять пул
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }
}
