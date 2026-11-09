using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArcherReferences : MonoBehaviour
{
    [field: SerializeField]
    public GameObject Arrow { get; private set; }
    
    [field: SerializeField]
    public Rig ArchRicg { get; private set; }
}
