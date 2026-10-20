using System;
using Scenes.SessionRework.Scripts.Network.Meta;
using Scenes.SessionRework.Scripts.Network.Tools.NetObjectSerializer;
using Scenes.SessionRework.Scripts.Network.Tools.NetObjectSerializer.Interfaces;
using UnityEditor;
using UnityEngine;

public class NewMonoBehaviourScript : EditorWindow
{
    private NetObjectDefinition _prefab;
    private string _outputPath = "Assets/NetObjects";
    
    private INetObjectSerializer  _serializer;
    
    [MenuItem("Tools/SerializeNetObjectTool")]
    public static void ShowWindow()
    {
        var window = GetWindow<NewMonoBehaviourScript>("SerializeNetObjectTool");
    }

    private void OnEnable()
    {
        _serializer = new XmlNetObjectSerializer();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField(
            "Net Object Serializer",
            EditorStyles.boldLabel
        );
        
        EditorGUILayout.Space(10);

        DrawPrefabField();
        
        EditorGUILayout.Space(10);

        DrawOutputPathField();
        
        EditorGUILayout.Space(20);

        DrawGenerateButton();
    }

    private void DrawGenerateButton()
    {
        GUI.enabled = _prefab != null &&
                      _prefab.GetComponent<NetObjectDefinition>() != null &&
                      !string.IsNullOrWhiteSpace(_outputPath);

        if (GUILayout.Button(
                "Generate",
                GUILayout.Height(35)))
        {
            Generate();
        }

        GUI.enabled = true;
    }

    private void Generate()
    {
        try
        {
            _serializer.Serialize(
                _prefab,
                _outputPath
            );
            
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog(
                "Success",
                $"NetObject успешно сериализован.\n\n" +
                $"Prefab: {_prefab.name}\n" +
                $"Output: {_outputPath}",
                "OK"
            );
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);

            EditorUtility.DisplayDialog(
                "Serialization Error",
                exception.Message,
                "OK"
            );
        }
    }

    private void DrawOutputPathField()
    {
        EditorGUILayout.LabelField(
            "Output Folder",
            EditorStyles.boldLabel
        );
        
        EditorGUILayout.BeginHorizontal();

        _outputPath = EditorGUILayout.TextField(_outputPath);

        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            BrowseOutputFolder();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawPrefabField()
    {
        EditorGUILayout.LabelField(
            "Net Object Prefab",
            EditorStyles.boldLabel
        );
        
        _prefab = (NetObjectDefinition)EditorGUILayout.ObjectField(
            _prefab,
            typeof(NetObjectDefinition),
            false
        );
        
        if (_prefab == null)
        {
            EditorGUILayout.HelpBox(
                "Укажите prefab с компонентом NetObjectDefinition.",
                MessageType.Info
            );

            return;
        }
    }
    
    
    private void BrowseOutputFolder()
    {
        string path = EditorUtility.OpenFolderPanel(
            "Select output folder",
            "",
            ""
        );

        if (string.IsNullOrEmpty(path))
            return;

        _outputPath = path.Replace('\\', '/');
    }
}
