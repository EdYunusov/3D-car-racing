using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraController : MonoBehaviour, IDependency<RaceStateTraker>
{
    [SerializeField] private Car car;

    [SerializeField] private new Camera camera;
    [SerializeField] private CarCameraFollow follower;
    [SerializeField] private SpeedVisualEffect shaker;
    [SerializeField] private CarCameraFovCorrection fovCorrection;
    [SerializeField] private CarCameraPathFollower pathFollower;

    private RaceStateTraker raceStateTraker;
    public void Construct(RaceStateTraker obj) => this.raceStateTraker = obj;

    private void Awake()
    {
        follower.SetProperties(car, camera);
        shaker.SetProperties(car, camera);
        fovCorrection.SetProperties(car, camera);

    }

    private void Start()
    {
        raceStateTraker.PreperignStart += OnPreperinStarted;
        raceStateTraker.Complited += OnComplited;

        follower.enabled = false;
        pathFollower.enabled = true;
    }

    private void OnDestroy()
    {
        raceStateTraker.PreperignStart -= OnPreperinStarted;
        raceStateTraker.Complited -= OnComplited;
    }

    private void OnComplited()
    {
        pathFollower.enabled = true;
        pathFollower.StartMoveToNearstPoint();
        pathFollower.SetLookTarget(car.transform);

        follower.enabled = false;
    }

    private void OnPreperinStarted()
    {
        follower.enabled = true;
        pathFollower.enabled = false;
    }
}
