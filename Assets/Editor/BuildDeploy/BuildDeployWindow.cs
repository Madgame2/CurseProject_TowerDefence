using UnityEngine;


using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildDeployWindow : EditorWindow
{
    // IP второго ноутбука
    private const string TargetIp = "10.184.85.140";

    private const int TargetPort = 8081;

    private const string BuildName = "TowerDefence";

    private bool _isBuilding;

    [MenuItem("Tools/Build & Deploy")]
    private static void Open()
    {
        GetWindow<BuildDeployWindow>("Build & Deploy");
    }

    private void OnGUI()
    {
        GUILayout.Label("Windows Build & Deploy", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Target", $"{TargetIp}:{TargetPort}");

        GUI.enabled = !_isBuilding;

        if (GUILayout.Button("Build & Deploy", GUILayout.Height(40)))
        {
            _ = BuildAndDeploy();
        }

        GUI.enabled = true;

        if (_isBuilding)
            EditorGUILayout.HelpBox("Building and deploying...", MessageType.Info);
    }

    private async Task BuildAndDeploy()
    {
        _isBuilding = true;

        try
        {
            string buildDir = Path.Combine(
                Directory.GetParent(Application.dataPath).FullName,
                "BuildDeploy"
            );

            string outputDir = Path.Combine(buildDir, "Windows");
            string archivePath = Path.Combine(buildDir, "build.zip");

            Directory.CreateDirectory(buildDir);

            if (Directory.Exists(outputDir))
                Directory.Delete(outputDir, true);

            // Собираем список сцен из Build Settings
            string[] scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

            if (scenes.Length == 0)
                throw new Exception("No enabled scenes in Build Settings.");

            string exePath = Path.Combine(outputDir, BuildName + ".exe");

            Directory.CreateDirectory(outputDir);

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = exePath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            Debug.Log("Building Windows client...");

            BuildReport report = BuildPipeline.BuildPlayer(options);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception($"Build failed: {report.summary.result}");

            // Создаём ZIP
            if (File.Exists(archivePath))
                File.Delete(archivePath);

            ZipFile.CreateFromDirectory(outputDir, archivePath);

            Debug.Log("Build completed. Uploading...");

            // Отправляем архив на второй ноутбук
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(30)
            };

            string url = $"http://{TargetIp}:{TargetPort}/deploy";

            using var fileStream = File.OpenRead(archivePath);
            using var content = new StreamContent(fileStream);

            using HttpResponseMessage response =
                await client.PostAsync(url, content);

            string responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            Debug.Log($"Deploy successful: {responseText}");

            EditorUtility.DisplayDialog(
                "Build & Deploy",
                "Build successfully deployed!",
                "OK"
            );
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);

            EditorUtility.DisplayDialog(
                "Build & Deploy",
                ex.Message,
                "OK"
            );
        }
        finally
        {
            _isBuilding = false;
            Repaint();
        }
    }
}

