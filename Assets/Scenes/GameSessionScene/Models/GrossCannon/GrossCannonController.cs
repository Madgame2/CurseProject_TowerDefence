using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class GrossCannonController : MonoBehaviour
{
    [Inject] private CannonballPool pool;
    [Inject] private VfxService vfxService;


    [SerializeField] private Transform HorizontalAxis;
    [SerializeField] private Transform VerticalAxis;


    [SerializeField] private GameObject _target;


    [SerializeField] private float RotationSpeed = 20f;
    [SerializeField] private float VerticalAmplitude = 10f;
    [SerializeField] private float VerticalSpeed = 2f;
    [SerializeField] private float RotationDuration = 0.25f;


    [SerializeField] private Transform ShootSpawnPoint;
    [SerializeField] private Transform ShootSpawnPoint_1;

    [SerializeField] private GameObject ShootEffect;

    private float _idleRotation;

    public void SetTarget(GameObject target)
    {
        this._target = target;
    }


    private void SetRotation(Quaternion rot)
    {
        Vector3 euler = rot.eulerAngles;

        HorizontalAxis.DOLocalRotate(
            new Vector3(0, euler.y, 0),
            RotationDuration
        );

        VerticalAxis.DOLocalRotate(
            new Vector3(euler.x, 0, 0),
            RotationDuration
        );
    }


    private void SetRotation(GameObject obj)
    {
        Vector3 dir =
            obj.transform.position - HorizontalAxis.position;

        Vector3 flatDir = dir;
        flatDir.y = 0;

        if (flatDir != Vector3.zero)
        {
            Quaternion horizontalRot =
                Quaternion.LookRotation(flatDir) *
                Quaternion.Euler(0, 90f, 0);

            HorizontalAxis.rotation = horizontalRot;
        }

        dir =
            obj.transform.position - VerticalAxis.position;

        Vector3 localDir =
            VerticalAxis.parent.InverseTransformDirection(dir);

        float angleZ =
            Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg;

        Quaternion targetRot =
            Quaternion.Euler(0, 0, angleZ);

        VerticalAxis.localRotation = targetRot;
    }


    private void Update()
    {
        if (_target == null)
        {
            IdleAnimation();
        }
        else
        {
            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        if (_target == null) return;

        Vector3 dir =
            _target.transform.position - HorizontalAxis.position;

        Vector3 flatDir = dir;
        flatDir.y = 0;

        if (flatDir != Vector3.zero)
        {
            Quaternion horizontalRot =
                Quaternion.LookRotation(flatDir) *
                Quaternion.Euler(0, 90f, 0);

            HorizontalAxis.rotation =
                Quaternion.RotateTowards(
                    HorizontalAxis.rotation,
                    horizontalRot,
                    120f * Time.deltaTime
                );
        }

        {


            dir =
                _target.transform.position - VerticalAxis.position;
            // переводим в локальное пространство родителя
            Vector3 localDir = VerticalAxis.parent.InverseTransformDirection(dir);

            // считаем угол вверх/вниз относительно Z
            float angleZ = Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg;

            // применяем ТОЛЬКО Z
            Quaternion targetRot = Quaternion.Euler(0, 0, angleZ);

            VerticalAxis.localRotation =
                Quaternion.RotateTowards(
                    VerticalAxis.localRotation,
                    targetRot,
                    90f * Time.deltaTime
                );
        }
    }

    private void IdleAnimation()
    {
        _idleRotation += RotationSpeed * Time.deltaTime;

        HorizontalAxis.localRotation =
            Quaternion.Euler(0, _idleRotation, 0);



        float angleX =
            Mathf.Sin(Time.time * VerticalSpeed)
            * VerticalAmplitude;

        VerticalAxis.localRotation =
            Quaternion.Euler(0, 0, angleX);
    }

    internal void ShootTo(GameObject npc)
    {
        SetRotation(npc);
        Shoot();
    }

    private void Shoot()
    {
        GameObject obj = pool.Get();

        obj.transform.position = ShootSpawnPoint.position;
        obj.transform.rotation = ShootSpawnPoint.rotation;

        var cannonball = obj.GetComponent<Cannonball>();
        Vector3 direction = ShootSpawnPoint.transform.position - ShootSpawnPoint_1.transform.position;
        cannonball.Init(direction.normalized);

        vfxService.PlayEffect(ShootEffect, ShootSpawnPoint.position);
    }
}
