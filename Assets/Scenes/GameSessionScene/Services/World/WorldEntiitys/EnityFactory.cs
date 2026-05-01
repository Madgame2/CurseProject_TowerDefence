using UnityEngine;
using Zenject;

public class EnityFactory : MonoBehaviour
{
    [SerializeField] private EntityDataBase entityDataBase;
    [Inject] private DiContainer _container;

    public GameObject PlaysEntity(EntityConfigDefinition config, Vector3 position)
    {

        var obj = _container.InstantiatePrefab(config.prefab, this.gameObject.transform);
        obj.transform.position = position*10;

        return obj;
    }
}
