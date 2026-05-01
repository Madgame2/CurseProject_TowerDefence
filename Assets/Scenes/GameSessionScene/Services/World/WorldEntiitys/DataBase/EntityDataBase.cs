using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityDataBase", menuName = "World/EntityDataBase")]
public class EntityDataBase : ScriptableObject
{
    public EntityConfigDefinition[] entityConfigDefinitions;


    public EntityConfigDefinition getById(string id) {
        return entityConfigDefinitions.FirstOrDefault(i=>i.NetName == id);
    }
}
