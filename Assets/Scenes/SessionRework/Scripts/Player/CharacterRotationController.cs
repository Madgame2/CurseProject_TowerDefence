using System;
using UnityEngine;

public class CharacterRotationController : MonoBehaviour
{
    

    [Header("Ссылки")]
    public Transform targetLook;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform characterRoot;

    [Header("Настройки веса")]
    [Range(0, 1)] public float totalWeight = 1.0f; // Общий вес IK
    [Range(0, 1)] public float bodyWeight = 0.5f;  // Насколько сильно гнется туловище (0.3 - 0.6 обычно в самый раз)
    [Range(0, 1)] public float headWeight = 1.0f;  // Насколько сильно поворачивается голова

    [Header("Настройки поворота")]
    [SerializeField] private float turnOffset = 0f;
    [SerializeField] private float turnThreshold = 80f;   // Градус, при котором персонаж начинает переступать
    [SerializeField] private float weightBlendSpeed = 5f; // Скорость, с которой IK затухает и возвращается

    private float targetWeight = 1.0f; // Целевой вес, к которому стремится totalWeight
    private bool isTurning = false;

    private void Update()
    {
        if (animator == null || targetLook == null || characterRoot == null) return;

        HandleTurning();
        BlendWeight();
    }
    private void HandleTurning()
    {
            Vector3 targetDir = targetLook.position - characterRoot.position;
            targetDir.y = 0;

            if (targetDir == Vector3.zero) return;
            targetDir.Normalize();

            Vector3 characterRootDirection = Quaternion.AngleAxis(turnOffset, Vector3.up) * characterRoot.forward;
            float rootAngle = Vector3.SignedAngle(characterRootDirection, targetDir, Vector3.up);

            bool isMoving = animator.GetFloat("Speed") > 0.1f;

            if (!isMoving && !isTurning)
            {
                if (rootAngle > turnThreshold)
                {
                    StartTurn("TurnRight90"); // Имена триггеров из Animator Controller
                }
                else if (rootAngle < -turnThreshold)
                {
                    StartTurn("TurnLeft90");
                }
            }

        if (isTurning)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 1. Убеждаемся, что Аниматор РЕАЛЬНО перешел в стейт поворота
            bool isPlayingTurn = stateInfo.IsName("TurnRight90") || stateInfo.IsName("TurnLeft90");

            if (isPlayingTurn)
            {
                // 2. Если мы уже крутимся, ждем момента, когда начнется переход (Transition) 
                // обратно в базовое состояние, либо анимация почти доиграет до конца
                if (animator.IsInTransition(0) || stateInfo.normalizedTime > 0.95f)
                {
                    EndTurn();
                }
            }
        }
    }

    private void StartTurn(string triggerName)
    {
        animator.ResetTrigger("TurnRight90");
        animator.ResetTrigger("TurnLeft90");

        isTurning = true;
        targetWeight = 0f; // Даем команду IK плавно отключиться
        animator.SetTrigger(triggerName);
    }
    private void EndTurn()
    {
        isTurning = false;
        targetWeight = 1f; // Даем команду IK плавно включиться обратно
    }

    private void BlendWeight()
    {
        // Плавно интерполируем текущий вес к целевому
        totalWeight = Mathf.MoveTowards(totalWeight, targetWeight, weightBlendSpeed * Time.deltaTime);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || targetLook == null) return;

        // Оптимизация: если вес IK почти равен нулю, вообще не передаем его в аниматор
        if (totalWeight <= 0.001f)
        {
            animator.SetLookAtWeight(0f);
            return;
        }

        animator.SetLookAtPosition(targetLook.position);
        animator.SetLookAtWeight(totalWeight, bodyWeight, headWeight, 0f, 0.45f);
    }
}
