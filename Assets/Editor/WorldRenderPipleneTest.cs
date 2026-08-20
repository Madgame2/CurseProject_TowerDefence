using System;
using Cysharp.Threading.Tasks;
using Scenes.SessionRework.Scripts.GameWorld.Core;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces;
using UnityEditor;
using UnityEngine;
using Zenject;

public class WorldRenderPipleneTest : EditorWindow
{
    private Vector2Int selectedChunk;
    
    [MenuItem("Tools/Chunk Generator")]
    public static void ShowWindow()
    {
        GetWindow<WorldRenderPipleneTest>("Chunk Generator");
    }

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Запустите игру (Play Mode), чтобы управлять Zenject-контекстом.", MessageType.Info);
            return;
        }
        
        GUILayout.Label("Generate Chunk: ");
        selectedChunk = EditorGUILayout.Vector2IntField("Chunk pos:", selectedChunk);
        
        GUILayout.Space(10);

        if (GUILayout.Button("Generate Chunk"))
        {
            GenerateChunk();
        }
    }

    private void GenerateChunk()
    {
        var sceneContext = FindFirstObjectByType<SceneContext>();        
        if (sceneContext == null)
        {
            Debug.LogError("[DebugWindow] SceneContext не найден на сцене! Убедитесь, что Zenject настроен.");
            return;
        }
        
        DiContainer container = sceneContext.Container;
        if (container == null)
        {
            Debug.LogError("[DebugWindow] Контейнер Zenject еще не построился.");
            return;
        }

        DiContainer worldContainer = container.TryResolve<WorldHolder>().WorldContainer.DiContainer;
        if (worldContainer == null)
        {
            Debug.LogError("[DebugWindow] IChunkOrchestrator не найден в контейнере");
            return;
        }

        var chunkOrchestrator = worldContainer.TryResolve<IChunkOrchestrator>();
        
        chunkOrchestrator.LoadChunk(selectedChunk).Forget();
    }
}
