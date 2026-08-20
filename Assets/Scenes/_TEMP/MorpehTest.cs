using Scellecs.Morpeh;
using UnityEngine;

public class MorpehTest : MonoBehaviour
{
    private World world;

    private void Start()
    {
        world = World.Create();

        var entity = world.CreateEntity();

        Debug.Log($"ENTITY: {entity}");
    }
}
