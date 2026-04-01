using DG.Tweening;
using Lobby.NavSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class NavController : MonoBehaviour
{
    public List<Path> ways;
    [SerializeField] private Transform _transformObject;

    //[SerializeField] private Path[] _paths;


    private Tween moveTween;
    private Coroutine rotationCoroutine;

    //public void Update()
    //{
    //    if (Keyboard.current.wKey.isPressed)
    //    {
    //        ExecAnim("LobbyPage");
    //    }

    //    if (Keyboard.current.sKey.isPressed)
    //    {
    //        ExecReverseAnim("LobbyPage");
    //    }
    //}

    private Path GetPath(string animName)
    {
        return ways.Where(t => t.PathName == animName).FirstOrDefault();
    }

    public void ExecAnim(string animName)
    {
        Stop();

        Path selectedPath = GetPath(animName);
        StartPath(selectedPath);
    }

    public void ExecReverseAnim(string animName)
    {
        Stop();

        Path selectedPath = GetPath(animName);
        StarReversPath(selectedPath);
    }

    private void StarReversPath(Path selectedPath)
    {
        Transform[] points = selectedPath.Points.Reverse().ToArray();
        Vector3[] positions = points.Select(t => t.position).ToArray();
        moveTween = _transformObject.DOPath(positions, selectedPath.ExecSpeed, PathType.CatmullRom).SetOptions(false).SetEase(Ease.InFlash);
        rotationCoroutine = StartCoroutine(SmoothRotationCoroutine(points, _transformObject));

    }
    private void StartPath(Path selectedPath)
    {
        Transform[] points = selectedPath.Points;
        Vector3[] positions = points.Select(t => t.position).ToArray();
        moveTween = _transformObject.DOPath(positions, selectedPath.ExecSpeed, PathType.CatmullRom).SetOptions(false).SetEase(Ease.InFlash);
        rotationCoroutine = StartCoroutine(SmoothRotationCoroutine(points, _transformObject));

    }

    private void Stop()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }

        if (moveTween != null)
        {
            moveTween.Kill();
            moveTween = null;
        }
    }

    IEnumerator SmoothRotationCoroutine(Transform[] points, Transform objToRotate)
    {
        if (points == null || points.Length < 2)
            yield break;

        int segmentIndex = 0;

        while (segmentIndex < points.Length - 1)
        {
            Transform from = points[segmentIndex];
            Transform to = points[segmentIndex + 1];

            float segmentDist = Vector3.Distance(from.position, to.position);

            while (true)
            {
                float traveled = Vector3.Distance(from.position, objToRotate.position);
                float t = Mathf.Clamp01(traveled / segmentDist);

                // Плавная интерполяция rotation между from и to
                objToRotate.rotation = Quaternion.Slerp(from.rotation, to.rotation, t);

                // Если прошли сегмент — переходим к следующему
                if (t >= 1f) break;

                yield return null;
            }

            segmentIndex++;
        }

        rotationCoroutine = null;
    }
}
