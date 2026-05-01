using System.Collections.Generic;
using UnityEngine;

public class DecorationInstance
{
    public string id;

    public HashSet<Chank> loadedInChunks = new();

    public GameObject gameObject;

    public int loadedChunksCount = 0;

    public bool IsLoaded => gameObject != null;
}
