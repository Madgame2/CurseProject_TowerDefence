using System;
using System.Drawing;
using Scenes.SessionRework.Scripts.World.Core.Meta;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.World.Interfaces;
using Scenes.SessionRework.Scripts.World.Model;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Color = UnityEngine.Color;

namespace Scenes.SessionRework.Scripts.World.Entities
{
    public class ChunkReader : MonoBehaviour, IChunkReader
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private BiomeTextureConfig _biomeTextureConfig;

        [Inject] private readonly IWorldProvider _worldProvider;

        private Chunk _chunk;
        private bool _isLinked;
        
        private Texture2D _biomeTexture;
        private MaterialPropertyBlock _propertyBlock;

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        public Texture2D BiomeTexture => _biomeTexture;

        public void Link(ref Chunk chunkModel)
        {
            _chunk = chunkModel;
            _isLinked = true;

            GenerateMesh();

            _biomeTexture = CreateBiomeTexture(_worldProvider,_chunk.ChunkPos);

            if (_propertyBlock == null)
                _propertyBlock = new MaterialPropertyBlock();

            _meshRenderer.GetPropertyBlock(_propertyBlock);

            _propertyBlock.SetTexture("_BiomeMap", _biomeTexture);
            _propertyBlock.SetFloat("_ChunkSize", _chunk.Size);

            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }

        public void Clear()
        {
            _isLinked = false;

            if (_biomeTexture != null)
            {
                Destroy(_biomeTexture);
                _biomeTexture = null;
            }

            _chunk = default;
        }

        private void GenerateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "Procedural Terrain";

            int vertCount = _chunk.RawSize * _chunk.RawSize;

            Vector3[] vertices = new Vector3[vertCount];
            Vector2[] uvs0 = new Vector2[vertCount];

            int size = _chunk.Size;
            Vector3 pivotOffset = new Vector3(_chunk.Pivot.x, 0, _chunk.Pivot.y) * size;

            for (int z = 0, i = 0; z < _chunk.RawSize; z++)
            {
                for (int x = 0; x < _chunk.RawSize; x++)
                {
                    float height = _chunk.GetHeight(x, z);
                    vertices[i] = new Vector3(x, height, z) - pivotOffset;

                    uvs0[i] = new Vector2((float)x / size, (float)z / size);

                    i++;
                }
            }

            int[] triangles = new int[size * size * 6];
            int vert = 0;
            int tris = 0;

            for (int z = 0; z < size; z++)
            {
                for (int x = 0; x < size; x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + _chunk.RawSize;
                    triangles[tris + 2] = vert + 1;

                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + _chunk.RawSize;
                    triangles[tris + 5] = vert + _chunk.RawSize + 1;

                    vert++;
                    tris += 6;
                }

                vert++;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.SetUVs(0, uvs0);

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            _meshFilter.mesh = mesh;
        }
        private byte GetTextureLayer(BiomeType biome)
        {
            return biome switch
            {
                BiomeType.GREEN_MEADOWS => 0,
                BiomeType.STONE_AREA => 1,
                _ => 0
            };
        }

        private Texture2D CreateBiomeTexture(IBiomeProvider worldProvider, Vector2Int chunkWorldPos)
        {
            int size = _chunk.Size;
            int paddedSize = size + 2;

            Texture2D texture = new Texture2D(paddedSize, paddedSize, TextureFormat.R8, false, true);
            
            texture.filterMode = FilterMode.Point; 
            texture.wrapMode = TextureWrapMode.Clamp;

            Color32[] pixels = new Color32[paddedSize * paddedSize];
            
            int pivotOffsetX = Mathf.RoundToInt(_chunk.Pivot.x * size);
            int pivotOffsetZ = Mathf.RoundToInt(_chunk.Pivot.y * size);
            
            for (int z = -1; z <= size; z++)
            {
                for (int x = -1; x <= size; x++)
                {
                    int globalX = chunkWorldPos.x * size + x - pivotOffsetX;
                    int globalZ = chunkWorldPos.y * size + z - pivotOffsetZ;

                    BiomeType biome = worldProvider.GetBiomeGlobal(globalX, globalZ);
                    byte layerIndex = GetTextureLayer(biome);

                    int arrayX = x + 1;
                    int arrayZ = z + 1;
            
                    pixels[arrayX + arrayZ * paddedSize] = new Color32(layerIndex, 0, 0, 255);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, false);

            return texture;
        }   
        
        private void OnDrawGizmos()
        {
            if (!_isLinked) return;

            int size = _chunk.Size;

            Vector3 pivotOffset = new Vector3(_chunk.Pivot.x, 0, _chunk.Pivot.y) * size;

            Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
            Vector3 chunkCenter = transform.position;

            Gizmos.DrawWireCube(chunkCenter, new Vector3(size, 0.1f, size));

            Gizmos.color = Color.green;

            for (int x = 0; x < _chunk.RawSize; x++)
            {
                for (int y = 0; y < _chunk.RawSize; y++)
                {
                    float height = _chunk.GetHeight(x, y);
                    Vector3 worldPos = transform.position + new Vector3(x, height, y) - pivotOffset;

                    Gizmos.DrawSphere(worldPos, 0.05f);

                    if (x < _chunk.RawSize - 1)
                    {
                        float nextHeightX = _chunk.GetHeight(x + 1, y);
                        Vector3 nextWorldPosX = transform.position + new Vector3(x + 1, nextHeightX, y) - pivotOffset;
                        Gizmos.DrawLine(worldPos, nextWorldPosX);
                    }

                    if (y < _chunk.RawSize - 1)
                    {
                        float nextHeightZ = _chunk.GetHeight(x, y + 1);
                        Vector3 nextWorldPosZ = transform.position + new Vector3(x, nextHeightZ, y + 1) - pivotOffset;
                        Gizmos.DrawLine(worldPos, nextWorldPosZ);
                    }
                }
            }
        }
    }
}