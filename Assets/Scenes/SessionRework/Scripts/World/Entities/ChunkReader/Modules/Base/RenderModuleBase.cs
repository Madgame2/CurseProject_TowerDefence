using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Entities.Chunk.Modules.Base
{
    public abstract class RenderModuleBase: MonoBehaviour
    {
        protected Model.Chunk _chunk;

        public void Link(ref Model.Chunk chunk)
        {
            _chunk = chunk;
        }

        public abstract void Setup();
        public abstract void Render();
    }
}
