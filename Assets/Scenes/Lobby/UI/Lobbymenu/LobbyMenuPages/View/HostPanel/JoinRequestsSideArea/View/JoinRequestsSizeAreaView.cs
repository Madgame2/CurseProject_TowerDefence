using Common.systems.ProfileSystem.Entities;
using Common.systems.UI.View;
using System;
using UnityEngine;
using Zenject;

public class JoinRequestsSizeAreaView : ViewBase<JoinRequestsSizeAreaViewModel>
{
    [SerializeField] private GameObject _templatePrefab;
    [SerializeField] private Transform _rootContainer;

    [Inject] private readonly DiContainer _container;

    protected override void OnViewModelAssigned()
    {
        ViewModel.onNewRequest += newRequesthandler;
    }

    private void newRequesthandler(Profile profile)
    {
        GameObject request_gameObject = _container.InstantiatePrefab(_templatePrefab, _rootContainer);
        if(request_gameObject.TryGetComponent<JoinRequest>(out JoinRequest joinRequest))
        {
            joinRequest.Init(profile, 30);
            joinRequest.onApplyPlayerRequest += ViewModel.ApplyRequest;
        }
    }

    public override void Cleanup()
    {
        ViewModel.onNewRequest += newRequesthandler;

        ViewModel.cleanUp();
    }
}
