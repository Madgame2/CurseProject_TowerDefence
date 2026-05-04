using System;
using UnityEngine;
using Zenject;

public class CardViewModel
{
    [Inject] private BuildSystem _buildSystem;


    public void UpdateGhost(Vector3 WorldPos, BuildObjectsEnum objType)
    {
        _buildSystem.UpdatePreview(WorldPos, objType);
    }

    internal void PlaceCardHandler(Vector3 worldPos, BuildObjectsEnum objType)
    {
        _buildSystem.PlaceRequestForBuilding(worldPos, objType);
    }
}
