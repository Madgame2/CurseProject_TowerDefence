using System;
using UnityEngine;
using Zenject;

public class SessionSceneInstaller : MonoInstaller
{
    [SerializeField] private VignetteController vignetteController;

    public override void InstallBindings()
    {

        Container.Bind<SessionNetInstaller>().AsTransient();



        Container.Bind<VignetteController>()
                    .FromInstance(vignetteController)
                    .AsSingle();


        Container.BindInterfacesTo<SessionSceneEnterPoint>().AsSingle();
    }
}
