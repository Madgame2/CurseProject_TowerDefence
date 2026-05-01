using UnityEngine;

public class DecorationFactory : MonoBehaviour
{
    [SerializeField] private DecorationConfigDatabase database;

    private void Awake()
    {
        database.Init();
    }

    public GameObject Create(DecorationInstance deco)
    {
        var config = database.Get(deco.id);

        var go = Instantiate(config.prefab);

        return go;
    }
    public void Destroy(DecorationInstance deco)
    {
        if (deco.gameObject != null)
            Destroy(deco.gameObject);
    }
}
