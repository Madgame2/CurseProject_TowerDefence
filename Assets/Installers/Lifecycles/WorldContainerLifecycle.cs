namespace Installers.Lifecycles
{

    using System;
    using Zenject; // Замени на namespace своего ZInject, если он отличается.

    public sealed class WorldContainerLifecycle :
        ITickable,
        IFixedTickable,
        ILateTickable,
        IDisposable
    {
        private DiContainer _worldContainer;

        private InitializableManager _initializableManager;
        private TickableManager _tickableManager;
        private DisposableManager _disposableManager;

        private bool _initialized;
        private bool _disposed;

        public void Attach(DiContainer worldContainer)
        {
            if (_worldContainer != null)
                throw new InvalidOperationException(
                    "World container is already attached.");

            _worldContainer = worldContainer;

            _initializableManager =
                worldContainer.Resolve<InitializableManager>();

            _tickableManager =
                worldContainer.Resolve<TickableManager>();

            _disposableManager =
                worldContainer.Resolve<DisposableManager>();

            _initializableManager.Initialize();
            _initialized = true;
        }

        public void Tick()
        {
            if (_initialized && !_disposed)
                _tickableManager.Update();
        }

        public void FixedTick()
        {
            if (_initialized && !_disposed)
                _tickableManager.FixedUpdate();
        }

        public void LateTick()
        {
            if (_initialized && !_disposed)
                _tickableManager.LateUpdate();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (_initialized)
                _disposableManager.Dispose();

            _worldContainer = null;
            _initializableManager = null;
            _tickableManager = null;
            _disposableManager = null;

            _initialized = false;
        }
    }

}