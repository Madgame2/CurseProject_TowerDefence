using System;
using System.Collections.Generic;
using Scenes.SessionRework.Scripts.GameWorld.Entities;
using Scenes.SessionRework.Scripts.GameWorld.Entities.Chunk;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;
using UnityEditor;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(ChunkReader))]
    public class ChunkReaderEditor : UnityEditor.Editor
    {
        private Texture2D _biomePreviewTexture;
        
        private readonly Dictionary<BiomeType, Color> _biomeColors = new Dictionary<BiomeType, Color>()
        {
            { BiomeType.GREEN_MEADOWS, new Color(0.13f, 0.69f, 0.3f) },
            { BiomeType.STONE_AREA, Color.gray }
        };
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ChunkReader chunkReader = (ChunkReader)target;
            RenderBiomeMap(chunkReader);
        }

        private void RenderBiomeMap(ChunkReader chunkReader)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Biome Map Inspector", EditorStyles.boldLabel);

            if (chunkReader.BiomeTexture == null)
            {
                EditorGUILayout.HelpBox("Biome Texture is NULL", MessageType.Warning);
                return;
            }

            // 1. Отрисовка настраиваемой Легенды (Legend)
            DrawLegend(chunkReader.BiomeTexture);

            EditorGUILayout.Space(10);

            // 2. Отрисовка самой карты и обработка наведения мыши
            DrawMapWithHover(chunkReader.BiomeTexture);
        }

        private void DrawLegend(Texture2D sourceTexture)
        {
            EditorGUILayout.LabelField("Legend", EditorStyles.boldLabel);

            bool colorChanged = false;

            foreach (BiomeType biome in Enum.GetValues(typeof(BiomeType)))
            {
                if(biome == BiomeType.NONE) continue;
                
                if (!_biomeColors.ContainsKey(biome))
                    _biomeColors[biome] = Color.magenta;
                
                EditorGUI.BeginChangeCheck();
                
                Color newColor = EditorGUILayout.ColorField(biome.ToString(), _biomeColors[biome]);
                
                if (EditorGUI.EndChangeCheck())
                {
                    _biomeColors[biome] = newColor;
                    colorChanged = true;
                }
            }
            
            if (colorChanged || _biomePreviewTexture == null)
            {
                GeneratePreview(sourceTexture);
            }
        }

        private void DrawMapWithHover(Texture2D sourceTexture)
        {
            float mapSize = 256f;
            Rect mapRect = GUILayoutUtility.GetRect(mapSize, mapSize, GUILayout.ExpandWidth(false));
            
            if (_biomePreviewTexture != null)
            {
                GUI.DrawTexture(mapRect, _biomePreviewTexture, ScaleMode.ScaleToFit);
            }
            
            Vector2 mousePos = Event.current.mousePosition;
            BiomeType hoveredBiome = BiomeType.NONE;
            Vector2Int hoveredCoord = Vector2Int.zero;
            bool isHovered = false;

            if (mapRect.Contains(mousePos))
            {
                isHovered = true;
                
                float relativeX = (mousePos.x - mapRect.x) / mapRect.width;
                float relativeY = 1f - ((mousePos.y - mapRect.y) / mapRect.height);
                
                int texX = Mathf.Clamp(Mathf.FloorToInt(relativeX * sourceTexture.width), 0, sourceTexture.width - 1);
                int texY = Mathf.Clamp(Mathf.FloorToInt(relativeY * sourceTexture.height), 0, sourceTexture.height - 1);
                
                hoveredCoord = new Vector2Int(texX, texY);
                
                Color32 rawPixel = sourceTexture.GetPixel(texX, texY);
                hoveredBiome = (BiomeType)(rawPixel.r+1);
                
                float cellWidth = mapRect.width / sourceTexture.width;
                float cellHeight = mapRect.height / sourceTexture.height;
                
                Rect cellRect = new Rect(
                    mapRect.x + texX * cellWidth,
                    mapRect.y + (sourceTexture.height - 1 - texY) * cellHeight,
                    cellWidth,
                    cellHeight
                );
                
                EditorGUI.DrawRect(cellRect, new Color(1f, 1f, 1f, 0.4f));
                
                if (Event.current.type == EventType.MouseMove)
                {
                    Repaint();
                }
            }
            
            if (isHovered)
            {
                EditorGUILayout.HelpBox($"Pos: [{hoveredCoord.x}, {hoveredCoord.y}] | Biome: {hoveredBiome}", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("Наведите курсор на карту для инспекции ячейки", MessageType.None);
            }
        }


        private void GeneratePreview(Texture2D sourceTexture)
        {
            int width = sourceTexture.width;
            int height = sourceTexture.height;
            
            if (_biomePreviewTexture == null || _biomePreviewTexture.width != width || _biomePreviewTexture.height != height)
            {
                _biomePreviewTexture = new Texture2D(width, height, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point
                };
            }
            
            Color32[] sourcePixels = sourceTexture.GetPixels32();
            Color32[] previewPixels = new Color32[sourcePixels.Length];
            
            for (int i = 0; i < sourcePixels.Length; i++)
            {
                byte layerIndex = sourcePixels[i].r;
                BiomeType biome = (BiomeType)(layerIndex+1);
                
                if (_biomeColors.TryGetValue(biome, out Color color))
                {
                    previewPixels[i] = color;
                }
                else
                {
                    previewPixels[i] = Color.magenta;
                }
            }

            _biomePreviewTexture.SetPixels32(previewPixels);
            _biomePreviewTexture.Apply(false, false);
        }
    }
}