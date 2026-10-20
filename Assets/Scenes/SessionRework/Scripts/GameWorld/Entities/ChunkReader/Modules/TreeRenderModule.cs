using System.Collections.Generic;
using Scenes.SessionRework.Scripts.GameWorld.Entities.Chunk.Modules.Base;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Entities.Chunk.Modules
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class TreeRenderModule : RenderModuleBase
    {
        [SerializeField] private GameObject _treePrefab;

        private class TreePart
        {
            public Mesh Mesh;
            public int SubmeshIndex;
            public Material Material;
            public Matrix4x4 LocalMatrix;
        }


        private List<TreePart> _treeParts;
        private List<Vector3> _treePositions = new List<Vector3>();
        private Bounds _localPrefabBounds;

        private List<Matrix4x4> _visibleInstanceMatrices = new List<Matrix4x4>();
        private Matrix4x4[] _batchBuffer = new Matrix4x4[1023];


        public override void Setup()
        {
            _treePositions.Clear();
            _treeParts = CachePrefabParts(_treePrefab, out _localPrefabBounds);

            foreach (var tree in _chunk.StaticTreeDatas)
            {
                
                var position = new Vector3(tree.LocalPosition.x, 0, tree.LocalPosition.y);
                if (position == Vector3.zero)
                {
                    Debug.Log("!!!!!!");
                }
                
                _treePositions.Add(position);
            }
        }

        public override void Render(Plane[] frustumPlanes)
        {
            if (_treeParts == null || _treeParts.Count == 0 || _treePositions.Count == 0) return;

            _visibleInstanceMatrices.Clear();

            for (int i = 0; i < _treePositions.Count; i+=1)
            {
                Vector3 position = _treePositions[i];

                Bounds worldBounds = new Bounds(_localPrefabBounds.center + position, _localPrefabBounds.size);

                if (GeometryUtility.TestPlanesAABB(frustumPlanes, worldBounds))
                {
                    Matrix4x4 instanceMatrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
                    _visibleInstanceMatrices.Add(instanceMatrix);
                }
            }
            
            if (_visibleInstanceMatrices.Count == 0) return;
            
            for (int p = 0; p < _treeParts.Count; p++)
            {
                DrawInstancedPart(_treeParts[p], _visibleInstanceMatrices);
            }
        }

        private void DrawInstancedPart(TreePart part, List<Matrix4x4> instanceMatrices)
        {
            int totalVisible = instanceMatrices.Count;
            
            for (int i = 0; i < totalVisible; i += 1023)
            {
                int count = Mathf.Min(1023, totalVisible - i);

                for (int j = 0; j < count; j++)
                {
                    _batchBuffer[j] = instanceMatrices[i + j] * part.LocalMatrix;
                }

                Graphics.DrawMeshInstanced(
                    part.Mesh,
                    part.SubmeshIndex,
                    part.Material,
                    _batchBuffer,
                    count
                );
            }
        }

        private List<TreePart> CachePrefabParts(GameObject prefab, out Bounds combinedBounds)
        {
            var parts = new List<TreePart>();
            combinedBounds = new Bounds();

            if (prefab == null) return parts;

            var renderers = prefab.GetComponentsInChildren<MeshRenderer>(true);
            Matrix4x4 rootWorldToLocal = prefab.transform.worldToLocalMatrix;

            
            bool hasBounds = false;

            int index = 0;
            foreach (var renderer in renderers)
            {
                var filter = renderer.GetComponent<MeshFilter>();
                if (filter == null || filter.sharedMesh == null) continue;

                Mesh mesh = filter.sharedMesh;
                Material[] materials = renderer.sharedMaterials;

                Matrix4x4 childLocalMatrix = rootWorldToLocal * renderer.transform.localToWorldMatrix;

                Bounds partLocalBounds = TransformBounds(childLocalMatrix, mesh.bounds);
                if (!hasBounds)
                {
                    combinedBounds = partLocalBounds;
                    hasBounds = true;
                }
                else
                {
                    combinedBounds.Encapsulate(partLocalBounds);
                }
                
                for (int submesh = 0; submesh < mesh.subMeshCount; submesh++)
                {
                    Material mat = materials[Mathf.Min(submesh, materials.Length - 1)];
                    if (mat == null) continue;
                    
                    mat.enableInstancing = true;

                    parts.Add(new TreePart
                    {
                        Mesh = mesh,
                        SubmeshIndex = submesh,
                        Material = mat,
                        LocalMatrix = childLocalMatrix
                    });
                }

                index++;
            }

            parts.RemoveAll(x => !x.Mesh.name.Contains("LOD1"));
            
            return parts;
        }

        private Bounds TransformBounds(Matrix4x4 matrix, Bounds localBounds)
        {
            Vector3 center = matrix.MultiplyPoint(localBounds.center);
            Vector3 extents = localBounds.extents;

            Vector3 axisX = matrix.GetColumn(0);
            Vector3 axisY = matrix.GetColumn(1);
            Vector3 axisZ = matrix.GetColumn(2);

            Vector3 newExtents = new Vector3(
                Mathf.Abs(axisX.x) * extents.x + Mathf.Abs(axisY.x) * extents.y + Mathf.Abs(axisZ.x) * extents.z,
                Mathf.Abs(axisX.y) * extents.x + Mathf.Abs(axisY.y) * extents.y + Mathf.Abs(axisZ.y) * extents.z,
                Mathf.Abs(axisX.z) * extents.x + Mathf.Abs(axisY.z) * extents.y + Mathf.Abs(axisZ.z) * extents.z
            );

            return new Bounds(center, newExtents * 2f);
        }
    }
}