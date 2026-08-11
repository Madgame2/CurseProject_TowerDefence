using UnityEngine;

namespace Editor.Interfaces
{
    public interface ITextureProvider
    {
        Texture2D[] GetTextures();
    }
}