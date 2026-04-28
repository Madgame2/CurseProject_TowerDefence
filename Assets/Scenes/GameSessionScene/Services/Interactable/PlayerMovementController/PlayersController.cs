using UnityEngine;

public class PlayersController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float positionSmooth = 10f;
    [SerializeField] private float rotationSmooth = 10f;
    [SerializeField] private float snapDistance = 5f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public void handlePlayerNewState(PlayerState state)
    {

        Debug.Log(state.Rotation.Y);
        targetPosition = new Vector3(
            state.Position.X,
            state.Position.Y,
            state.Position.Z
        );

        targetRotation = Quaternion.Euler(
            state.Rotation.X,
            state.Rotation.Y,
            state.Rotation.Z
        );

        Vector3 velocity = new Vector3(state.Velocity.X,
            state.Velocity.Y,
            state.Velocity.Z);

        player.gameObject.TryGetComponent<CharacterAnimController>(out CharacterAnimController component);
        component.SetVelocity(velocity);
    }

    void Update()
    {
        // если слишком далеко — телепорт (анти-лаг)
        if (Vector3.Distance(player.position, targetPosition) > snapDistance)
        {
            player.position = targetPosition;
        }
        else
        {
            // плавное движение
            player.position = Vector3.Lerp(
                player.position,
                targetPosition,
                Time.deltaTime * positionSmooth
            );
        }

        // плавный поворот
        player.rotation = Quaternion.Slerp(
            player.rotation,
            targetRotation,
            Time.deltaTime * rotationSmooth
        );
    }
}
