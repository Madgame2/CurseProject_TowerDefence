using Editor.Interfaces;
using UnityEditor;
using UnityEngine;

public class TextureArrayCreator: EditorWindow
{
    private ScriptableObject _textureSource;
    private string _savePath = "Assets/BiomeTexturesArray.asset";
    
    [MenuItem("Tools/Texture Array Creator")]
    public static void ShowWindow()
    {
        var window = GetWindow<TextureArrayCreator>("Chunk Generator");
        window.minSize = new Vector2(400, 200);
    }
    private void OnGUI()
    {
        GUILayout.Label("Генерация Texture2DArray", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        _textureSource = (ScriptableObject)EditorGUILayout.ObjectField(
            "Источник текстур (SO)", 
            _textureSource, 
            typeof(ScriptableObject), 
            false);
        
        bool isValidProvider = false;
        if (_textureSource != null)
        {
            if (_textureSource is ITextureProvider)
            {
                isValidProvider = true;
            }
            else
            {
                EditorGUILayout.HelpBox("Ошибка: Выбранный объект не реализует интерфейс ITextureProvider!", MessageType.Error);
            }
        }
        
        EditorGUILayout.Space();
        
        _savePath = EditorGUILayout.TextField("Путь сохранения", _savePath);
        
        EditorGUILayout.Space();
        
        EditorGUI.BeginDisabledGroup(!isValidProvider || string.IsNullOrEmpty(_savePath));
        if (GUILayout.Button("Сгенерировать Texture2DArray", GUILayout.Height(30)))
        {
            GenerateArray();
        }
        EditorGUI.EndDisabledGroup();
    }
    
    private void GenerateArray()
    {
        var provider = _textureSource as ITextureProvider;
        Texture2D[] textures = provider.GetTextures();

        if (textures == null || textures.Length == 0)
        {
            Debug.LogError("Массив текстур пуст!");
            return;
        }

        int width = textures[0].width;
        int height = textures[0].height;
        TextureFormat format = textures[0].format;

        for (int i = 0; i < textures.Length; i++)
        {
            if (textures[i] == null)
            {
                Debug.LogError($"Текстура по индексу {i} отсутствует!");
                return;
            }
            if (textures[i].width != width || textures[i].height != height)
            {
                Debug.LogError($"Текстура '{textures[i].name}' имеет другой размер! Все текстуры должны быть одинакового размера.");
                return;
            }
            if (textures[i].format != format)
            {
                Debug.LogWarning($"Текстура '{textures[i].name}' имеет формат {textures[i].format}, ожидался {format}. Это может вызвать ошибку.");
            }
        }

        Texture2DArray textureArray = new Texture2DArray(width, height, textures.Length, format, true)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Repeat,
            anisoLevel = 4
        };
        
        for (int i = 0; i < textures.Length; i++)
        {
            for (int mip = 0; mip < textures[i].mipmapCount; mip++)
            {
                Graphics.CopyTexture(textures[i], 0, mip, textureArray, i, mip);
            }
        }
        
        AssetDatabase.CreateAsset(textureArray, _savePath);
        AssetDatabase.SaveAssets();
        
        EditorGUIUtility.PingObject(textureArray);
        
        Debug.Log($"<color=green>Texture2DArray успешно создан:</color> {_savePath}");
    }
}
