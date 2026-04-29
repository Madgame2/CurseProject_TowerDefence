using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayersController : MonoBehaviour
{
    [Inject] private readonly PlayerStorage _playersStorage;
    [SerializeField] private float positionSmooth = 10f;
    [SerializeField] private float rotationSmooth = 10f;
    [SerializeField] private float snapDistance = 5f;

    private readonly Dictionary<string, PlayerViewState> _states = new();

    public void handlePlayerNewState(PlayerState state)
    {
        var playerEntity = _playersStorage.GetPlayer(state.Id);
        if (playerEntity == null)
            return;

        if (!_states.TryGetValue(state.Id, out var viewState))
        {
            viewState = new PlayerViewState();
            _states[state.Id] = viewState;
        }

        viewState.targetPosition = new Vector3(
            state.Position.X,
            state.Position.Y,
            state.Position.Z
        );

        viewState.targetRotation = Quaternion.Euler(
            state.Rotation.X,
            state.Rotation.Y,
            state.Rotation.Z
        );

        var velocity = new Vector3(
            state.Velocity.X,
            state.Velocity.Y,
            state.Velocity.Z
        );

        if (playerEntity.gameObject.TryGetComponent<CharacterAnimController>(out var anim))
        {
            anim.SetVelocity(velocity);
        }
    }

    void Update()
    {
        foreach (var kvp in _states)
        {
            var playerId = kvp.Key;
            var state = kvp.Value;

            var player = _playersStorage.GetPlayer(playerId);
            if (player == null)
                continue;

            Transform playerTransform = player.transform;

            // snap check
            if (Vector3.Distance(playerTransform.position, state.targetPosition) > snapDistance)
            {
                playerTransform.position = state.targetPosition;
            }
            else
            {
                playerTransform.position = Vector3.Lerp(
                    playerTransform.position,
                    state.targetPosition,
                    Time.deltaTime * positionSmooth
                );
            }

            playerTransform.rotation = Quaternion.Slerp(
                playerTransform.rotation,
                state.targetRotation,
                Time.deltaTime * rotationSmooth
            );
        }
    }


    private class PlayerViewState
    {
        public Vector3 targetPosition;
        public Quaternion targetRotation;
    }

}
