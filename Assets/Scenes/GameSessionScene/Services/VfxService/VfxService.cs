using UnityEngine;

public class VfxService : IVfxService
{
    public void PlayEffect(GameObject prefab, Vector3 position)
    {
        var effect = Object.Instantiate(prefab, position, Quaternion.identity);

        var ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Object.Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Object.Destroy(effect, 5f);
        }
    }
}