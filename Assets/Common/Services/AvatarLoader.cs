using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AvatarLoader : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    private Texture2D _texture;

    /// <summary>
    /// Загрузка из локального файла (byte[])
    /// </summary>
    public void SetFromBytes(byte[] imageData)
    {
        if (!this || !gameObject)
            return;

        if (imageData == null || imageData.Length == 0)
        {
            Debug.LogError("Empty image data");
            return;
        }

        _texture = new Texture2D(2, 2);
        _texture.LoadImage(imageData);

        ApplyToUI();
    }

    /// <summary>
    /// Загрузка из URL
    /// </summary>
    public void LoadFromUrl(string url)
    {
        if (!this || !gameObject)
            return;

        StartCoroutine(LoadFromUrlCoroutine(url));
    }
    private string NormalizeHostToIPv4(string url)
    {
        var uri = new Uri(url);

        // Ищем IPv4 адрес для hostname
        var ipv4 = Dns.GetHostAddresses(uri.Host)
            .FirstOrDefault(x =>
                x.AddressFamily == AddressFamily.InterNetwork);

        if (ipv4 == null)
            return url; // fallback

        return $"{uri.Scheme}://{ipv4}:{uri.Port}{uri.PathAndQuery}";
    }

    private IEnumerator LoadFromUrlCoroutine(string url)
    {
        url = NormalizeHostToIPv4(url);
        Debug.Log(url);
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            request.certificateHandler = new DevCertHandler();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load image: " + request.error);
                yield break;
            }

            _texture = DownloadHandlerTexture.GetContent(request);
            ApplyToUI();
        }
    }

    /// <summary>
    /// Применение текстуры в UI Image
    /// </summary>
    private void ApplyToUI()
    {
        if (targetImage == null)
        {
            Debug.LogError("Target Image is not assigned");
            return;
        }

        Sprite sprite = Sprite.Create(
            _texture,
            new Rect(0, 0, _texture.width, _texture.height),
            new Vector2(0.5f, 0.5f)
        );

        targetImage.sprite = sprite;
        targetImage.preserveAspect = true;
    }
}