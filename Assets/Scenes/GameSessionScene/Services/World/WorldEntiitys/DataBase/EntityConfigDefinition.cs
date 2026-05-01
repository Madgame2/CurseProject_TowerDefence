using UnityEngine;

[CreateAssetMenu(fileName = "EntityConfig", menuName = "World/EntityConfig")]
public class EntityConfigDefinition : ScriptableObject
{
    public string NetName;
    public GameObject prefab;
    public bool neverUnload;
}
