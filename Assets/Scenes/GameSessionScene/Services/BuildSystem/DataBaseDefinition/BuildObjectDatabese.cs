using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildObjectDatabase", menuName = "BuildSystem/BuildObjectDatabase")]
public class BuildObjectDatabase : ScriptableObject
{
    public BuildObjectDef[] buildObjects;

    public BuildObjectDef GetByType(BuildObjectsEnum type)
    {
        return buildObjects.FirstOrDefault(i=>i.type== type);
    }
}
