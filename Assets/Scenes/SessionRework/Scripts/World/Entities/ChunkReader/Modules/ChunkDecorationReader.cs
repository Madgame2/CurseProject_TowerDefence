using System.Collections.Generic;
using Scenes.SessionRework.Scripts.World.Entities.Chunk.Modules.Base;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Meta.Enum;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.World.Entities.Chunk.Modules
{
    public class ChunkDecorationReader: RenderModuleBase
    {
        //TEMP
        [Header("Settings")] 
        [SerializeField] private GameObject grassPrefab;
        
        private readonly List<Matrix4x4[]> _batches = new List<Matrix4x4[]>();
        
        public override void Setup()
        {
            _batches.Clear();
            var matrices = PlaceDecorations();
            
            for (int i = 0; i < matrices.Count; i += 1023)
            {
                int count = Mathf.Min(1023, matrices.Count - i);
                Matrix4x4[] batch = new Matrix4x4[count];
            
                for (int j = 0; j < count; j++)
                {
                    batch[j] = matrices[i + j];
                }
                _batches.Add(batch);
            }
        }

        public override void Render()
        {
            if (_batches == null || _batches.Count == 0) return;
            
            Mesh mesh = grassPrefab.GetComponent<MeshFilter>().sharedMesh;
            Material material = grassPrefab.GetComponent<MeshRenderer>().sharedMaterial;

            foreach (var batch in _batches)
            {
                if (batch == null || batch.Length == 0) continue;
                
                Graphics.DrawMeshInstanced(
                    mesh, 
                    0, 
                    material, 
                    batch, 
                    batch.Length, 
                    null, 
                    UnityEngine.Rendering.ShadowCastingMode.TwoSided, 
                    true
                );
            }
        }

        private List<Matrix4x4> PlaceDecorations()
        {
            Vector3 pivotOffset = new Vector3(_chunk.Pivot.x, 0, _chunk.Pivot.y) * _chunk.Size;
            
            List<Matrix4x4> grassMatrices = new List<Matrix4x4>();

            for (int x = 0; x < _chunk.Size; x++)
            {
                for (int z = 0; z < _chunk.Size; z++)
                {
                    var decorations = _chunk.GetDecorationInCell(x, z);
                    if (decorations == DecorationType.NONE) continue;
                    
                    float u = 0.5f;
                    float v = 0.5f;
                    
                    GetPointInCell(x, z, u, v, out Vector3 localPos, out Vector3 terrainNormal);
                    Vector3 worldPos = transform.TransformPoint(localPos);

                    if (decorations.HasFlag(DecorationType.GRASS))
                    {
                        Quaternion prefabRot = grassPrefab.transform.rotation;
                        
                        Quaternion finalRotation = prefabRot;
                        
                        Vector3 scale = grassPrefab.transform.localScale;
                        
                        Matrix4x4 matrix = Matrix4x4.TRS(worldPos, finalRotation, scale);
                        
                        grassMatrices.Add(matrix);
                    }
                }
            }

            return grassMatrices;
        }

        private void GetPointInCell(int cellX, int cellZ, float u, float v,
            out Vector3 localPosition,
            out Vector3 normal)
        {
            u = Mathf.Clamp01(u);
            v = Mathf.Clamp01(v);
            
            float hBL = _chunk.GetHeight(cellX, cellZ);         // Bottom-Left  (0, 0)
            float hTL = _chunk.GetHeight(cellX, cellZ + 1);     // Top-Left     (0, 1)
            float hBR = _chunk.GetHeight(cellX + 1, cellZ);     // Bottom-Right (1, 0)
            float hTR = _chunk.GetHeight(cellX + 1, cellZ + 1); // Top-Right    (1, 1)
            
            float height;
            Vector3 rawNormal;
            
            if (u + v <= 1f)
            {
                float dX = hBR - hBL;
                float dZ = hTL - hBL;
                
                height = hBL + u * dX + v * dZ;
                
                rawNormal = new Vector3(-dX, 1f, -dZ);
            }
            else
            {
                float dX = hTR - hTL;
                float dZ = hTR - hBR;
                
                height = (1f - u) * hTL + (1f - v) * hBR + (u + v - 1f) * hTR;
                
                rawNormal = new Vector3(-dX, 1f, -dZ);
            }
            
            Vector3 pivotOffset = new Vector3(_chunk.Pivot.x, 0, _chunk.Pivot.y) * _chunk.Size;
            
            localPosition = new Vector3(cellX + u, height, cellZ + v) - pivotOffset;
            
            normal = rawNormal.normalized;
        }
    }
}