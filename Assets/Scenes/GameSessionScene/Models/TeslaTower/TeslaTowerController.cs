using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TeslaTowerController : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject LightPrefab;

    [Inject] private NpcManager _npcManager;

    internal void ProcessAttack(TeslaTowerAttackDTO payload)
    {
        if (payload?.tree == null)
            return;

        StartCoroutine(PlayLightning(payload.tree));
    }

    private IEnumerator PlayLightning(NpcIdTree root)
    {
        if (root == null)
            yield break;

        float delayPerLevel = 0.25f;

        var currentLevel = new List<NpcIdTree> { root };

        Vector3 start = spawnPosition.position;

        var rootNpc = _npcManager.GetNpc(root.id);
        if (rootNpc == null)
            yield break;

        StartCoroutine(SpawnLightning(start, GetTargetPos(rootNpc), delayPerLevel));

        yield return new WaitForSeconds(delayPerLevel);

        while (currentLevel.Count > 0)
        {
            var nextLevel = new List<NpcIdTree>();

            foreach (var node in currentLevel)
            {
                var parentNpc = _npcManager.GetNpc(node.id);
                if (parentNpc == null)
                    continue;

                Vector3 from = GetTargetPos(parentNpc);

                foreach (var child in node.children)
                {
                    var childNpc = _npcManager.GetNpc(child.id);
                    if (childNpc == null)
                        continue;

                    Vector3 to = GetTargetPos(childNpc);

                    StartCoroutine(SpawnLightning(from, to, delayPerLevel));

                    nextLevel.Add(child);
                }
            }

            currentLevel = nextLevel;

            yield return new WaitForSeconds(delayPerLevel);
        }
    }

    private Vector3 GetTargetPos(GameObject npc)
    {
        if (npc.TryGetComponent<Skeleton>(out var skeleton))
            return skeleton.LightningTarget.position;

        return npc.transform.position;
    }

    private IEnumerator SpawnLightning(Vector3 from, Vector3 to, float duration)
    {
        GameObject light = Instantiate(LightPrefab);

        Transform start = light.transform.Find("LightningStart");
        Transform end = light.transform.Find("LightningEnd");

        if (start == null || end == null)
        {
            Destroy(light);
            yield break;
        }

        start.position = from;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            end.position = Vector3.Lerp(from, to, t);

            yield return null;
        }

        end.position = to;

        Destroy(light, 0.1f);
    }

    private List<LightningSegment> BuildSegments(NpcIdTree root)
    {
        var segments = new List<LightningSegment>();

        Vector3 start = spawnPosition.position;

        // 1. соединяем spawn → root
        var rootNpc = _npcManager.GetNpc(root.id);
        segments.Add(new LightningSegment
        {
            from = start,
            to = rootNpc.transform.position
        });

        // 2. рекурсивно строим дерево
        BuildRecursive(root, segments);

        return segments;
    }

    private void BuildRecursive(NpcIdTree node, List<LightningSegment> segments)
    {
        var parentNpc = _npcManager.GetNpc(node.id);
        var parentPos = parentNpc.transform.position;

        foreach (var child in node.children)
        {
            var childNpc = _npcManager.GetNpc(child.id);

            segments.Add(new LightningSegment
            {
                from = parentPos,
                to = childNpc.transform.position
            });

            BuildRecursive(child, segments);
        }
    }
}


public struct LightningSegment
{
    public Vector3 from;
    public Vector3 to;
}