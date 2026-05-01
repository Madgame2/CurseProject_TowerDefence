using UnityEngine;

[CreateAssetMenu(menuName = "World/Decoration Config")]
public class DecorationConfig : ScriptableObject
{
    public long id;
    public string name;
    public GameObject prefab;

    public bool neverUnload;
}
