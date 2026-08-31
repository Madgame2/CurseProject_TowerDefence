using Editor.Interfaces.Math;
using Scenes.SessionRework.Scripts.GameWorld.Core;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using UnityEditor;
using UnityEngine;
using Zenject;

public class DebugClentMathTools : EditorWindow
{
    private Vector2 _inputCoordinate;
    private float _resultHeight;

    private bool _showDebugPoint = false;
    private Vector3 _debugPointPosition;
    
    [MenuItem("Tools/Debug client math tools")]
    public static void ShowWindow()
    {
        var window = GetWindow<DebugClentMathTools>("Debug client math tools");
        window.minSize = new Vector2(400, 200);
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    // Отписываемся при закрытии окна
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    
    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Запустите игру (Play Mode), чтобы управлять Zenject-контекстом.", MessageType.Info);
            return;
        }
        
        GUILayout.Space(10);

        RenderChunkHeightMapTools();
    }

    private void RenderChunkHeightMapTools()
    {
        GUILayout.Label("Chunk Height Map", EditorStyles.boldLabel);
        
        _inputCoordinate = EditorGUILayout.Vector2Field("Input Coordinate", _inputCoordinate);
        
        if (GUILayout.Button("Get Height"))
        {
            _resultHeight = GetChunkHeightMapData(_inputCoordinate);
            
            _debugPointPosition = new Vector3(_inputCoordinate.x, _resultHeight, _inputCoordinate.y);
            _showDebugPoint = true;
            
            SceneView.RepaintAll();
        }

        GUILayout.Space(5);
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("Result Height", _resultHeight);
        EditorGUI.EndDisabledGroup();
        
        GUILayout.Space(15);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!_showDebugPoint) 
            return;

        // Сохраняем текущий цвет, чтобы не сломать чужую отрисовку
        Color defaultColor = Handles.color;
        
        Handles.color = Color.red;
        
        // Рисуем сферу (размер 0.5f, можешь поменять под масштаб своей игры)
        Handles.SphereHandleCap(0, _debugPointPosition, Quaternion.identity, 0.5f, EventType.Repaint);
        
        // Рисуем луч вверх, чтобы точку было легко найти даже если она под землей или в траве
        Handles.DrawLine(_debugPointPosition, _debugPointPosition + Vector3.up * 10f);

        // Возвращаем цвет
        Handles.color = defaultColor;
    }
    
    private float GetChunkHeightMapData(Vector2 inputCoordinate)
    {
        var sceneContext = FindFirstObjectByType<SceneContext>();        
        if (sceneContext == null)
        {
            Debug.LogError("[DebugWindow] SceneContext не найден на сцене! Убедитесь, что Zenject настроен.");
            return 0;
        }
        
        DiContainer container = sceneContext.Container;
        if (container == null)
        {
            Debug.LogError("[DebugWindow] Контейнер Zenject еще не построился.");
            return 0;
        }

        DiContainer worldContainer = container.TryResolve<WorldHolder>().WorldContainer.DiContainer;
        if (worldContainer == null)
        {
            Debug.LogError("[DebugWindow] IChunkOrchestrator не найден в контейнере");
            return 0;
        }

        var landscapeGraphRoot = worldContainer.TryResolve<ILandscapeGraphNode>();
        if (landscapeGraphRoot == null)
        {
            Debug.LogError("[DebugWindow] ILandscapeGraphNode не найден в контейнере");
        }
        
        float gridSize = 1f; // Измени на свой реальный шаг сетки

        // 2. Находим нижний левый угол ячейки сетки, в которой находится наша координата
        float startX = Mathf.Floor(inputCoordinate.x / gridSize) * gridSize;
        float startY = Mathf.Floor(inputCoordinate.y / gridSize) * gridSize;

        // 3. Получаем высоты графа для всех 4-х углов текущего квадрата (ячейки)
        float h00 = landscapeGraphRoot.Evaluate(startX, startY);                       // Нижний левый
        float h10 = landscapeGraphRoot.Evaluate(startX + gridSize, startY);            // Нижний правый
        float h01 = landscapeGraphRoot.Evaluate(startX, startY + gridSize);            // Верхний левый
        float h11 = landscapeGraphRoot.Evaluate(startX + gridSize, startY + gridSize); // Верхний правый

        // 4. Узнаем дробное смещение внутри квадрата (от 0.0 до 1.0)
        float percentX = (inputCoordinate.x - startX) / gridSize;
        float percentY = (inputCoordinate.y - startY) / gridSize;
        
        Vector3 p1, p2, p3;
        
        if (percentX > percentY) // Попали в нижний-правый треугольник
        {
            p1 = new Vector3(startX, h00, startY);                               // (0,0)
            p2 = new Vector3(startX + gridSize, h10, startY);                    // (1,0)
            p3 = new Vector3(startX + gridSize, h11, startY + gridSize);         // (1,1)
        }
        else // Попали в верхний-левый треугольник
        {
            p1 = new Vector3(startX, h00, startY);                               // (0,0)
            p2 = new Vector3(startX + gridSize, h11, startY + gridSize);         // (1,1)
            p3 = new Vector3(startX, h01, startY + gridSize);                    // (0,1)
        }
        
        return MathTools.GetHeightOnTriangle(p1, p2, p3, inputCoordinate);
    }
}
