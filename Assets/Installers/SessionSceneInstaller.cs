using Common.systems.SceneStates;
using Common.systems.SceneStates.Graph;
using Common.systems.UI;
using System;
using UnityEngine;
using Zenject;

public class SessionSceneInstaller : MonoInstaller
{
    [SerializeField] private UIManager _uiManager;

    public override void InstallBindings()
    {
        Container.Bind<SceneStateMachine<GameSessionScene>>().AsSingle().NonLazy();
        Container.Bind<IInitializable>().To<SceneStateMachine<GameSessionScene>>().FromResolve();

        Container.Bind<GraphReader>().AsSingle();

        Container.Bind<UIManager>().FromComponentInHierarchy(_uiManager).AsSingle();
        Container.Bind<SessionNetInstaller>().AsTransient();


        Container.BindInterfacesTo<SessionSceneEnterPoint>().AsSingle();
    }
}
