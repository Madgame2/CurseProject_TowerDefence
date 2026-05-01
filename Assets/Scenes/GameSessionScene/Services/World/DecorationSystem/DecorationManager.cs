using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    private Dictionary<Chank, List<DecorationInstance>> chunkToDecorations = new();
    [SerializeField] private DecorationConfigDatabase config;
    [SerializeField] private DecorationFactory factory;

    public void LoadChankDecorations(Chank chank)
    {
        if (!chunkToDecorations.TryGetValue(chank, out var decorations))
            return;

        foreach( var objects in chank.EntiersRecords)
        {
            foreach (var deco in decorations)
            {
                if (!deco.loadedInChunks.Add(chank))
                    continue;



                if (!deco.IsLoaded)
                {
                    deco.gameObject = factory.Create(deco);
                }
            }
        }
    }

    public void UnloadChankDecorations(Chank chank)
    {
        if (!chunkToDecorations.TryGetValue(chank, out var decorations))
            return;

        foreach (var deco in decorations)
        {
            // если чанка не было — пропускаем
            if (!deco.loadedInChunks.Remove(chank))
                continue;

            if (deco.loadedInChunks.Count == 0)
            {
                factory.Destroy(deco);
                deco.gameObject = null;
            }
        }
    }
}