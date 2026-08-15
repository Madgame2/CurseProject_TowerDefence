using UnityEngine;

using UnityEngine;
using UnityEditor;
using System.IO;

public class TexturePackerWindow : EditorWindow
{
    private enum Mode
    {
        MergeAlbedoAndAlpha,
        PackMaskMap
    }

    private Mode currentMode = Mode.MergeAlbedoAndAlpha;

    // Ссылки для объединения Albedo + Alpha
    private Texture2D albedoTex;
    private Texture2D alphaTex;

    // Ссылки для упаковки каналов (Mask Map)
    private Texture2D aoTex;
    private Texture2D glossTex;
    private Texture2D metallicTex;
    private Texture2D maskAlphaTex;

    [MenuItem("Tools/Texture Packer & Merger")]
    public static void ShowWindow()
    {
        GetWindow<TexturePackerWindow>("Texture Packer");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        currentMode = (Mode)GUILayout.Toolbar((int)currentMode, new string[] { "Albedo + Alpha", "Pack Mask Map (AO/Gloss/Alpha)" });
        GUILayout.Space(15);

        if (currentMode == Mode.MergeAlbedoAndAlpha)
        {
            DrawAlbedoAlphaGUI();
        }
        else
        {
            DrawMaskMapGUI();
        }
    }

    private void DrawAlbedoAlphaGUI()
    {
        EditorGUILayout.LabelField("Слияние Albedo (RGB) + Alpha Mask (R) в один RGBA PNG", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        albedoTex = (Texture2D)EditorGUILayout.ObjectField("Albedo (RGB)", albedoTex, typeof(Texture2D), false);
        alphaTex = (Texture2D)EditorGUILayout.ObjectField("Alpha Mask (R)", alphaTex, typeof(Texture2D), false);

        GUILayout.Space(15);

        EditorGUI.BeginDisabledGroup(albedoTex == null || alphaTex == null);
        if (GUILayout.Button("Склеить и сохранить PNG", GUILayout.Height(35)))
        {
            MergeAlbedoAndAlpha();
        }
        EditorGUI.EndDisabledGroup();
    }

    private void DrawMaskMapGUI()
    {
        EditorGUILayout.LabelField("Упаковка каналов в единую маску (Linear)", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("R = Ambient Occlusion\nG = Gloss / Smoothness\nB = Metallic (опционально)\nA = Alpha Mask", MessageType.Info);
        EditorGUILayout.Space(5);

        aoTex = (Texture2D)EditorGUILayout.ObjectField("Красный (R) - Ambient Occlusion", aoTex, typeof(Texture2D), false);
        glossTex = (Texture2D)EditorGUILayout.ObjectField("Зеленый (G) - Gloss / Smoothness", glossTex, typeof(Texture2D), false);
        metallicTex = (Texture2D)EditorGUILayout.ObjectField("Синий (B) - Metallic (необязательно)", metallicTex, typeof(Texture2D), false);
        maskAlphaTex = (Texture2D)EditorGUILayout.ObjectField("Альфа (A) - Alpha Mask", maskAlphaTex, typeof(Texture2D), false);

        GUILayout.Space(15);

        EditorGUI.BeginDisabledGroup(aoTex == null && glossTex == null && maskAlphaTex == null);
        if (GUILayout.Button("Упаковать маску и сохранить PNG", GUILayout.Height(35)))
        {
            PackMaskMap();
        }
        EditorGUI.EndDisabledGroup();
    }

    private void MergeAlbedoAndAlpha()
    {
        string defaultPath = GetDefaultSavePath(albedoTex, "_Combined.png");
        string savePath = EditorUtility.SaveFilePanelInProject("Сохранить Albedo с Альфой", Path.GetFileNameWithoutExtension(defaultPath), "png", "Выберите куда сохранить текстуру", defaultPath);

        if (string.IsNullOrEmpty(savePath)) return;

        Texture2D readAlbedo = CreateReadableTexture(albedoTex);
        Texture2D readAlpha = CreateReadableTexture(alphaTex);

        int width = readAlbedo.width;
        int height = readAlbedo.height;

        Color[] albedoPixels = readAlbedo.GetPixels();
        Color[] alphaPixels = ScaleOrGetPixels(readAlpha, width, height);

        Color[] resultPixels = new Color[albedoPixels.Length];

        for (int i = 0; i < albedoPixels.Length; i++)
        {
            Color c = albedoPixels[i];
            float alpha = alphaPixels[i].r; // Используем красный канал черно-белой маски
            resultPixels[i] = new Color(c.r, c.g, c.b, alpha);
        }

        Texture2D resultTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        resultTex.SetPixels(resultPixels);
        resultTex.Apply();

        byte[] pngBytes = resultTex.EncodeToPNG();
        File.WriteAllBytes(savePath, pngBytes);

        DestroyImmediate(readAlbedo);
        DestroyImmediate(readAlpha);
        DestroyImmediate(resultTex);

        AssetDatabase.Refresh();

        // Автоматическая настройка импорта для Albedo
        TextureImporter importer = AssetImporter.GetAtPath(savePath) as TextureImporter;
        if (importer != null)
        {
            importer.alphaIsTransparency = true;
            importer.sRGBTexture = true;
            importer.SaveAndReimport();
        }

        Debug.Log($"[TexturePacker] Текстура успешно сохранена: {savePath}");
    }

    private void PackMaskMap()
    {
        Texture2D refTex = aoTex != null ? aoTex : (glossTex != null ? glossTex : maskAlphaTex);
        if (refTex == null) return;

        string defaultPath = GetDefaultSavePath(refTex, "_PackedMask.png");
        string savePath = EditorUtility.SaveFilePanelInProject("Сохранить Packed Mask", Path.GetFileNameWithoutExtension(defaultPath), "png", "Выберите куда сохранить текстуру", defaultPath);

        if (string.IsNullOrEmpty(savePath)) return;

        int width = refTex.width;
        int height = refTex.height;

        Color[] aoPixels = aoTex != null ? ScaleOrGetPixels(CreateReadableTexture(aoTex), width, height) : null;
        Color[] glossPixels = glossTex != null ? ScaleOrGetPixels(CreateReadableTexture(glossTex), width, height) : null;
        Color[] metallicPixels = metallicTex != null ? ScaleOrGetPixels(CreateReadableTexture(metallicTex), width, height) : null;
        Color[] alphaPixels = maskAlphaTex != null ? ScaleOrGetPixels(CreateReadableTexture(maskAlphaTex), width, height) : null;

        Color[] resultPixels = new Color[width * height];

        for (int i = 0; i < resultPixels.Length; i++)
        {
            float r = aoPixels != null ? aoPixels[i].r : 1.0f;       // AO (по умолчанию 1)
            float g = glossPixels != null ? glossPixels[i].r : 0.0f;  // Gloss (по умолчанию 0)
            float b = metallicPixels != null ? metallicPixels[i].r : 0.0f; // Metallic
            float a = alphaPixels != null ? alphaPixels[i].r : 1.0f;  // Alpha

            resultPixels[i] = new Color(r, g, b, a);
        }

        Texture2D resultTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        resultTex.SetPixels(resultPixels);
        resultTex.Apply();

        byte[] pngBytes = resultTex.EncodeToPNG();
        File.WriteAllBytes(savePath, pngBytes);

        DestroyImmediate(resultTex);

        AssetDatabase.Refresh();

        // Настройка импорта для Packed Mask (обязательно Linear)
        TextureImporter importer = AssetImporter.GetAtPath(savePath) as TextureImporter;
        if (importer != null)
        {
            importer.sRGBTexture = false; // Маски математически корректно обрабатываются только в Linear
            importer.alphaIsTransparency = false;
            importer.SaveAndReimport();
        }

        Debug.Log($"[TexturePacker] Маска успешно упакована: {savePath}");
    }

    // Обход ограничения Read/Write Enabled у текстур через RenderTexture
    private Texture2D CreateReadableTexture(Texture2D source)
    {
        if (source == null) return null;

        RenderTexture renderTex = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear
        );

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;

        Texture2D readableText = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);

        return readableText;
    }

    // Масштабирование пикселей, если размеры входных текстур различаются
    private Color[] ScaleOrGetPixels(Texture2D source, int targetWidth, int targetHeight)
    {
        if (source == null) return null;

        if (source.width == targetWidth && source.height == targetHeight)
        {
            return source.GetPixels();
        }

        RenderTexture renderTex = RenderTexture.GetTemporary(
            targetWidth,
            targetHeight,
            0,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear
        );

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;

        Texture2D scaledTex = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
        scaledTex.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        scaledTex.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);

        Color[] pixels = scaledTex.GetPixels();
        DestroyImmediate(scaledTex);
        return pixels;
    }

    private string GetDefaultSavePath(Texture2D sourceTex, string suffix)
    {
        if (sourceTex == null) return "Assets/";
        string path = AssetDatabase.GetAssetPath(sourceTex);
        if (string.IsNullOrEmpty(path)) return "Assets/";

        string dir = Path.GetDirectoryName(path);
        string filename = Path.GetFileNameWithoutExtension(path) + suffix;
        return Path.Combine(dir, filename).Replace("\\", "/");
    }
}
